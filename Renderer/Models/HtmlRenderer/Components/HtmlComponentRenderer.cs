using Factory.Components;
using Factory.Renderer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Renderer.Models.HtmlRenderer.Components {
    public abstract class HtmlComponentRenderer<D>: ComponentRenderer<D, HtmlRenderOut> where D : Component {
        public string VariableName => RendererDataObject.GetType().Name + "---" + RendererDataObject.ID.ToString(); //oops
        protected HtmlComponentRenderer(D RendererDataObject) : base(RendererDataObject) {} 
    }
}
