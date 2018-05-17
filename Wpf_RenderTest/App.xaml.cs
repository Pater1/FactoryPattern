using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Wpf_RenderTest {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App: Application {
        public App(): base(){
            ////ProcessStartInfo proc = new ProcessStartInfo();
            //////proc.FileName = @"C:\windows\system32\cmd.exe";
            ////proc.FileName = @"dotnet \..\..\..\WPFRenderer\bin\Debug\netcoreapp2.0\Factory.dll";
            ////proc.UseShellExecute = false;
            ////proc.RedirectStandardInput = true;
            ////proc.RedirectStandardOutput = true;
            ////proc.RedirectStandardError = true;

            //ProcessStartInfo proc = new ProcessStartInfo {
            //    FileName = @"dotnet",
            //    Arguments = @"..\..\..\WPFRenderer\bin\Debug\netcoreapp2.0\Factory.dll",
            //    UseShellExecute = false,
            //    RedirectStandardInput = true,
            //    RedirectStandardOutput = true,
            //    RedirectStandardError = true
            //};

            //Process p = new Process() {
            //    StartInfo = proc
            //};
            //p.Start();
            ////p.StandardInput.WriteLine(@"dotnet \..\..\..\WPFRenderer\bin\Debug\netcoreapp2.0\Factory.dll");

            //string line = "", lastLine = "", xaml = "", xamlcs = "";
            //do {
            //    line = p.StandardOutput.ReadLine();
            //    if(lastLine == ".xaml.cs:"){
            //        xamlcs = line;
            //    }else if(lastLine == ".xaml:") {
            //        xaml = line;
            //    }
            //    lastLine = line;
            //} while(!p.StandardOutput.EndOfStream);
            //line = line;
        }
    }
}
