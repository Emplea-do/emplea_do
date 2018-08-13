using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace Api.Controllers
{
    public class BaseController
    {
        protected ILogger _logger;
        public BaseController(ILogger logger)
        {
            _logger = logger;
        }
    }
}
