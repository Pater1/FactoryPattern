using Factory.Components;
using Factory.Renderer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Renderer.Models.HtmlRenderer.Components {
    public abstract class HtmlComponentRenderer<D>: ComponentRenderer<D, HtmlRenderOut> where D : Component {
        protected static readonly Random gen = new Random();

        public string VariableName { get; private set; }
        protected HtmlComponentRenderer(D RendererDataObject) : base(RendererDataObject) {
            VariableName = typeof(D).Name + "_" + gen.Next();
        } 
    }
}
