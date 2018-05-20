using Factory.Components;
using Factory.Renderer;
using System.IO;

namespace WPFRenderer.XamlRenderer.Components {
    public class XamlTextBoxRenderer: XamlComponentRenderer<TextBox>{
        public XamlTextBoxRenderer(TextBox RendererDataObject) : base(RendererDataObject) { }

        public override void Render(XamlRenderOut writer, ComponentRenderer<XamlRenderOut> parent = null) {
            writer.WriteLine("Xaml", $"<TextBox Grid.Column=\"{RendererDataObject.X}\" Grid.Row=\"{RendererDataObject.Y}\" Grid.ColumnSpan=\"{RendererDataObject.XSpan+1}\" Grid.RowSpan=\"{RendererDataObject.YSpan+1}\" IsReadOnly = \"{RendererDataObject.IsReadOnly}\">");
            writer["Xaml"].Indent++;
            writer.WriteLine("Xaml", RendererDataObject.Text);
            writer["Xaml"].Indent--;
            writer.WriteLine("Xaml", $"</TextBox>");
        }
    }
}