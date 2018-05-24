using System;
using System.Collections.Generic;
using ComMod = System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Common_Core.Extentions;
using Factory.Commands;
using Factory.Components;
using Factory.Renderer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Renderer.CustomBindings;
using Renderer.Models;
using Renderer.Models.HtmlRenderer;
using Renderer.Services;
using Factory.Renderer.Components;

namespace Renderer.Controllers {
    [Route("")]
    public class HomeController: Controller {
        //Root Default => new Root() {
        //    Child = new Grid() {
        //        Width = 5,
        //        Height = 3,
        //        Children = new List<Component>{
        //                    new Button("Hello There!", "OnButtonClick"){
        //                        X = 2,
        //                        Y = 1,
        //                        XSpan = 1
        //                    },
        //                    new TextBox("Text goes here!"){
        //                        X = 2,
        //                        Y = 2,
        //                        XSpan = 1
        //                    },
        //                    new Image(@"http://www.theamazingpics.com/includes/img/pics/captivating-pic-of-rotterdam-in-a-bubble.jpg", true){
        //                        X = 0,
        //                        Y = 0,
        //                        YSpan = 1,
        //                        XSpan = 1
        //                    },
        //                    new Image(@"http://www.theamazingpics.com/includes/img/pics/captivating-pic-of-rotterdam-in-a-bubble.jpg", false){
        //                        X = 0,
        //                        Y = 2,
        //                        YSpan = 0,
        //                        XSpan = 1
        //                    }
        //                },
        //    }
        //};

        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IViewRenderService _viewRenderService;

        public HomeController(IHostingEnvironment hostingEnvironment, IViewRenderService viewRenderService) {
            _hostingEnvironment = hostingEnvironment;
            _viewRenderService = viewRenderService;
        }

        public IActionResult Index() {
            return View();
        }

        /// <summary>
        /// Converts a page name to full file path to view generator data file.
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        private string GeneratorPath(string page) {
            return Path.Combine(_hostingEnvironment.ContentRootPath, @"Views\Generated", page) + ".vson";
        }
        private Root PullFromPath(string path) {
            Root root = null;
            if(System.IO.File.Exists(path)) {
                using(var v = new StreamReader(new FileStream(path, FileMode.Open))) {
                    root = v.ReadToEnd().AutoDeserialize<Root>(AutoSerializer.SerializationMethod.RawJson);
                }
            } else {
                root = new Root();
            }
            return root;
        }
        private Root PullPage(string page) {
            return PullFromPath(GeneratorPath(page));
        }

        private void RenderView(Root Model) {
            using(var renderer = new RendererFactory<CshtmlRenderOut>().Allow<HtmlRenderOut>().ForStringWriter().ForComponent(Model)){
                if(ViewData["Patches"] != null) {
                    renderer.MonkeyPatchChildren(ViewData["Patches"] as IEnumerable<MonkeyPatch<HtmlRenderOut>>);
                }

                var rend = renderer.Render();
                ViewData["CompressedSource"] = Model.AutoSerialize(AutoSerializer.SerializationMethod.CompressedJson);
                ViewData["RenderedStyle"] = rend["Style"].ToString();
                ViewData["RenderedView"] = rend["View"].ToString();
                ViewData["RenderedLogic"] = rend["Logic"].ToString();
            }
        }

        [Route("edit/{page}")]
        public IActionResult Edit(string page = "About") {
            ViewData["Title"] = page;
            
            Root root = PullPage(page);
            ViewData["Root"] = root;

            //to view
            ViewData["Json"] = ((Root)ViewData["Root"]).AutoSerialize(AutoSerializer.SerializationMethod.CompressedJson);

            RenderView(root);
            return View("Edit");
        }

        [HttpPost]
        [Route("update/view/{page}/{modify?}")]
        public async Task<IActionResult> UpdateView(string page = "About", string modify = "") {
            string path = GeneratorPath(page);

            //from view
            Root root = Request.Headers["CompJson"][0].AutoDeserialize<Root>(AutoSerializer.SerializationMethod.CompressedJson);

            if(modify == "save") {
                string json = root.AutoSerialize(AutoSerializer.SerializationMethod.RawJson);
                using(var v = new StreamWriter(new FileStream(path, FileMode.Create))) {
                    //to disk
                    v.WriteLine(json);
                    v.Flush();
                }
            }
            if(modify == "undo") {
                root.Down();
            }
            if(modify == "redo") {
                root.Up();
            }

            RenderView(root);
            return PartialView("Generator");
        }
        [HttpPost]
        [Route("update/edit/{page}")]
        public async Task<IActionResult> UpdateEdit(string page = "About") {
            string path = GeneratorPath(page);

            //from view
            Root root = Request.Headers["CompJson"][0].AutoDeserialize<Root>(AutoSerializer.SerializationMethod.CompressedJson);

            IEnumerable<string> activeKeys = Request.Headers["ActiveKeys"][0].Split(',')
                //.Select(x => {
                //    if(Guid.TryParse(x, out Guid ret)){
                //        return (Guid?)ret;
                //    }else{
                //        return null;
                //    }   
                //}).Where(x => x.HasValue).Select(x => x.Value)
                .ToArray();

            Dictionary<Component, IEnumerable<Command>> possibleCommands = activeKeys.Select(x => root[x]).Where(x => x != null).Select(x => new KeyValuePair<Component, IEnumerable<Command>>(x, x.Commands)).ToDictionary(x => x.Key, x => x.Value);//.SelectMany(x => x).ToArray();

            return PartialView("EditorGenerator", possibleCommands);
        }
        [HttpPost]
        [Route("update/command/{page}")]
        public async Task<string> PostCommand(string page) {
            Root root = ((string)Request.Headers["CompJson"]).AutoDeserialize<Root>(AutoSerializer.SerializationMethod.CompressedJson);
            
            string dataRaw = Request.Headers["CommandData"];
            ICollection<Dictionary<string, string>> dataParsed = new List<Dictionary<string, string>>();
            dataParsed = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(dataRaw);
            Dictionary<string, string> data = dataParsed.Select(x => new KeyValuePair<string, string>(x["name"], x["value"])).ToDictionary(x => x.Key, x => x.Value);

            string commType = data["CommandType"];
            Type commandType = AppDomain.CurrentDomain.GetAssemblies()
                                        .Select(x => x.GetTypes())
                                        .SelectMany(x => x)
                                        .Where(x => x.Name == commType).FirstOrDefault();
            Command comm = Activator.CreateInstance(commandType) as Command;
            foreach(KeyValuePair<string, string> v in data) {
                if(v.Key != "CommandType") {
                    PropertyInfo prop = commandType.GetProperty(v.Key);
                    if(prop!= null){
                        object val = v.Value;
                        if(prop.PropertyType != typeof(object) && prop.PropertyType != typeof(string)) {
                            var converter = ComMod.TypeDescriptor.GetConverter(prop.PropertyType);
                            val = converter.ConvertFrom(v.Value);
                        }
                        prop.SetValue(comm,  val);
                    }
                }
            }

            root.Add(comm);
            root.Up();

            return root.AutoSerialize(AutoSerializer.SerializationMethod.CompressedJson);
        }

        [Route("view/{page}")]
        public IActionResult ViewPage(string page = "About") {
            Root root = PullPage(page);
            RenderView(root);

            return View("Generator", root);
        }
    }
}
