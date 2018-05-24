using Factory.Components;
using Factory.Renderer;
using System.IO;

namespace Renderer.Models.HtmlRenderer.Components {
    public class HtmlTextBoxRenderer: HtmlComponentRenderer<TextBox> {
        public HtmlTextBoxRenderer(TextBox RendererDataObject) : base(RendererDataObject) { }

        public override void Render(HtmlRenderOut writer, ComponentRenderer<HtmlRenderOut> parent = null) {
            writer.WriteLine("View", $"<p id=\"{VariableName}\" data-guid=\"{RendererDataObject.ID.ToString()}\">");
            writer["View"].Indent++;
            writer.WriteLine("View", RendererDataObject.Text);
            RenderMonkeyPatches(writer);
            writer["View"].Indent--;
            writer.WriteLine("View", $"</p>");

            writer.WriteLine("Style", $"#{VariableName}" + "{");
            writer["View"].Indent++;

            writer.WriteLine("Style", $"    grid-column-start: {RendererDataObject.X};");
            writer.WriteLine("Style", $"    grid-column-end: {RendererDataObject.X + RendererDataObject.XSpan + 1};");
            writer.WriteLine("Style", $"    grid-row-start: {RendererDataObject.Y};");
            writer.WriteLine("Style", $"    grid-row-end: {RendererDataObject.Y + RendererDataObject.YSpan + 1};");
            writer.WriteLine("Style", $"    background: {RendererDataObject.BackgroundColor};");

            writer["View"].Indent--;
            writer.WriteLine("Style", "}");
        }
    }
}