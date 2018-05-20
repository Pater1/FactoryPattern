using Factory.Components;
using Factory.Renderer;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renderer.Models.HtmlRenderer.Components {
    public class HtmlGridRenderer: HtmlComponentRenderer<Grid> {
        public HtmlGridRenderer(Grid RendererDataObject) : base(RendererDataObject) { }

        public override void Render(HtmlRenderOut writer, ComponentRenderer<HtmlRenderOut> parent = null) {
            //add some css here!
            writer.WriteLine("View", $"<div id=\"{VariableName}\">");
            writer["View"].Indent++;
            RenderChildren(writer);
            writer["View"].Indent--;
            writer.WriteLine("View", $"</div>");

            writer.WriteLine("Style", $"#{VariableName}" + "{");
            writer["View"].Indent++;

            writer.WriteLine("Style", $"display: grid;");
            writer.WriteLine("Style", $"grid-template-columns: repeat({RendererDataObject.Width}, 1fr);");
            writer.WriteLine("Style", $"grid-template-rows: repeat({RendererDataObject.Height}, 1fr);");
            writer.WriteLine("Style", $"/**/");
            writer.WriteLine("Style", $"grid-column-start: {RendererDataObject.X};");
            writer.WriteLine("Style", $"grid-column-end: {RendererDataObject.X + RendererDataObject.XSpan + 1};");
            writer.WriteLine("Style", $"grid-row-start: {RendererDataObject.Y};");
            writer.WriteLine("Style", $"grid-row-end: {RendererDataObject.Y + RendererDataObject.YSpan + 1};");

            writer["View"].Indent--;
            writer.WriteLine("Style", "}");
        }
    }
}
