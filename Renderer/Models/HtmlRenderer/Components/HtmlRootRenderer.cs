using Factory.Components;
using Factory.Renderer;

namespace Renderer.Models.HtmlRenderer.Components {
    public class HtmlRootRenderer: HtmlComponentRenderer<Root> {
        public HtmlRootRenderer(Root RendererDataObject) : base(RendererDataObject) { }

        public override void Render(HtmlRenderOut writer, ComponentRenderer<HtmlRenderOut> parent = null) {
            writer.WriteLine("View", $"<div id=\"{VariableName}\">");
            writer["View"].Indent++;
            RenderChildren(writer);
            writer["View"].Indent--;
            writer.WriteLine("View", $"</div>");

            writer.WriteLine("Style", $"#{VariableName}" + "{");
            writer["View"].Indent++;

            //writer.WriteLine("Style", $"grid-column-start: {RendererDataObject.X};");
            //writer.WriteLine("Style", $"grid-column-end: {RendererDataObject.X + RendererDataObject.XSpan + 1};");
            //writer.WriteLine("Style", $"grid-row-start: {RendererDataObject.Y};");
            //writer.WriteLine("Style", $"grid-row-end: {RendererDataObject.Y + RendererDataObject.YSpan + 1};");

            writer["View"].Indent--;
            writer.WriteLine("Style", "}");
        }
    }
}
