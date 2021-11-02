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
    public class CRUDQueryModel : PageModel
    {
        #region Service variable(s), FeedBack and DI constructor
        private readonly AlbumServices _albumservices;

        [TempData]
        public string FeedBackMessage { get; set; }

        public CRUDQueryModel(AlbumServices albumservices)
        {
            _albumservices = albumservices;

        }
        #endregion

        #region Paginator variables
        //my desired page size
        private const int PAGE_SIZE = 5;
        //instance of the Paginator
        public Paginator Pager { get; set; }
        #endregion

        #region Local Page Variables
        [BindProperty]
        public List<AlbumItem> Albums { get; set; }

        [BindProperty(SupportsGet = true)]
        public string partialtitle { get; set; }
        #endregion
        
        public void OnGet(int? currentPage)
        {
            if (partialtitle != null && partialtitle.Trim().Length > 0)
            {
                int pageNumber = currentPage.HasValue ? currentPage.Value : 1;
                PageState current = new(pageNumber, PAGE_SIZE);
                int totalcount;
                Albums = _albumservices.Albums_GetAlbumsByTitle(partialtitle,
                                                    pageNumber, PAGE_SIZE,
                                                    out totalcount);
                Pager = new(totalcount, current);
            }
        }
    }
}
