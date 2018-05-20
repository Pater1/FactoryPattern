using Factory.Components;
using Factory.Renderer;

namespace Renderer.Models.HtmlRenderer.Components {
    public class HtmlRootRenderer: ComponentRenderer<Root, HtmlRenderOut> {
        public HtmlRootRenderer(Root RendererDataObject) : base(RendererDataObject) { }

        public override void Render(HtmlRenderOut writer, ComponentRenderer<HtmlRenderOut> parent = null) {
            writer.WriteLine("View", $"<div>");
            writer["View"].Indent++;
            RenderChildren(writer);
            writer["View"].Indent--;
            writer.WriteLine("View", $"</div>");
        }
    }
}
