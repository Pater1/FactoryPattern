using Factory.Components;
using Factory.Renderer;
using System.IO;

namespace WPFRenderer.XamlRenderer.Components {
    public class XamlButtonRenderer: XamlComponentRenderer<Button> {
        public XamlButtonRenderer(Button RendererDataObject) : base(RendererDataObject) {}

        public override void Render(XamlRenderOut writer, ComponentRenderer<XamlRenderOut> parent = null) {
            writer.WriteLine("Xaml", $"<Button Grid.Column=\"{RendererDataObject.X}\" Grid.Row=\"{RendererDataObject.Y}\" Grid.ColumnSpan=\"{RendererDataObject.XSpan + 1}\" Grid.RowSpan=\"{RendererDataObject.YSpan + 1}\">");
            writer["Xaml"].Indent++;
            RenderChildren(writer);
            writer["Xaml"].Indent--;
            writer.WriteLine("Xaml", $"</Button>");
        }
    }
}
