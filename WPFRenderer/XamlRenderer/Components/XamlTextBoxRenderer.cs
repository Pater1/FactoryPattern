using Factory.Components;
using Factory.Renderer;

namespace WPFRenderer.XamlRenderer.Components {
    public class XamlTextBoxRenderer: XamlComponentRenderer<TextBox> {
        public XamlTextBoxRenderer(TextBox RendererDataObject) : base(RendererDataObject) { }

        public override void Render(XamlRenderOut writer, ComponentRenderer<XamlRenderOut> parent = null) {
            writer.WriteLine("Xaml", $"<TextBox Grid.Column=\"{_RendererDataObject.X}\" Grid.Row=\"{_RendererDataObject.Y}\" Grid.ColumnSpan=\"{_RendererDataObject.XSpan+1}\" Grid.RowSpan=\"{_RendererDataObject.YSpan+1}\" IsReadOnly = \"{_RendererDataObject.IsReadOnly}\">");
            writer["Xaml"].Indent++;
            writer.WriteLine("Xaml", _RendererDataObject.Text);
            writer["Xaml"].Indent--;
            writer.WriteLine("Xaml", $"</TextBox>");
        }
    }
}