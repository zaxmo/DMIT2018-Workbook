using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


#region Additional 
using ChinookSystem.Models;
using ChinookSystem.DAL;
using ChinookSystem.Entities;
using Microsoft.EntityFrameworkCore.ChangeTracking;
#endregion

namespace ChinookSystem.BLL
{
    public class AlbumServices
    {
        #region Constructor and DI variable setup
        private readonly ChinookContext _context;

        internal AlbumServices(ChinookContext context)
        {
            _context = context;
        }
        #endregion

        #region Queries
        public AlbumItem Albums_GetAlbumById(int albumid)
        {
            //linq to entity therefore you need to access the DbSet in your
            //      context class
            AlbumItem info = _context.Albums
                            .Where(x => x.AlbumId == albumid)
                            .Select(x => new AlbumItem
                            {
                                AlbumId = x.AlbumId,
                                Title = x.Title,
                                ArtistId = x.ArtistId,
                                ReleaseYear = x.ReleaseYear,
                                ReleaseLabel = x.ReleaseLabel
                            }).FirstOrDefault();
            return info;
        }

        public List<AlbumItem> Albums_GetAlbumsByGenre(int genreid,
                                            int pageNumber,
                                            int pagesize,
                                            out int totalcount)
        {
            IEnumerable<AlbumItem> albums = _context.Tracks
                                .Where(x => x.GenreId == genreid &&
                                        x.AlbumId.HasValue)
                                .Select(x => new AlbumItem
                                {
                                    AlbumId = (int)x.AlbumId,
                                    Title = x.Album.Title,
                                    ArtistId = x.Album.ArtistId,
                                    ReleaseYear = x.Album.ReleaseYear,
                                    ReleaseLabel = x.Album.ReleaseLabel
                                })
                                .Distinct()
                                .OrderBy(x => x.Title);
            //Determine the size of the whole collection w.r.t. the query
            totalcount = albums.Count();
            //limit the actually nummber of records returned from the database
            // depending on the page number and page size
            //calcuate the number of rows to skip
            //page 1= skip 0 rows, page 2 = skip Page Size;
            //  page n = skip (n - 1) * page size
            int skipRows = (pageNumber - 1) * pagesize;
            //the query has yet to be actually executed
            //Linq queries are "Lazy Loaders"
            //We will force the execution on sql by using .ToList()
            //we will inform sql to Skip(n rows) and Take(pagesize rows)
            return albums.Skip(skipRows).Take(pagesize).ToList();
        }

        public List<AlbumItem> Albums_GetAlbumsByTitle(string title,
                                     int pageNumber,
                                     int pagesize,
                                     out int totalcount)
        {
            IEnumerable<AlbumItem> albums = _context.Albums
                                .Where(x => x.Title.Contains(title))
                                .Select(x => new AlbumItem
                                {
                                    AlbumId = x.AlbumId,
                                    Title = x.Title,
                                    ArtistId = x.ArtistId,
                                    ReleaseYear = x.ReleaseYear,
                                    ReleaseLabel = x.ReleaseLabel
                                })
                                .OrderBy(x => x.Title);
            totalcount = albums.Count();
            int skipRows = (pageNumber - 1) * pagesize;
            return albums.Skip(skipRows).Take(pagesize).ToList();
        }
        #endregion

        #region Add,Update and Delete
        public int AddAlbum(AlbumItem item)
        {
            //this method will return the new album id if the add was successful
            //REMINDER: AlbumItem is NOT an entity; it is a view model class
            //          This means you MUST move the data from the view model class
            //              to an instance of your desired entity

            //add a business rule to the method
            //     rule: no album with the same title, same year, same artist
            //     result: this will be considered a duplicate album

            //how can one do such a test
            //1) use a search loop pattern: set a flag as found or not found
            //2) use can use Linq and test the result of a query: .FirstOrDefault()
            Album exist = _context.Albums
                            .Where(x => x.Title.Equals(item.Title)
                                     && x.ArtistId == item.ArtistId
                                     && x.ReleaseYear == item.ReleaseYear)
                            .FirstOrDefault();
            if (exist == null)
            {
                throw new Exception("Album already exists on file");
            }

            //setup the entity instance with the data from the view model parameter
            //NOTE: Album has a identity pkey; therefore one does NOT need to set
            //      the AlbumId
            exist = new Album
            {
                Title = item.Title,
                ArtistId = item.ArtistId,
                ReleaseYear = item.ReleaseYear,
                ReleaseLabel = item.ReleaseLabel
            };
            //stage add in local memory
            _context.Add(exist);
            //do any validation within the entity (validation anotation)
            //send stage request to the database for processing
            _context.SaveChanges();
            return exist.AlbumId;
        }
        public int UpdateAlbum(AlbumItem item)
        {
            Album exist = _context.Albums
                            .Where(x => x.AlbumId == item.AlbumId)
                            .FirstOrDefault();
            if (exist == null)
            {
                throw new Exception("Album does not exist on file");
            }
            //setup the entity instance with the data from the view model parameter
            //NOTE: For an update you need the pkey value
            exist = new Album
            {
                AlbumId = item.AlbumId,
                Title = item.Title,
                ArtistId = item.ArtistId,
                ReleaseYear = item.ReleaseYear,
                ReleaseLabel = item.ReleaseLabel
            };
            //stage add in local memory
            EntityEntry<Album> updating = _context.Entry(exist);
            updating.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            //do any validation within the entity (validation anotation)
            //send stage request to the database for processing
            //the returned value is the number of rows altered
            return _context.SaveChanges();
            
        }

        public int DeleteAlbum(AlbumItem item)
        {
            Album exist = _context.Albums
                            .Where(x => x.AlbumId == item.AlbumId)
                            .FirstOrDefault();
            if (exist == null)
            {
                throw new Exception("Album already has been removed from the file");
            }
            exist = new Album
            {
                AlbumId = item.AlbumId,
                Title = item.Title,
                ArtistId = item.ArtistId,
                ReleaseYear = item.ReleaseYear,
                ReleaseLabel = item.ReleaseLabel
            };
            //stage add in local memory
            EntityEntry<Album> deleting = _context.Entry(exist);
            deleting.State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
            //send stage request to the database for processing
            //the returned value is the number of rows altered
            return _context.SaveChanges();
        }

        #endregion
    }
}
