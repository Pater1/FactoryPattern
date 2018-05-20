using Factory.Components;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Factory.Renderer.FileOut;
using System.Threading.Tasks;
using System.IO;

namespace Factory.Renderer {
    public class RendererTemplate<R, T> 
    where R : RenderOut
    where T : TextWriter {
        private RenderOut<T> renderOut;
        private IEnumerable<Type> allowedRenderers = new List<Type>();

        public RendererTemplate(RenderOut renderOut, IEnumerable<Type> allowedRenderers, Func<(string extention, string alias), RenderOut, FileOut<T>> fileOutBuilder) {
            this.renderOut = renderOut.ForWriter<T>(fileOutBuilder);
            this.allowedRenderers = allowedRenderers;
        }

        public RenderOut<R, T> ForComponent<D>(D component)where D: Component{
            RenderOut<R, T>  ret = new RenderOut<R, T>(renderOut, BuildComponentRendererTree(component));
            return ret;
        }

        private ComponentRenderer<D, R> BuildComponentRendererTree<D>(D component) where D : Component {
            return BuildComponentRendererTree(component, component.GetType()).ForComponent<D>();
        }
        private ComponentRenderer<R> BuildComponentRendererTree(Component component, Type componentDataType) {
            IEnumerable<Type> allTypes = AppDomain.CurrentDomain.GetAssemblies()
                                                                    .Select(x => x.GetTypes())
                                                                    .SelectMany(x => x);
            IEnumerable<Type> allRs = allowedRenderers.Select(x => typeof(ComponentRenderer<,>).MakeGenericType(componentDataType, x)).ToArray();
            IEnumerable<Type> renderers = allTypes.Where(x => allRs.Where(check => check.IsAssignableFrom(x)).Any()).ToArray();

            int rendererCount = renderers.Count();
            if(rendererCount < 1) {
                throw new ArgumentException($"There are no known component renderers for a {component} for the renderer type specified.");
            } else if(rendererCount > 1) {
                throw new ArgumentException($"There are multiple known component renderers for a {component} for the renderer type specified.");
            }

            ComponentRenderer<R> rendererInstance = Activator.CreateInstance(renderers.First(), component) as ComponentRenderer<R>;

            foreach(Component c in component) {
                rendererInstance.Add(BuildComponentRendererTree(c, c.GetType()));
            }

            return rendererInstance;
        }

    }
    public class RendererFactory<R> where R : RenderOut, new() {
        private R renderer;
        private ICollection<Type> allowedRenderers = new List<Type>();
        public RendererFactory(){
            renderer = new R();
            allowedRenderers.Add(typeof(R));
        }

        public RendererFactory<R2> Allow<R2>() where R2: RenderOut, new(){
            RendererFactory<R2> ret = new RendererFactory<R2>();
            ret.renderer = this.renderer as R2;

            if(ret.renderer == null){
                throw new InvalidCastException($"TMP");
            }

            allowedRenderers.Add(typeof(R2));
            ret.allowedRenderers = this.allowedRenderers;
            return ret;
        }

        public RendererTemplate<R, T> ForWriter<T>(Func<(string extention, string alias), RenderOut, FileOut<T>> fileOutBuilder) where T: TextWriter{
            return new RendererTemplate<R, T>(renderer, allowedRenderers, fileOutBuilder);
        }

        public RendererTemplate<R, StringWriter> ForStringWriter() => ForWriter(((string extention, string alias) file, RenderOut rout) => new FileOut<StringWriter>(new StringWriter()));
        public RendererTemplate<R, StreamWriter> ForStreamWriter(Stream toWriteTo) => ForWriter(((string extention, string alias) file, RenderOut rout) => new FileOut<StreamWriter>(new StreamWriter(toWriteTo)));
        public RendererTemplate<R, StreamWriter> ForFileStreamWriter(string baseFileFullPath) => ForWriter(((string extention, string alias) file, RenderOut rout) => new FileOut<StreamWriter>(new StreamWriter(new FileStream(baseFileFullPath + file.extention, FileMode.Create))));
    }
}
