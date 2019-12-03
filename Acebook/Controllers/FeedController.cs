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
    public class FeedController : Controller
    {
		Object user;
        public IActionResult Index()
        {
			string json = HttpContext.Session.GetString("User");

			user = JsonConvert.DeserializeObject(json);


			ViewBag.user = user;
			return View();
        }

		[HttpPost]
		public string Index(PostModel Model)
		{
			string json = HttpContext.Session.GetString("User");

			Object objectuser = JsonConvert.DeserializeObject(json);
			ViewBag.user = objectuser;
			var user = ViewBag.user;

			string Firstname = user.Firstname;
			string Surname = user.Surname;
			string Username = user.Username;
			string Body = Model.Body;
			DateTime Date = DateTime.Now;

			DBhelper.CreatePost(Firstname, Surname, Username, Body, Date);

			return "String";
		}
	}
}