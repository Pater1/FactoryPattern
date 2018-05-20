using Factory.Components;
using Factory.Renderer;
using System;
using System.IO;

namespace WPFRenderer.XamlRenderer.Components {
    public abstract class XamlComponentRenderer<D>: ComponentRenderer<D, XamlRenderOut> where D : Component{
        protected static readonly Random gen = new Random();

        public string VariableName{ get; private set; }
        protected XamlComponentRenderer(D RendererDataObject) : base(RendererDataObject) {
            VariableName = typeof(D).Name + "_" + gen.Next();
        }
    }
}