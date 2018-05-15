using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Factory.Renderer.FileOut {
    public abstract class RenderOut: IDisposable {
        protected Dictionary<string, FileOut> writers = new Dictionary<string, FileOut>();
        public FileOut this[string key]{
            get{
                return writers[key];
            }
            set {
                if(writers.ContainsKey(key)) {
                    throw new ArgumentException($"this RenderOut already contains an output named or aliased to \"{key}\"! Please pick an new name/alias.");
                } else {
                    writers.Add(key, value);
                }
            }
        }
        
        protected RenderOut(string folderPath, params string[] filesWithExtention) 
            : this(folderPath, (IEnumerable<string>)filesWithExtention) { }
        protected RenderOut(string folderPath, IEnumerable<string> filesWithExtention) 
            : this(folderPath, filesWithExtention.Select(file => ((string file, string alias))(file, null))) { }
        protected RenderOut(string folderPath, params (string file, string alias)[] filesWithExtentionAndAlias) 
            : this(folderPath, (IEnumerable<(string file, string alias)>)filesWithExtentionAndAlias) { }
        protected RenderOut(string folderPath, IEnumerable<(string file,string alias)> filesWithExtentionAndAlias) {
            if(!Directory.Exists(folderPath)) {
                Directory.CreateDirectory(folderPath);
            }

            if(filesWithExtentionAndAlias != null) {
                foreach((string file,string alias) file in filesWithExtentionAndAlias) {
                    string path = Path.Combine(folderPath, file.file);

                    FileOut strm = new FileOut(file.file, folderPath, new StreamWriter(new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite, 8182, FileOptions.Asynchronous), Encoding.UTF8, 32768) { AutoFlush = true, NewLine = Environment.NewLine });
                    this[file.file] = strm;

                    if(!string.IsNullOrWhiteSpace(file.alias)) {
                        this[file.alias] = strm;
                    }
                }
            }

            Setup();
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

        protected virtual void Setup() { }
        protected virtual void Close() { }

        public virtual void Dispose() {
            Close();
            foreach(KeyValuePair<string, FileOut> kv in writers){
                kv.Value.Dispose();
            }
        }
    }
}
