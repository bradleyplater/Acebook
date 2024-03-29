﻿using System;
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
      
        public IActionResult SignOut()
        {
            HttpContext.Session.Clear();
			HttpContext.Session.SetString("User", "No User");
            return RedirectToAction("index", "Home");
        }

		public IActionResult Index()
		{
			HttpContext.Session.SetString("User", "No User");
            ViewBag.page = "login";
			return View();
		}

		[HttpPost]
        public IActionResult Index(LoginModel model)
        {
            User user = DBhelper.CheckIfUserExists(model.Email, model.Password, "User");

            if (user.Email != "")
            {
				HttpContext.Session.Clear();
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
