using Factory.Renderer.FileOut;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WPFRenderer.XamlRenderer {
    public class XamlRenderOut: RenderOut {
        public override IEnumerable<(string extention, string alias)> RequiredFileExtentionsWithAlias {
            get {
                return new(string file, string alias)[] {
                    (".xaml", "Xaml"),
                    (".xaml.cs", "CodeBehind")
                };
            }
        }
        
        public override void Setup() {}
        public override void Close() {}
    }
}
