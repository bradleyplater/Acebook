﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Acebook.Controllers
{
    public class FeedController : Controller
    {
        public IActionResult Index()
        {
            return Content("You Logged In!");
        }
    }
}