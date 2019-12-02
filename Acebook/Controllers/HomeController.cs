﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Acebook.Models;

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
			Console.WriteLine("Loading View");
            return View();
        }

        [HttpPost]
        public IActionResult Index(LoginModel model)
        {

			Console.WriteLine("I got to the post request");

            User user = DBhelper.CheckIfUserExists(model.Username, model.Password);

            if (user.Email != "")
            {
                return RedirectToAction("Index", "Feed");
            } else
            {
                return RedirectToAction("LoginFail", "Home");
            }
        }

        public string LoginFail()
        {
            return "Could not log you in";
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
