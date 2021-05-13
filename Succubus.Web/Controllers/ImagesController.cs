using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Succubus.Web.Controllers
{
    public class ImagesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
