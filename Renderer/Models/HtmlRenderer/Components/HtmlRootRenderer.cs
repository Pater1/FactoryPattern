using Factory.Components;
using Factory.Renderer;

namespace Renderer.Models.HtmlRenderer.Components {
    public class HtmlRootRenderer: HtmlComponentRenderer<Root> {
        public HtmlRootRenderer(Root RendererDataObject) : base(RendererDataObject) { }

        public override void Render(HtmlRenderOut writer, ComponentRenderer<HtmlRenderOut> parent = null) {
            writer.WriteLine("Style", $"#{VariableName}" + "{");
            writer["View"].Indent++;
            writer.WriteLine("Style", $"    background: {RendererDataObject.BackgroundColor};");
            writer.WriteLine("Style", $"    min-height: 200px;");
            writer["View"].Indent--;
            writer.WriteLine("Style", "}");


            writer.WriteLine("View", $"<div id=\"{VariableName}\" data-guid=\"{RendererDataObject.ID.ToString()}\">");
            writer["View"].Indent++;

            RenderChildren(writer);
            RenderMonkeyPatches(writer);

            writer["View"].Indent--;
            writer.WriteLine("View", $"</div>");
        }
    }
}
