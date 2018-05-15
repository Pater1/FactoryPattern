using Factory.Renderer.FileOut;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Renderer.Models.HtmlRenderer {
    public class CshtmlRenderOut: HtmlRenderOut {
        public CshtmlRenderOut(string folderPath, string fileNameBase) :
            base(folderPath, fileNameBase, "cshtml", "css", "js") { }
        
        protected override void Setup() {
            this["View"].WriteLine($"<link rel=\"stylesheet\" type=\"text/css\" href=\"{this["Style"].File}\">");
            this["View"].WriteLine($"<script src=\"{this["Logic"].File}\"></script>");
            this["View"].WriteLine("");
        }
        protected override void Close() {}
    }
}
