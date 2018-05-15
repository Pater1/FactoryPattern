﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Renderer.Models;

namespace Renderer.Controllers {
    public class HomeController: Controller {
        public IActionResult Index() {
            return View();
        }

        public IActionResult About() {
            return View();
        }
    }
}