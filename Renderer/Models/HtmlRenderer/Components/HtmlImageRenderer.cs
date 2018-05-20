using Factory.Components;
using Factory.Renderer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Renderer.Models.HtmlRenderer.Components {
    public class HtmlImageRenderer: ComponentRenderer<Image, HtmlRenderOut> {
        public HtmlImageRenderer(Image RendererDataObject) : base(RendererDataObject) { }

        public override void Render(HtmlRenderOut writer, ComponentRenderer<HtmlRenderOut> parent = null) {
            writer.WriteLine("View", $"<a href=\"{RendererDataObject.LinkPath}\">");
            writer["View"].Indent++;

            writer.WriteLine("View", $"<img src=\"{RendererDataObject.LinkPath}\">");
            writer["View"].Indent++;
            RenderChildren(writer);
            writer["View"].Indent--;
            writer.WriteLine("View", $"</img>");

            writer["View"].Indent--;
            writer.WriteLine("View", $"</a>");
        }
    }

}
