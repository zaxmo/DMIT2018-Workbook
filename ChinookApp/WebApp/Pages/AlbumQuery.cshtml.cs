using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

#region Additional Namespaces
using ChinookSystem.BLL;
using ChinookSystem.Models;
#endregion

namespace WebApp.Pages
{
    public class AlbumQueryModel : PageModel
    {
        private readonly AlbumServices _services;

        [BindProperty]
        public AlbumItem Album { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? albumid { get; set; }

        [TempData]
        public string FeedBackMessage { get; set; }

        public AlbumQueryModel(AlbumServices services)
        {
            _services = services;
        }
        public void OnGet()
        {
            if (albumid.HasValue)
            {
                Album = _services.Albums_GetAlbumById((int)albumid);
            }
        }

        public IActionResult OnPost()
        {
            if (!albumid.HasValue)
            {
                FeedBackMessage = "You did not supply an album id";
            }
            return RedirectToPage(new { albumid = albumid });
        }
    }
}
