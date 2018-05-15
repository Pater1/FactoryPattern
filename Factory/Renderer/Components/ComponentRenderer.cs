using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Factory.Components;
using Factory.Renderer.FileOut;

namespace Factory.Renderer {

    public abstract class ComponentRenderer<R>: ICollection<ComponentRenderer<R>> where R : RenderOut {
        protected object data;
        public object _RendererDataObject { 
            get {
                return data;
            }
            internal set {
                data = value;
            }
        }
        protected ComponentRenderer(object RendererDataObject) {
            _RendererDataObject = RendererDataObject;
        }
        internal ComponentRenderer() { }

        internal ICollection<ComponentRenderer<R>> children = new List<ComponentRenderer<R>>();
        
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

        public ComponentRenderer<D,R> UpConvert<D>() where D: Component => this as ComponentRenderer<D, R>;

        #region ICollection<ComponentRenderer>
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
            return children.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return children.GetEnumerator();
        }
        #endregion
    }
    public abstract class ComponentRenderer<D, R>: ComponentRenderer<R> 
        where D: Component 
        where R: RenderOut
    {
        protected ComponentRenderer(D RendererDataObject) {
            _RendererDataObject = RendererDataObject;
        }
        internal ComponentRenderer() { }

        public new D _RendererDataObject {
            get {
                return (D)base._RendererDataObject;
            }
            internal set {
                base._RendererDataObject = value;
            }
        }
    }
}
