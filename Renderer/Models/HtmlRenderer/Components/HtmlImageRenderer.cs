using Factory.Components;
using Factory.Renderer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Renderer.Models.HtmlRenderer.Components
{
    public class HtmlImageRenderer : ComponentRenderer<Image, HtmlRenderOut>
    {
        public HtmlImageRenderer(Image RendererDataObject) : base(RendererDataObject) { }

        public override void Render(HtmlRenderOut writer, ComponentRenderer<HtmlRenderOut> parent = null)
        {
            writer.WriteLine("View", $"<a href=\"{_RendererDataObject.LinkPath}\">");
            writer["View"].Indent++;

            writer.WriteLine("View", $"<img src=\"{_RendererDataObject.LinkPath}\">");
            writer["View"].Indent++;
            RenderChildren(writer);
            writer["View"].Indent--;
            writer.WriteLine("View", $"</img>");

            writer["View"].Indent--;
            writer.WriteLine("View", $"</a>");
        }
    }

}
