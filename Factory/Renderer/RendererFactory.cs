using Factory.Components;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Factory.Renderer.FileOut;
using Factory.Components.Renderer;

namespace Factory.Renderer {
    public class RendererFactory<R> where R : RenderOut {
        public static void BuildAndRender<C>(string folder, C component) where C : Component {
            Build(folder, component).Render();
        }

        public static Renderer<R> Build<C>(string folder, C component) where C : Component {
            R rout = Activator.CreateInstance(typeof(R), folder) as R;
            ComponentRenderer<C, R> rend = Build(component);
            return new Renderer<R>(rout, rend);
        }

        public static ComponentRenderer<C, R> Build<C>(C component) where C: Component  {
            IEnumerable<Type> renderers = AppDomain.CurrentDomain.GetAssemblies()
                                                                    .Select(x => x.GetTypes())
                                                                    .SelectMany(x => x)
                                                                    .Where(t => t.IsSubclassOf(typeof(ComponentRenderer<R>))
                                                                                && t.GetGenericArguments()[0] == typeof(C));
            int rendererCount = renderers.Count();
            if(rendererCount < 1){
                throw new ArgumentException($"There are no known component renderers for a {component} for the renderer type specified.");
            }else if(rendererCount > 1){
                throw new ArgumentException($"There are multiple known component renderers for a {component} for the renderer type specified.");
            }

            ComponentRenderer<C, R> renderer = Activator.CreateInstance(renderers.First(), true) as ComponentRenderer<C, R>;
            renderer._RendererDataObject = component;

            foreach(C c in component){
                renderer.Add(Build<C>(c));
            }

            return renderer;
        }
    }
}
