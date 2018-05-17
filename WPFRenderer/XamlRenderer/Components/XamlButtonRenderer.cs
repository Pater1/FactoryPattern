using Factory.Components;
using Factory.Renderer;

namespace WPFRenderer.XamlRenderer.Components {
    public class XamlButtonRenderer: XamlComponentRenderer<Button> {
        public XamlButtonRenderer(Button RendererDataObject) : base(RendererDataObject) {}

        public override void Render(XamlRenderOut writer, ComponentRenderer<XamlRenderOut> parent = null) {
            writer.WriteLine("Xaml", $"<Button Grid.Column=\"{_RendererDataObject.X}\" Grid.Row=\"{_RendererDataObject.Y}\" Grid.ColumnSpan=\"{_RendererDataObject.XSpan + 1}\" Grid.RowSpan=\"{_RendererDataObject.YSpan + 1}\">");
            writer["Xaml"].Indent++;
            RenderChildren(writer);
            writer["Xaml"].Indent--;
            writer.WriteLine("Xaml", $"</Button>");
        }
    }
}
