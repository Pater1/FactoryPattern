using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Factory.Renderer.FileOut {
    public abstract class FileOut: TextWriter, IDisposable {

        private Dictionary<string, object> backingDB = new Dictionary<string, object>();
        public object this[string key] {
            get {
                if(backingDB.ContainsKey(key)) {
                    return backingDB[key];
                } else {
                    return null;
                }
            }
            set {
                if(backingDB.ContainsKey(key)) {
                    backingDB[key] = value;
                } else {
                    backingDB.Add(key, value);
                }
            }
        }

        public uint Indent { get; set; } = 0;
        protected string TabbedIndent {
            get {
                string ret = "";
                for(int i = 0; i < Indent; i++) {
                    ret += "    ";
                }
                return ret;
            }
        }

    }
    public sealed class FileOut<T>: FileOut where T: TextWriter {
        private T stream;
        public T Stream {
            get {
                return stream;
            }

            internal set {
                stream = value;
            }
        }
        
        public FileOut(T sw) {
            Stream = sw ?? throw new ArgumentException("Stream writer cannot be null!");
        }

        public override Encoding Encoding => Stream.Encoding;
        
        public override void Write(string value) {
            Stream.Write(value);
            Stream.Flush();
        }
        public override Task WriteAsync(string value) {
            return Stream.WriteAsync(value).ContinueWith((x) => Stream.FlushAsync());
        }
        public override void WriteLine(string value) {
            Stream.WriteLine(TabbedIndent + value);
            Stream.Flush();
        }
        public override Task WriteLineAsync(string value) {
            return Stream.WriteLineAsync(TabbedIndent + value).ContinueWith((x) => Stream.FlushAsync());
        }

        protected override void Dispose(bool disposing = true) {
            Stream.Dispose();
        }
    }
}
