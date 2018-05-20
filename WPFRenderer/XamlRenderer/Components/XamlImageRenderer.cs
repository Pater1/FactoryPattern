using Factory.Components;
using Factory.Renderer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace WPFRenderer.XamlRenderer.Components {
    public class XamlImageRenderer: XamlComponentRenderer<Image>  {
        public XamlImageRenderer(Image RendererDataObject) : base(RendererDataObject) { }

        public override void Render(XamlRenderOut writer, ComponentRenderer<XamlRenderOut> parent = null) {
            writer.Write("Xaml", $"<Image");
            writer.Write("Xaml", $" Grid.Column=\"{RendererDataObject.X}\" Grid.Row=\"{RendererDataObject.Y}\"");
            writer.Write("Xaml", $" Grid.ColumnSpan=\"{RendererDataObject.XSpan + 1}\" Grid.RowSpan=\"{RendererDataObject.YSpan + 1}\"");
            writer.Write("Xaml", $" Source=\"{RendererDataObject.LinkPath}\" Stretch=\"{(RendererDataObject.PreserveAspect? "Uniform": "Fill")}\"");
            writer.WriteLine("Xaml", $"/>");
        }
    }
}