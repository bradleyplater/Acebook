﻿using System;
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
            return View();
        }

		[HttpPost]
		public IActionResult Index(SignUpModel Model)
		{

			if (formValidation(Model))
			{
				DBhelper.CreateNewUser(Model.Firstname, Model.Surname, Model.Email, Model.Username, Model.Password);
				User user = DBhelper.CheckIfUserExists(Model.Email, Model.Password);

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
			}else if (DBhelper.CheckEmailExists(Model.Email))
			{
				return false; 
			} else if (DBhelper.CheckUsernameExists(Model.Username))
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


		// GET: SignUp/Details/5
		public ActionResult Details(int id)
        {
            return View();
        }

        // GET: SignUp/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SignUp/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: SignUp/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: SignUp/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: SignUp/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: SignUp/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}