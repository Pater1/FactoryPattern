using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Factory.Components;
using Factory.Renderer;
using Microsoft.AspNetCore.Mvc;
using Renderer.Models;
using Renderer.Models.HtmlRenderer;

namespace Renderer.Controllers {
    public class HomeController: Controller {
        public IActionResult Index() {
            return View();
        }

        public IActionResult About() {
            RendererFactory<HtmlRenderOut>.BuildAndRender(
                new CshtmlRenderOut($@"E:\Neumont\C#\CSC360\Factory\Renderer\Views\Home", "About"),
                new Button("BUTTON!", "http://google.com")
            );
            
            return View();
        }
    }
}
