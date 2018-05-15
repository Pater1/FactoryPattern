using Factory.Renderer.FileOut;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Renderer.Models.HtmlRenderer {
    public class HtmlRenderOut: RenderOut {
        public HtmlRenderOut(string folderPath, string fileNameBase) :
            this(folderPath, fileNameBase, "html", "css", "js") { }
        protected HtmlRenderOut(string folderPath, string fileNameBase, params string[] extentions) :
            base(folderPath, ($"{fileNameBase}.{extentions[0]}", "View"), ($"{fileNameBase}.{extentions[1]}", "Style"), ($"{fileNameBase}.{extentions[2]}", "Logic")) { }

        protected override void Setup() {
            WriteLine("View", "<html>");
            this["View"].Indent++;
            WriteLine("View", "<head>");

            this["View"].Indent++;
            this["View"].WriteLine($"<link rel=\"stylesheet\" type=\"text/css\" href=\"{this["Style"].File}\">");
            this["View"].WriteLine($"<script src=\"{this["Logic"].File}\"></script>");
            this["View"].Indent--;

            WriteLine("View", "</head>");
            WriteLine("View", "<body>");
            this["View"].Indent++;
        }
        protected override void Close() {
            this["View"].Indent--; 
            WriteLine("View", "</body>");
            this["View"].Indent--;
            WriteLine("View", "</html>");
        }
    }
}
