using Factory.Renderer.FileOut;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WPFRenderer.XamlRenderer {
    public class XamlRenderOut: RenderOut {
        public XamlRenderOut(string folderPath, string fileNameBase) :
            this(folderPath, fileNameBase, "xaml", "xaml.cs") { }
        protected XamlRenderOut(string folderPath, string fileNameBase, params string[] extentions) :
            base(folderPath, ($"{fileNameBase}.{extentions[0]}", "Xaml"), ($"{fileNameBase}.{extentions[1]}", "CodeBehind")) { }

        protected override void Setup() {}
        protected override void Close() {}
    }
}
