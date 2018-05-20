using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Factory.Components;
using Factory.Renderer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Renderer.Models;
using Renderer.Models.HtmlRenderer;

namespace Renderer.Controllers {
    [Route("")]
    public class HomeController: Controller {
        Root Default => new Root() {
            Child = new Grid() {
                X = 5,
                Y = 3,
                Children = new List<Component>{
                            new Button("Hello There!", "OnButtonClick"){
                                X = 2,
                                Y = 1,
                                XSpan = 1
                            },
                            new Image(@"http://www.theamazingpics.com/includes/img/pics/captivating-pic-of-rotterdam-in-a-bubble.jpg", true){
                                X = 0,
                                Y = 0,
                                YSpan = 1,
                                XSpan = 1
                            },
                            new Image(@"http://www.theamazingpics.com/includes/img/pics/captivating-pic-of-rotterdam-in-a-bubble.jpg", false){
                                X = 0,
                                Y = 2,
                                YSpan = 0,
                                XSpan = 1
                            }
                        },
            }
        };

        private readonly IHostingEnvironment _hostingEnvironment;
        public HomeController(IHostingEnvironment hostingEnvironment) {
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index() {
            return View();
        }

        /// <summary>
        /// Converts a page name to full file path to view generator data file.
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        private string GeneratorPath(string page){
            return Path.Combine(_hostingEnvironment.ContentRootPath, @"Views\Generated", page) + ".vson";
        }

        [Route("edit/{page}")]
        public IActionResult Edit(string page = "About") {
            ViewData["Title"] = page;

            string path = GeneratorPath(page);
            JsonSerializer ser = new JsonSerializer() {
                TypeNameHandling = TypeNameHandling.All,
                NullValueHandling = NullValueHandling.Include,
            };
            ViewData["Serializer"] = ser;

            Root root = null;
            if(System.IO.File.Exists(path)){
                using(var v = new JsonTextReader(new StreamReader(new FileStream(path, FileMode.Create)))) {
                    root = ser.Deserialize<Root>(v);
                }
            }else{
                root = new Root();
            }
            ViewData["Root"] = Default;

            StringWriter json = new StringWriter();
            ser.Serialize(json, root);
            ViewData["Json"] = json;

            return View("Edit");
        }
        [HttpPost]
        [Route("update/{page}")]
        public void Update(string page = "About") {
            string path = GeneratorPath(page);
            JsonSerializer ser = new JsonSerializer() {
                TypeNameHandling = TypeNameHandling.All,
                NullValueHandling = NullValueHandling.Include,
            };

            Component root = null;
            using(JsonReader reader = new JsonTextReader(new StreamReader(Request.Body))) {
                root = ser.Deserialize<Root>(reader);
            }

            using(StreamWriter v = new StreamWriter(new FileStream(path, FileMode.Create))) {
                ser.Serialize(v, root);
            };
        }

        [Route("view/{page}")]
        public IActionResult About(string page = "About") {
            //RendererFactory<HtmlRenderOut>.BuildAndRender(
            //    new CshtmlRenderOut($@"E:\Neumont\C#\CSC360\Factory\Renderer\Views\Home", page),
            //    new Button("BUTTON!", "http://google.com")
            //);
            
            return View();
        }
    }
}
