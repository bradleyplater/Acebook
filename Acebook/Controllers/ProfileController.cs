using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Acebook.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;


namespace Acebook.Controllers
{
    public class ProfileController : Controller
    {

        public IActionResult Index()
        {
			string json = HttpContext.Session.GetString("User");

			Object user = JsonConvert.DeserializeObject(json);


			ViewBag.user = user;
            return View();
        }

		
    }
}