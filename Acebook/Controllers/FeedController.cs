using Acebook.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
namespace Acebook.Controllers
{
	public class FeedController : Controller
    {
		Object user;
		
        public IActionResult Index()
        {
			string json = HttpContext.Session.GetString("User");
			
			if (json != "No User")
			{
				user = JsonConvert.DeserializeObject(json);

				var posts = DBhelper.GetAllPosts("Posts");

				ViewBag.posts = posts;

				ViewBag.user = user;
				ViewBag.page = "feed";
				return View();
			} else
			{
				return RedirectToAction("Index", "Home");
			}
        }

		[HttpPost]
		public IActionResult SubmitPost(string message)
		{
			string json = HttpContext.Session.GetString("User");

			Object objectuser = JsonConvert.DeserializeObject(json);
			ViewBag.user = objectuser;
			var user = ViewBag.user;

			string Firstname = user.Firstname;
			string Surname = user.Surname;
			string Username = user.Username;
			string Body = message;
			DateTime Date = DateTime.Now;

			DBhelper.CreatePost(Firstname, Surname, Username, Body, Date, "Posts");

			return RedirectToAction("Index", "Feed");
		}

		public IActionResult Like(int count)
		{
			string json = HttpContext.Session.GetString("User");

			Object objectuser = JsonConvert.DeserializeObject(json);
			ViewBag.user = objectuser;
			var user = ViewBag.user;

			string id = user.Id;

			var document = DBhelper.SearchForDocument(count, "Posts");

			DBhelper.AddLike(document, id, "Posts");

			return RedirectToAction("Index", "Feed");
		}

		public IActionResult Dislike(int count)
		{
			string json = HttpContext.Session.GetString("User");

			Object objectuser = JsonConvert.DeserializeObject(json);
			ViewBag.user = objectuser;
			var user = ViewBag.user;

			string id = user.Id;

			var document = DBhelper.SearchForDocument(count, "Posts");

			DBhelper.AddDislike(document, id, "Posts");

			return RedirectToAction("Index", "Feed");
		}
	}
}