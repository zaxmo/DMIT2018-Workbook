
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


#region Additional 
using ChinookSystem.Models;
using ChinookSystem.DAL;
#endregion

namespace ChinookSystem.BLL
{
    public class AboutServices
    {
        #region Constructor and DI variable setup
        private readonly ChinookContext _context;

        internal AboutServices(ChinookContext context)
        {
            _context = context;
        }
        #endregion

        #region Queries
        public DatabaseVersion GetDataVersion()
        {
            DatabaseVersion results = _context.DbVersions
                                .Select(x => new DatabaseVersion
                                {
                                    Version = x.Major + "." + x.Minor + "." + x.Build,
                                    ReleaseDate = x.ReleaseDate
                                }
                                )
                                .FirstOrDefault();
            return results;
        }
        #endregion
    }
}
