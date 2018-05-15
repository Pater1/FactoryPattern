using Factory.Components.Renderer;
using Factory.Renderer.FileOut;
using System.Threading.Tasks;

namespace Factory.Renderer {
    public sealed class Renderer<R> where R: RenderOut {
        public R ROut { get; internal set; }
        public ComponentRenderer<R> Root { get; internal set; }

        public Renderer(R rOut, ComponentRenderer<R> root) {
            this.ROut = rOut;
            this.Root = root;
        }
        
        public void Render() {
            Root.Render(ROut);
        }
        public Task RenderAsync() {
            return Root.RenderAsync(ROut);
        }
    }
}
