using System;
using System.Net;

namespace Web.ViewModels
{
    public class ErrorViewModel
    {
        public ErrorViewModel()
        {
            ImageUrl = "~/images/pinedax.png";
        }

        public HttpStatusCode HttpStatusCode { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string ImageUrl { get; set; }
    }
}