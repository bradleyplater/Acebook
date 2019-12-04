using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Acebook.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Acebook.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

		public IActionResult Index()
		{
            ViewBag.page = "login";
			return View();
		}

		[HttpPost]
        public IActionResult Index(LoginModel model)
        {
            User user = DBhelper.CheckIfUserExists(model.Email, model.Password);

            if (user.Email != "")
            {
				HttpContext.Session.SetString("User", JsonConvert.SerializeObject(user));
                return RedirectToAction("Index", "Feed");
            } else
            {
                return RedirectToAction("CouldNotLogIn", "Home");
            }
        }

        public IActionResult CouldNotLogIn()
        {
            return View();
        }

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

		
    }
}
