using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

#region Additional Namespaces
using ChinookSystem.BLL;
using ChinookSystem.Models;
using Microsoft.Extensions.Logging;
using WebApp.Helpers;
#endregion

namespace WebApp.Pages
{
    public class AlbumListTableQueryModel : PageModel
    {
        #region Private variables and DI constructor
        private readonly AlbumServices _albumservices;
        private readonly GenreServices _genreservices;
        
        [TempData]
        public string FeedBackMessage { get; set; }

        public AlbumListTableQueryModel(AlbumServices albumservices, 
                                        GenreServices genreservices)
        {
            _albumservices = albumservices;
            _genreservices = genreservices;
            
        }
        #endregion

        [BindProperty]
        public List<AlbumItem> Albums { get; set; }

        [BindProperty]
        public List<SelectionList> Genres  { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? genreid { get; set; }

        //installing
        //my desired page size
        private const int PAGE_SIZE = 5;
        //instance of the Paginator
        public Paginator Pager { get; set; }
        //total rows in the complete query collection
        public int TotalCount { get; set; }

        //currentPage will appear on your Url as a Get parameter
        // url address...?currentPage=n
        public void OnGet(int? currentPage)
        {
            //for your drop down list, you must retrieve the list on each pass
            Genres = _genreservices.Genre_List();


            if (genreid.HasValue && genreid > 0)
            {
                //installing the paginator
                int pageNumber = currentPage.HasValue ? currentPage.Value : 1;
                //call our paginator to setup data needed for paging
                PageState current = new(pageNumber, PAGE_SIZE);
                //temporary int to hold the results of the query's total collection size
                int totalcount;

                //using Jazz results in a total query collection count of 13

                //for efficiency of data being transferred, we will pass in the
                //  current page number and the desired page size to the query
                //the total number of records for the whole query collection will
                //  be returned as an out parameter. This value is needed by the
                //  Paginator to set up its display
                //the returned collection will only have the rows of the whole query
                //  collection that will be actually shown (PAGE_SIZE or less)
                Albums = _albumservices.Albums_GetAlbumsByGenre((int)genreid,
                                                    pageNumber, PAGE_SIZE,
                                                    out totalcount);

                //move the int primitive value to a property for use in the Paginator
                TotalCount = totalcount;

                //set my Paginator instance
                Pager = new(TotalCount, current);
            }
        }

        public IActionResult OnPost()
        {
            if(!genreid.HasValue || genreid == 0)
            {
                FeedBackMessage = "You did not select a genre";
            }
            return RedirectToPage(new { genreid = genreid });
        }
    }
}
