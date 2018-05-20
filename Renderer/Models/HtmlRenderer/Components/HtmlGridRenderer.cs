using Factory.Components;
using Factory.Renderer;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renderer.Models.HtmlRenderer.Components {
    public class HtmlGridRenderer: ComponentRenderer<Grid, HtmlRenderOut> {
        public HtmlGridRenderer(Grid RendererDataObject) : base(RendererDataObject) { }

        public override void Render(HtmlRenderOut writer, ComponentRenderer<HtmlRenderOut> parent = null) {
            //add some css here!
            writer.WriteLine("View", $"<div>");
            writer["View"].Indent++;
            RenderChildren(writer);
            writer["View"].Indent--;
            writer.WriteLine("View", $"</div>");
        }
    }
}
