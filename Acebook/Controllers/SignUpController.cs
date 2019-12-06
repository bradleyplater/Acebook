using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Acebook.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Acebook.Controllers
{
    public class SignUpController : Controller
    {
        // GET: SignUp
        public ActionResult Index()
        {
            ViewBag.page = "signup";
            return View();
        }

		[HttpPost]
		public IActionResult Index(SignUpModel Model)
		{

			if (formValidation(Model))
			{
				DBhelper.CreateNewUser(Model.Firstname, Model.Surname, Model.Email, Model.Username, Model.Password, "User");
				User user = DBhelper.CheckIfUserExists(Model.Email, Model.Password, "User");

				if (user.Email != "")
				{
					return RedirectToAction("Index", "Home");
				}
				else
				{
					return RedirectToAction("CouldNotSignUp", "SignUp");
				}
			} else
			{
				return RedirectToAction("CouldNotSignUp", "SignUp");
			}

			
		}

		public bool formValidation(SignUpModel Model)
		{
			if (Model.Firstname == "" && Model.Email == "" && Model.Surname == "" && Model.Password == "" && Model.ConfirmPassword == "")
			{
				return false;
			}else if (DBhelper.CheckEmailExists(Model.Email, "User"))
			{
				return false; 
			} else if (DBhelper.CheckUsernameExists(Model.Username, "User"))
			{
				return false;
			} else if (Model.Password != Model.ConfirmPassword)
			{
				return false;
			} else if (Model.Password.Length < 8)
			{
				return false;
			}
			{
				return true;
			}
		}

		public IActionResult CouldNotSignUp()
		{
			return View();
		}
    }
}