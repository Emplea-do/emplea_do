using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Web.ViewModels;

namespace Web.Controllers
{
	public class ErrorController : Controller
    {
        /// <summary>
        /// HTTP ERROR 400 (BAD REQUEST)
        /// </summary>
        [Route("error/400")]
        public ActionResult Http400()
        {
            var viewModel = new ErrorViewModel
            {
                HttpStatusCode = HttpStatusCode.BadRequest,
                Title = "Lo siento, no encontré lo que buscabas.",
                SubTitle = "No te preocupes, estaremos arreglandolo en breve.",
            };

            return View("Index", viewModel);
        }

        /// <summary>
        /// HTTP ERROR 403 (FORBIDDEN)
        /// </summary>
        [Route("error/403")]
        public ActionResult Http403()
        {
            var viewModel = new ErrorViewModel
            {
                HttpStatusCode = HttpStatusCode.Forbidden,
                Title = "Lo siento, no tienes permisos para ver esto.",
                SubTitle = "Si sigues intentando tendré que tomar cartas en el asunto.",
            };

            return View("Index", viewModel);
        }

        /// <summary>
        /// HTTP ERROR 404 (NOT FOUND)
        /// </summary>
        [Route("error/404")]
        public ActionResult Http404()
        {
            var viewModel = new ErrorViewModel
            {
                HttpStatusCode = HttpStatusCode.NotFound,
                Title = "Lo siento, no encontré lo que buscabas.",
                SubTitle = "Descuida, No eres la única persona a quien esto le ha sucedido.",
            };

            return View("Index", viewModel);
        }

        /// <summary>
        /// HTTP ERROR 500 (INTERNAL SERVER ERROR)
        /// </summary>
        [Route("error/500")]
        [Route("error/exception")]
        public ActionResult Http500()
        {
            var viewModel = new ErrorViewModel
            {
                HttpStatusCode = HttpStatusCode.InternalServerError,
                Title = "Oops! Ha ocurrido un error en nuestro sistema",
                SubTitle = "Estaré revisandolo en breve y empleando mi fuerza para arreglarlo. <br/>Gracias por tu paciencia.",
            };

            return View("Index", viewModel);
        }
    }
}
