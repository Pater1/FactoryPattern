using Factory.Components;
using Factory.Renderer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace WPFRenderer.XamlRenderer.Components {
    public class XamlWindowRenderer: XamlComponentRenderer<Root> {
        public XamlWindowRenderer(Root RendererDataObject) : base(RendererDataObject) {}

        public override void Render(XamlRenderOut writer, ComponentRenderer<XamlRenderOut> parent = null) {
            writer.WriteLine("CodeBehind", "using System;");
            writer.WriteLine("CodeBehind", "using System.Collections.Generic;");
            writer.WriteLine("CodeBehind", "using System.Linq;");
            writer.WriteLine("CodeBehind", "using System.Text;");
            writer.WriteLine("CodeBehind", "using System.Threading.Tasks;");
            writer.WriteLine("CodeBehind", "using System.Windows;");
            writer.WriteLine("CodeBehind", "using System.Windows.Controls;");
            writer.WriteLine("CodeBehind", "using System.Windows.Data;");
            writer.WriteLine("CodeBehind", "using System.Windows.Documents;");
            writer.WriteLine("CodeBehind", "using System.Windows.Input;");
            writer.WriteLine("CodeBehind", "using System.Windows.Media;");
            writer.WriteLine("CodeBehind", "using System.Windows.Media.Imaging;");
            writer.WriteLine("CodeBehind", "using System.Windows.Navigation;");
            writer.WriteLine("CodeBehind", "using System.Windows.Shapes;");
            writer.WriteLine("CodeBehind", "");
            writer.WriteLine("CodeBehind", "namespace Wpf_RenderTest {");
            writer["CodeBehind"].Indent++;
                writer.WriteLine("CodeBehind", "public partial class " + "MainWindow" + ": Window{");
                writer["CodeBehind"].Indent++;
                    writer.WriteLine("CodeBehind", "public " + "MainWindow" + "(){");
                    writer["CodeBehind"].Indent++;
                        writer.WriteLine("CodeBehind", "InitializeComponent();");
                    writer["CodeBehind"].Indent--;
                    writer.WriteLine("CodeBehind", "}");
                writer["CodeBehind"].Indent--;

            writer.WriteLine("Xaml", "<Window x:Class=\"Wpf_RenderTest.MainWindow\"");
            writer.WriteLine("Xaml", "    xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\"");
            writer.WriteLine("Xaml", "    xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\"");
            writer.WriteLine("Xaml", "    xmlns:d=\"http://schemas.microsoft.com/expression/blend/2008\"");
            writer.WriteLine("Xaml", "    xmlns:mc=\"http://schemas.openxmlformats.org/markup-compatibility/2006\"");
            writer.WriteLine("Xaml", "    xmlns:local=\"clr -namespace:Wpf_RenderTest\"");
            writer.WriteLine("Xaml", "    mc:Ignorable=\"d\"");
            writer.WriteLine("Xaml", "   Title=\"MainWindow\" Height=\"450\" Width=\"800\">");
            writer["Xaml"].Indent++;
                RenderChildren(writer);
            writer["Xaml"].Indent--;
            writer.WriteLine("Xaml", "</Window>");


                writer.WriteLine("CodeBehind", "}");
            writer["CodeBehind"].Indent--;
            writer.WriteLine("CodeBehind", "}");
        }
    }
}
