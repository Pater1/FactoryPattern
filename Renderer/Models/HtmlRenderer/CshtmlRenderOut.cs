using Factory.Renderer.FileOut;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Renderer.Models.HtmlRenderer {
    public class CshtmlRenderOut: HtmlRenderOut {
        public override IEnumerable<(string extention, string alias)> RequiredFileExtentionsWithAlias {
            get {
                return new(string extention, string alias)[] {
                    ($".cshtml", "View"),
                    ($".css", "Style"),
                    ($".js", "Logic")
                };
            }
        }
        public CshtmlRenderOut() { }
        
        public override void Setup() {
            this["View"].WriteLine($"<link rel=\"stylesheet\" type=\"text/css\" href=\"{this["Style"]["File"] as string}\">");
            this["View"].WriteLine($"<script src=\"{this["Logic"]["File"] as string}\"></script>");
            this["View"].WriteLine("");
        }
        public override void Close() {}
    }
}
