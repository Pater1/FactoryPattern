using Factory.Components;
using Factory.Renderer.FileOut;
using System;
using System.IO;
using System.Collections.Generic;
using WPFRenderer.XamlRenderer;
using Factory.Renderer;
using Newtonsoft.Json;

namespace WPFRenderer
{
    class Program
    {
        public static int StringStream { get; private set; }

        static void Main(string[] args)
        {
            Root toRender = new Root() {
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
            };

            JsonSerializer ser = new JsonSerializer() {
                TypeNameHandling = TypeNameHandling.All, 
                NullValueHandling = NullValueHandling.Include,
            };
            
            using(var v = new StreamWriter(new FileStream(@"E:\Neumont\C#\CSC360\Factory\Renderer\Views\Generated\Test.vson", FileMode.Create))) {
                ser.Serialize(v, toRender);
            }

            new RendererFactory<XamlRenderOut>().ForFileStreamWriter(@"E:\Neumont\C#\CSC360\Factory\Wpf_RenderTest\MainWindow").ForComponent<Root>(toRender).Render();
            //RenderOut<StringWriter> fout = new XamlRenderOut().ForWriter<StringWriter>();

            //RendererFactory<XamlRenderOut<StreamWriter>, StreamWriter>
            //.BuildRenderOut("MainWindow", (string file, string alias, XamlRenderOut<StreamWriter> renderOut) => {
            //    return new FileOut<StreamWriter>(new StreamWriter(new MemoryStream()));
            //});

            //string file = new StreamReader(new FileStream(@"E:\Neumont\C#\CSC360\Factory\WPFRenderer\Output\Window.xaml.cs", FileMode.Open)).ReadToEnd();

            //Console.WriteLine(".xaml.cs:");
            Console.WriteLine("done");

            Console.ReadLine();
        }
    }
}
