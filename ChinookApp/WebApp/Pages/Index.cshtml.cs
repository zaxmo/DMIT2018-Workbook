
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#region Additional Namespaces
using ChinookSystem.BLL;
using ChinookSystem.Models;
#endregion



namespace WebApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly AboutServices _services;

        public DatabaseVersion DbVersion { get; set; }

        //on the constructor of your Model class, injecture any service you
        //  require for this page
        //the default service injection is the ILogger class
        //we need to have access to the services (methods) in the AboutServices BLL class
        public IndexModel(ILogger<IndexModel> logger,
                            AboutServices services)
        {
            _logger = logger;
            _services = services;
        }

        public void OnGet()
        {
            DbVersion = _services.GetDataVersion();
        }
    }
}
