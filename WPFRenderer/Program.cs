using Factory.Components;
using Factory.Renderer;
using System;
using System.Collections.Generic;
using System.IO;
using WPFRenderer.XamlRenderer;

namespace WPFRenderer
{
    class Program
    {
        static void Main(string[] args)
        {
            RendererFactory<XamlRenderOut>.BuildAndRender(
                new XamlRenderOut($@"E:\Neumont\C#\CSC360\Factory\Wpf_RenderTest\", "MainWindow"),
                new Root() {
                    Child = new Grid() {
                        X = 5,
                        Y = 3,
                        Children = new List<Component>{
                            new Button("Hello There!", "OnButtonClick"){
                                X = 2,
                                Y = 1,
                                XSpan = 1
                            },
                            new Image(@"E:\Neumont\C#\CSC360\Factory\WPFRenderer\resources\captivating-pic-of-rotterdam-in-a-bubble.jpg", true){
                                X = 0,
                                Y = 0,
                                YSpan = 1,
                                XSpan = 1
                            },
                            new Image(@"E:\Neumont\C#\CSC360\Factory\WPFRenderer\resources\captivating-pic-of-rotterdam-in-a-bubble.jpg", false){
                                X = 0,
                                Y = 2,
                                YSpan = 0,
                                XSpan = 1
                            }
                        },
                    }
                }
            );

            //string file = new StreamReader(new FileStream(@"E:\Neumont\C#\CSC360\Factory\WPFRenderer\Output\Window.xaml.cs", FileMode.Open)).ReadToEnd();

            //Console.WriteLine(".xaml.cs:");
            Console.WriteLine("done");

            Console.ReadLine();
        }
    }
}
