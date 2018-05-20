using Factory.Renderer.FileOut;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Renderer.Models.HtmlRenderer {
    public class HtmlRenderOut: RenderOut {
        public override IEnumerable<(string extention, string alias)> RequiredFileExtentionsWithAlias {
            get {
                return new(string extention, string alias)[] {
                    ($".html", "View"),
                    ($".css", "Style"),
                    ($".js", "Logic")
                };
            }
        }
        public HtmlRenderOut() { }
        
        public override void Setup() {
            WriteLine("View", "<html>");
            this["View"].Indent++;
            WriteLine("View", "<head>");

            this["View"].Indent++;
            this["View"].WriteLine($"<link rel=\"stylesheet\" type=\"text/css\" href=\"{this["Style"]["File"] as string}\">");
            this["View"].WriteLine($"<script src=\"{this["Logic"]["File"] as string}\"></script>");
            this["View"].Indent--;

            WriteLine("View", "</head>");
            WriteLine("View", "<body>");
            this["View"].Indent++;
        }
        public override void Close() {
            this["View"].Indent--; 
            WriteLine("View", "</body>");
            this["View"].Indent--;
            WriteLine("View", "</html>");
        }
    }
}
