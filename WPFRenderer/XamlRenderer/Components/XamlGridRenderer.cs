using Factory.Components;
using Factory.Renderer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace WPFRenderer.XamlRenderer.Components {
    public class XamlGridRenderer: XamlComponentRenderer<Grid> {
        public XamlGridRenderer(Grid RendererDataObject) : base(RendererDataObject) { }

        public override void Render(XamlRenderOut writer, ComponentRenderer<XamlRenderOut> parent = null) {
            //writer.WriteLine("CodeBehind", "Grid " + VariableName + " = new Grid();");
            //writer.WriteLine("CodeBehind", VariableName + ".ShowGridLines = true;");

            //writer.WriteLine("CodeBehind", "for(int x = 0; x < " + RendererDataObject.X + "; x++) {");
            //writer["CodeBehind"].Indent++;
            //    writer.WriteLine("CodeBehind", "ColumnDefinition gridCol = new ColumnDefinition();");
            //    writer.WriteLine("CodeBehind", "for(int y = 0; y < " + RendererDataObject.Y + "; y++) {");
            //    writer["CodeBehind"].Indent++;
            //        writer.WriteLine("CodeBehind", "RowDefinition gridRow = new RowDefinition();");
            //        //writer.WriteLine("CodeBehind", "gridRow.Height = new GridLength(45);");
            //        writer.WriteLine("CodeBehind", VariableName + ".RowDefinitions.Add(gridRow);");
            //        writer["CodeBehind"].Indent--;
            //    writer.WriteLine("CodeBehind", "}");
            //writer["CodeBehind"].Indent--;
            //writer.WriteLine("CodeBehind", VariableName + ".ColumnDefinitions.Add(gridCol1); ");
            //writer.WriteLine("CodeBehind", "}");
            ////writer.WriteLine("View", $@"<p>");
            ////writer["CodeBehind"].Indent++;

            writer.WriteLine("Xaml", $"<Grid>");
            writer["Xaml"].Indent++;
                writer.WriteLine("Xaml", $"<Grid.ColumnDefinitions>");
                writer["Xaml"].Indent++;
                    for(int i = 0; i < RendererDataObject.X; i++){ 
                    writer.WriteLine("Xaml", $"<ColumnDefinition Width=\"*\"/>");
                    }
                writer["Xaml"].Indent--;
                writer.WriteLine("Xaml", $"</Grid.ColumnDefinitions>");
                writer.WriteLine("Xaml", $"<Grid.RowDefinitions>");
                writer["Xaml"].Indent++;
                    for(int i = 0; i < RendererDataObject.Y; i++){ 
                    writer.WriteLine("Xaml", $"<RowDefinition Height=\"*\"/>");
                    }
                writer["Xaml"].Indent--;
                writer.WriteLine("Xaml", $"</Grid.RowDefinitions>");

                RenderChildren(writer);
            writer["Xaml"].Indent--;
            writer.WriteLine("Xaml", "</Grid>");
        }
    }
}
