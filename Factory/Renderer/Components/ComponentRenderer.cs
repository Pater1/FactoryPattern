using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Factory.Components;
using Factory.Renderer.FileOut;
using Factory.Renderer.Components;

namespace Factory.Renderer {
    public interface IComponentRenderer {

    }
    public abstract class ComponentRenderer<R>: IComponentRenderer, ICollection<ComponentRenderer<R>> where R : RenderOut {
        public virtual void RenderChildren(R writer) {
            foreach(ComponentRenderer<R> child in children) {
                child.Render(writer, this);
            }
        }

        public abstract void Render(R writer, ComponentRenderer<R> parent = null);
        public virtual Task RenderAsync(R writer, ComponentRenderer<R> parent = null) {
            Render(writer);
            return Task.CompletedTask;
        }

        #region ICollection<ComponentRenderer>
        internal ICollection<ComponentRenderer<R>> children = new List<ComponentRenderer<R>>();

        internal ComponentRenderer<D, R> ForComponent<D>() where D : Component => (ComponentRenderer<D, R>)this;

        public int Count => children.Count;

        public bool IsReadOnly => children.IsReadOnly;

        public void Add(ComponentRenderer<R> item) {
            children.Add(item);
        }

        public void Clear() {
            children.Clear();
        }

        public bool Contains(ComponentRenderer<R> item) {
            return children.Contains(item);
        }

        public void CopyTo(ComponentRenderer<R>[] array, int arrayIndex) {
            children.CopyTo(array, arrayIndex);
        }

        public bool Remove(ComponentRenderer<R> item) {
            return children.Remove(item);
        }

        public IEnumerator<ComponentRenderer<R>> GetEnumerator() {
            yield return this;
            foreach(ComponentRenderer<R> child in children){
                foreach(ComponentRenderer<R> grandChild in child) {
                    yield return grandChild;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return this.GetEnumerator();
        }
        
        public abstract bool TryMonkeyPatch(MonkeyPatch<R> patch);
        #endregion
    }
    public abstract class ComponentRenderer<D, R>: ComponentRenderer<R>, IComponentRenderer where R : RenderOut where D : Component {
        protected internal ComponentRenderer(D RendererDataObject) {
            this.RendererDataObject = RendererDataObject;
        }

        public ICollection<MonkeyPatch<R>> patches = new List<MonkeyPatch<R>>();
        public virtual void RenderMonkeyPatches(R renderer){
            foreach(MonkeyPatch<R> patch in patches){
                patch.WritePatch(renderer, this);
            }
        }


        public override bool TryMonkeyPatch(MonkeyPatch<R> patch) {
            Type[] patchType = patch.GetType().GetGenericArguments();
            bool patchable = patchType.Length == 2?
                patchType[0].IsAssignableFrom(typeof(D))
                && patchType[1].IsAssignableFrom(typeof(R))
                ://patchType.Length == 1
                patchType[0].IsAssignableFrom(typeof(R));

            if(patchable) {
                patches.Add(patch);
                return true;
            } else {
                return false;
            }
        }
        public void MonkeyPatch(MonkeyPatch<D, R> patch) {
            patches.Add(patch);
        }

        public D RendererDataObject {
            get;
            internal set;
        }

        protected internal ComponentRenderer() { }
    }
}
