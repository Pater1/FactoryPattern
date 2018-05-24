using Factory.Components;
using Factory.Renderer.FileOut;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Factory.Renderer.Components {
    public class MonkeyPatch<R> where R : RenderOut {
        [JsonProperty]
        public ICollection<Patch> patches = new List<Patch>();

        public virtual void WritePatch(R renderer, ComponentRenderer<R> componentRenderer) {
            Component c = componentRenderer.GetType().GetProperty("RendererDataObject").GetValue(componentRenderer) as Component;
            if(c != null) {
                foreach(Patch patch in patches) {
                    patch.Apply(renderer, c);
                }
            }
        }
    }
    public class MonkeyPatch<T, R>: MonkeyPatch<R> where T : Component where R : RenderOut {
        [JsonProperty]
        public ICollection<Patch<T>> typedPatches = new List<Patch<T>>();

        public override void WritePatch(R renderer, ComponentRenderer<R> componentRenderer) {
            Component c = componentRenderer.GetType().GetProperty("RendererDataObject").GetValue(componentRenderer) as Component;
            if(c != null) {
                T t = c as T;
                if(t != null) {
                    foreach(Patch<T> patch in typedPatches) {
                        patch.Apply(renderer, t);
                    }
                    foreach(Patch patch in base.patches) {
                        patch.Apply(renderer, t);
                    }
                }
            }
        }
    }
    public abstract class Patch {
        internal protected abstract void Apply(RenderOut renderer, Component t);
    }
    public abstract class Patch<T>: Patch where T : Component {
        protected internal override void Apply(RenderOut renderer, Component t) {
            T nT = t as T;
            if(nT != null) {
                Apply(renderer, nT);
            }
        }
        internal protected abstract void Apply(RenderOut renderer, T t);
    }
    public class WriteLine: Patch{
        [JsonProperty]
        private string file;
        [JsonProperty]
        private string line;

        public WriteLine(string file, string line) {
            this.file = file;
            this.line = line;
        }

        protected internal override void Apply(RenderOut renderer, Component t) {
            renderer.WriteLine(file, line);
        }
    }
    public class Write: Patch {
        [JsonProperty]
        private string file;
        [JsonProperty]
        private string text;

        public Write(string file, string line) {
            this.file = file;
            this.text = line;
        }

        protected internal override void Apply(RenderOut renderer, Component c) {
            renderer.Write(file, text);
        }
    }
    public class WriteProperty<T>: Patch<T> where T : Component {
        [JsonProperty]
        private string file;
        [JsonProperty]
        private string propertyName;

        public WriteProperty(string file, string line) {
            this.file = file;
            this.propertyName = line;
        }

        protected internal override void Apply(RenderOut renderer, T t) {
            renderer.Write(file, t.GetType().GetProperty(propertyName).GetValue(t).ToString());
        }
    }
    public class WriteField<T>: Patch<T> where T : Component {
        [JsonProperty]
        private string file;
        [JsonProperty]
        private string fieldName;

        public WriteField(string file, string line) {
            this.file = file;
            this.fieldName = line;
        }

        protected internal override void Apply(RenderOut renderer, T t) {
            renderer.Write(file, t.GetType().GetField(fieldName).GetValue(t).ToString());
        }
    }
    public class Indent: Patch {
        [JsonProperty]
        private string file;
        [JsonProperty]
        private bool up;

        public Indent(string file, bool up) {
            this.file = file;
            this.up = up;
        }

        protected internal override void Apply(RenderOut renderer, Component c) {
            if(up) {
                renderer[file].Indent++;
            }else{
                renderer[file].Indent--;
            }
        }
    }
}
