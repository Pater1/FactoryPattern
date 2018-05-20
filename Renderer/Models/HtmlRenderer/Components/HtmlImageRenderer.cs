using Factory.Components;
using Factory.Renderer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Renderer.Models.HtmlRenderer.Components {
    public class HtmlImageRenderer: HtmlComponentRenderer<Image> {
        public HtmlImageRenderer(Image RendererDataObject) : base(RendererDataObject) { }

        public override void Render(HtmlRenderOut writer, ComponentRenderer<HtmlRenderOut> parent = null) {
            writer.WriteLine("View", $"<a href=\"{RendererDataObject.LinkPath}\" id=\"{VariableName}\">");
            writer["View"].Indent++;

            writer.WriteLine("View", $"<img src=\"{RendererDataObject.LinkPath}\">");
            writer["View"].Indent++;
            RenderChildren(writer);
            writer["View"].Indent--;
            writer.WriteLine("View", $"</img>");

            writer["View"].Indent--;
            writer.WriteLine("View", $"</a>");

            writer.WriteLine("Style", $"#{VariableName}" + "{");
            writer["View"].Indent++;

            writer.WriteLine("Style", $"grid-column-start: {RendererDataObject.X};");
            writer.WriteLine("Style", $"grid-column-end: {RendererDataObject.X + RendererDataObject.XSpan + 1};");
            writer.WriteLine("Style", $"grid-row-start: {RendererDataObject.Y};");
            writer.WriteLine("Style", $"grid-row-end: {RendererDataObject.Y + RendererDataObject.YSpan + 1};");

            writer["View"].Indent--;
            writer.WriteLine("Style", "}");
        }
    }

}
