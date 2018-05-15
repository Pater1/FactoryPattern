using Factory.Components;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Factory.Renderer.FileOut;
using System.Threading.Tasks;

namespace Factory.Renderer {
    public class RendererFactory<R> where R : RenderOut {
        public static void BuildAndRender<C>(R renderOut, C component) where C : Component {
            using(var r = Build(renderOut, component)) {
                r.Render();
            }
        }
        public static Task BuildAndRenderAsync<C>(R renderOut, C component) where C : Component {
            using(var r = Build(renderOut, component)) {
                return r.RenderAsync();
            }
        }


        public static Renderer<R> Build<C>(R renderOut, C component) where C : Component {
            ComponentRenderer<C, R> rend = Build(component);
            return new Renderer<R>(renderOut, rend);
        }

        public static ComponentRenderer<C, R> Build<C>(C component) where C : Component {
            return Build(component, typeof(C)).UpConvert<C>();
        }
        public static ComponentRenderer<R> Build(Component component, Type componentType) {
            IEnumerable<Type> renderers = AppDomain.CurrentDomain.GetAssemblies()
                                                                    .Select(x => x.GetTypes())
                                                                    .SelectMany(x => x)
                                                                    .Where(x => typeof(ComponentRenderer<,>).MakeGenericType(componentType, typeof(R)).IsAssignableFrom(x));
            int rendererCount = renderers.Count();
            if(rendererCount < 1) {
                throw new ArgumentException($"There are no known component renderers for a {component} for the renderer type specified.");
            } else if(rendererCount > 1) {
                throw new ArgumentException($"There are multiple known component renderers for a {component} for the renderer type specified.");
            }

            ComponentRenderer<R> rendererInstance = Activator.CreateInstance(renderers.First(), component) as ComponentRenderer<R>;

            foreach(Component c in component) {
                rendererInstance.Add(Build(c, c.GetType()));
            }

            return rendererInstance;
        }
    }
}
