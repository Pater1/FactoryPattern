using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Factory.Renderer.FileOut{
    public sealed class FileOut: IDisposable {
        StreamWriter stream;
        public FileOut(string fileName, string folderPath, StreamWriter sw){
            File = fileName;
            Folder = folderPath;
            stream = sw ?? throw new ArgumentException("Stream writer cannot be null!");
        }

        public string File { get; private set; }
        public string Folder { get; private set; }
        public string FullPath => Path.Combine(Folder, File);

        public uint Indent { get; set; } = 0;
        private string ndnt{
            get{
                string ret = "";
                for(int i = 0; i < Indent; i++){
                    ret += "    ";
                }
                return ret;
            }
        }

        public void Write(string value) {
            stream.Write(value);
        }
        public Task WriteAsync(string value) {
            return stream.WriteAsync(value);
        }
        public void WriteLine(string value) {
            stream.WriteLine(ndnt + value);
        }
        public Task WriteLineAsync(string value) {
            return stream.WriteLineAsync(ndnt + value);
        }

        public void Dispose() {
            stream.Dispose();
        }
    }
}
