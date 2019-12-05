using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Acebook.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using MongoDB.Bson;

namespace Acebook.Controllers
{
    public class ProfileController : Controller
    {

        public IActionResult Index()
        {
            string json = HttpContext.Session.GetString("User");

            if (json != "No User")
            { 
                
                Object user = JsonConvert.DeserializeObject(json);
                var t = user.ToBsonDocument();
                ViewBag.user = user;
                ViewBag.page = "profile";
                var newUser = ViewBag.user;
                string id = newUser.Id;
                var posts = DBhelper.GetPostById(id);
                ViewBag.posts = posts; 

                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}