using Factory.Components;
using Factory.Renderer;
using Renderer.Models.HtmlRenderer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Renderer.Models.HtmlRenderer.Components {
    public class HtmlButtonRenderer: ComponentRenderer<Button, HtmlRenderOut> {
        public HtmlButtonRenderer(Button RendererDataObject) : base(RendererDataObject) {}

        public override void Render(HtmlRenderOut writer, ComponentRenderer<HtmlRenderOut> parent = null) {
            writer.WriteLine("View", $"<a href=\"{RendererDataObject.FuncLink}\" guid=\"{RendererDataObject.ID.ToString()}\">");
            writer["View"].Indent++;

            writer.WriteLine("View", $"<button type=\"button\">");
            writer["View"].Indent++;
            RenderChildren(writer);
            writer["View"].Indent--;
            writer.WriteLine("View", $"</button>");

            writer["View"].Indent--;
            writer.WriteLine("View", $"</a>");
        }
    }
}
