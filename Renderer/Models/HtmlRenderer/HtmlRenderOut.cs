using Factory.Renderer.FileOut;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Renderer.Models.HtmlRenderer {
    public class HtmlRenderOut: RenderOut {
        public HtmlRenderOut(string folderPath, string fileNameBase): base(folderPath, ($"{fileNameBase}.html", "View"), ($"{fileNameBase}.css", "Style"), ($"{fileNameBase}.js", "Logic")) {}
        
        protected override void Setup() {
            WriteLine("View", "<head>");
            WriteLine("View", "<link rel=\"stylesheet\" type=\"text/css\" href=\"theme.css\">");
            WriteLine("View", "</head>");
        }
        protected override void Close() { }
    }
}
