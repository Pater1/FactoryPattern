using Factory.Components;
using Factory.Renderer;
using Renderer.Models.HtmlRenderer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Renderer.Models.HtmlRenderer.Components {
    public class HtmlButtonRenderer: HtmlComponentRenderer<Button> {
        public HtmlButtonRenderer(Button RendererDataObject) : base(RendererDataObject) {}

        public override void Render(HtmlRenderOut writer, ComponentRenderer<HtmlRenderOut> parent = null) {
            writer.WriteLine("View", $"<a href=\"{RendererDataObject.FuncLink}\" guid=\"{RendererDataObject.ID.ToString()}\" id=\"{VariableName}\">");
            writer["View"].Indent++;

            writer.WriteLine("View", $"<button type=\"button\">");
            writer["View"].Indent++;
            
            RenderChildren(writer);

            writer["View"].Indent--;
            writer.WriteLine("View", $"</button>");

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
