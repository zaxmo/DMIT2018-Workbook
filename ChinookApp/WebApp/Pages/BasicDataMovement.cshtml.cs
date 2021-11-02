using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages
{
    public class BasicDataMovementModel : PageModel
    {
        //data field
        public string MyName;

        //properties

        [TempData]
        public string FeedBackMessage { get; set; }

        [BindProperty(SupportsGet =true)]
        public int? theInput { get; set; }

        //constructors


        //Behaviours (aka methods, events)
        public void OnGet()
        { 
            //execute in response to a Get request from the user (browser)
            //when the page is first accessed, it issues a Get Request
            //when the page is refreshed, WITHOUT a Post, it issues a Get Request
            //when the page is retrieved in response to a form POST, the Get Request
            //  is NOT automatically requested.
            Random rnd = new Random();
            int oddeven = rnd.Next(0, 25);
            if (oddeven % 2 == 0)
            {
                MyName = $"Don is {oddeven}";
            }
            else
            {
                MyName = null;
            }
        }

        public IActionResult OnPost()
        {
            //IF you do NOT use asp-page-handler on your buttons in your form
            //  the general method=post will request the OnPost() event
            Thread.Sleep(2000);
            string buttonValue = Request.Form["theButton"];
            FeedBackMessage = buttonValue;

            //the IActionResult of RedirectToPage() sends an OnGet request to
            //    be executed after the OnPost is complete
            //without and href, the RedirectToPage defaults to the current page
            return RedirectToPage();
        }

        public IActionResult OnPostAButton()
        {
            //if you are using the asp-page-handler(xxxx) then your post events
            //  will have a method name of OnPostxxxx()
            Thread.Sleep(1000);
            FeedBackMessage = $"You pressed the handler for the A button with an input value of {theInput}";
            return RedirectToPage(new { theInput = theInput });
        }

        public IActionResult OnPostBButton()
        {
            Thread.Sleep(1000);
            FeedBackMessage = $"You pressed the handler for the B button with an input value of {theInput}";
            //create an anonymous object and initialize using the object initialize a property with the value you wish to retain.
            //there is NO class name, thus an anonymous object
            return RedirectToPage(new { theInput = theInput });
        }
    }
}
