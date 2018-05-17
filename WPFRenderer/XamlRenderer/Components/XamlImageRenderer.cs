using Factory.Components;
using Factory.Renderer;
using System;
using System.Collections.Generic;
using System.Text;

namespace WPFRenderer.XamlRenderer.Components {
    public class XamlImageRenderer: XamlComponentRenderer<Image> {
        public XamlImageRenderer(Image RendererDataObject) : base(RendererDataObject) { }

        public override void Render(XamlRenderOut writer, ComponentRenderer<XamlRenderOut> parent = null) {
            writer.Write("Xaml", $"<Image");
            writer.Write("Xaml", $" Grid.Column=\"{_RendererDataObject.X}\" Grid.Row=\"{_RendererDataObject.Y}\"");
            writer.Write("Xaml", $" Grid.ColumnSpan=\"{_RendererDataObject.XSpan + 1}\" Grid.RowSpan=\"{_RendererDataObject.YSpan + 1}\"");
            writer.Write("Xaml", $" Source=\"{_RendererDataObject.LinkPath}\" Stretch=\"{(_RendererDataObject.PreserveAspect? "Uniform": "Fill")}\"");
            writer.WriteLine("Xaml", $"/>");
        }
    }
}