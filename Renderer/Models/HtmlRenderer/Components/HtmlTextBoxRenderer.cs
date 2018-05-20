using Factory.Components;
using Factory.Renderer;
using System.IO;

namespace Renderer.Models.HtmlRenderer.Components {
    public class HtmlTextBoxRenderer: ComponentRenderer<TextBox, HtmlRenderOut> {
        public HtmlTextBoxRenderer(TextBox RendererDataObject) : base(RendererDataObject) { }

        public override void Render(HtmlRenderOut writer, ComponentRenderer<HtmlRenderOut> parent = null) {
            writer.WriteLine("View", $"<p>");
            writer["View"].Indent++;
            writer.WriteLine("View", RendererDataObject.Text);
            writer["View"].Indent--;
            writer.WriteLine("View", $"</p>");
        }
    }
}