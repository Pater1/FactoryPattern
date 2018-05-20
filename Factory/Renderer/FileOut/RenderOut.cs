using Factory.Components;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Factory.Renderer.FileOut {
    public interface IRenderOut: IDisposable {
        IEnumerable<(string extention, string alias)> RequiredFileExtentionsWithAlias { get; }
        void Setup();
        void Close();

        bool Write(string file, string value);
        Task<bool> WriteAsync(string file, string value);
        bool WriteLine(string file, string value);
        Task<bool> WriteLineAsync(string file, string value);

        FileOut this[string key]{ get; set; }
    }

    public abstract class RenderOut: IRenderOut{
        internal IRenderOut baseWriter;

        public abstract IEnumerable<(string extention, string alias)> RequiredFileExtentionsWithAlias { get; }

        public FileOut this[string key] {
            get {
                return baseWriter[key];
            }

            set {
                baseWriter[key] = value;
            }
        }

        public virtual void Setup() { }
        public virtual void Close() { }

        public bool Write(string file, string value) => baseWriter.Write(file, value);
        public Task<bool> WriteAsync(string file, string value) => baseWriter.WriteAsync(file, value);
        public bool WriteLine(string file, string value) => baseWriter.WriteLine(file, value);
        public Task<bool> WriteLineAsync(string file, string value) => baseWriter.WriteLineAsync(file, value);

        public virtual void Dispose() {
            baseWriter.Dispose();
        }

        public RenderOut<T> ForWriter<T>(Func<(string extention, string alias), RenderOut, FileOut<T>> fileOutBuilder) where T: TextWriter {
            RenderOut<T> rout = new RenderOut<T>(this, fileOutBuilder);
            baseWriter = rout;
            return rout;
        }
    }
    
    public sealed class RenderOut<T>: IRenderOut where T : TextWriter {
        internal RenderOut implementedWriter;
        public RenderOut(RenderOut implementedWriter, Func<(string extention, string alias), RenderOut, FileOut<T>> fileOutBuilder){
            this.implementedWriter = implementedWriter;
            this.writers = implementedWriter.RequiredFileExtentionsWithAlias
                                                .Select(x => new KeyValuePair<string, FileOut<T>>(x.alias, fileOutBuilder(x, implementedWriter)))
                                                .ToDictionary(x => x.Key, x => x.Value);
        }

        internal IDictionary<string, FileOut<T>> writers = new Dictionary<string, FileOut<T>>();

        public IEnumerable<(string extention, string alias)> RequiredFileExtentionsWithAlias => throw new NotImplementedException();
        
        FileOut IRenderOut.this[string key] {
            get {
                return this[key];
            }
            set {
                FileOut<T> asT = value as FileOut<T>;
                if(asT != null) {
                    this[key] = asT;
                }else{
                    throw new ArgumentException($"Supplied FileOut is not compatable with RenderOut<{typeof(T).Name}>");
                }
            }
        }

        public FileOut<T> this[string key] {
            get {
                return writers[key];
            }
            set {
                if(writers.ContainsKey(key)) {
                    throw new ArgumentException($"this RenderOut already contains an output for \"{key}\"! Please pick an new name/alias.");
                } else {
                    writers.Add(key, value);
                }
            }
        }
        
        public bool Write(string file, string value) {
            if(!writers.ContainsKey(file)) {
                return false;
            }

            this[file].Write(value);
            return true;
        }
        public Task<bool> WriteAsync(string file, string value) {
            if(!writers.ContainsKey(file)) {
                return Task.FromResult(false);
            }

            return this[file].WriteAsync(value).ContinueWith(x => true);
        }
        public bool WriteLine(string file, string value) {
            if(!writers.ContainsKey(file)) {
                return false;
            }

            this[file].WriteLine(value);
            return true;
        }
        public Task<bool> WriteLineAsync(string file, string value) {
            if(!writers.ContainsKey(file)) {
                return Task.FromResult(false);
            }

            return this[file].WriteLineAsync(value).ContinueWith(x => true);
        }
        
        public void Dispose() {
            foreach(KeyValuePair<string, FileOut<T>> kv in writers){
                kv.Value.Dispose();
            }
        }

        public void Setup() {
            implementedWriter.Setup();
        }
        public void Close() {
            implementedWriter.Close();
        }
    }
    
    public sealed class RenderOut<R, T>: IRenderOut where T : TextWriter where R: RenderOut {
        private R implementedRenderOut;
        private RenderOut<T> baseRenderOut;
        public RenderOut(R implementedRenderOut) {
            this.implementedRenderOut = implementedRenderOut;

        }

        public RenderOut(RenderOut<T> renderOut, ComponentRenderer<R> componentRenderer) {
            this.baseRenderOut = renderOut;
            this.implementedRenderOut = renderOut.implementedWriter as R;
            if(this.implementedRenderOut == null){
                throw new ArgumentException("Subrenderer not compatable!");
            }
            this.componentRenderer = componentRenderer;
        }

        internal ComponentRenderer<R> componentRenderer;

        public Dictionary<string, T> Render() {
            componentRenderer.Render(implementedRenderOut);
            return Collapse();
        }
        public Task<Dictionary<string, T>> RenderAsync() {
            return componentRenderer.RenderAsync(implementedRenderOut).ContinueWith(x => { return Collapse(); });
        }
        private Dictionary<string, T> Collapse() => baseRenderOut.writers.Select(x => new KeyValuePair<string, T>(x.Key, x.Value.Stream)).ToDictionary(x => x.Key, x => x.Value);


        public IEnumerable<(string extention, string alias)> RequiredFileExtentionsWithAlias => implementedRenderOut.RequiredFileExtentionsWithAlias;

        public FileOut this[string key] {
            get {
                return implementedRenderOut[key];
            }

            set {
                implementedRenderOut[key] = value;
            }
        }

        public void Dispose() {
            baseRenderOut.Dispose();
        }

        public void Setup() => implementedRenderOut.Setup();
        public void Close() => implementedRenderOut.Close();

        public bool Write(string file, string value) => baseRenderOut.Write(file, value);
        public Task<bool> WriteAsync(string file, string value) => baseRenderOut.WriteAsync(file, value);
        public bool WriteLine(string file, string value) => baseRenderOut.WriteLine(file, value);
        public Task<bool> WriteLineAsync(string file, string value) => baseRenderOut.WriteLineAsync(file, value);
    }
}