﻿using Acebook.Models;
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
			user = JsonConvert.DeserializeObject(json);

		    var posts = DBhelper.GetAllPosts();

			ViewBag.posts = posts;

			ViewBag.user = user;
            ViewBag.page = "feed"; 
			return View();
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
            int Like = 0;
            int Dislike = 0;

			DBhelper.CreatePost(Firstname, Surname, Username, Body, Date, Like, Dislike);

			return RedirectToAction("Index", "Feed");
		}

		public IActionResult Like(int count)
		{
			var document = DBhelper.SearchForDocument(count);

			DBhelper.AddLike(document);

			return RedirectToAction("Feed", "Index");
		}
	}
}