using CefSharp;
using CefSharp.WinForms;

namespace MWebBrowserForm
{
    public class InitCefSharp
    {
        public static void InitializeCefSharp()
        {
            //var settings = new CefSettings();

            var settings = new CefSettings()
            {
                //By default CefSharp will use an in-memory cache, you need to specify a Cache Folder to persist data
                CachePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CefSharp\\Cache")
            };
            string bit = Environment.Is64BitProcess ? "x64" : "x86";
            settings.BrowserSubprocessPath = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, $"runtimes\\win-{bit}\\native",
                                                   "CefSharp.BrowserSubprocess.exe");

            settings.CefCommandLineArgs.Add("ppapi-flash-path", AppDomain.CurrentDomain.BaseDirectory + $"RefDLL\\{bit}\\pepflashplayer.dll");
            settings.CefCommandLineArgs.Add("ppapi-flash-version", "99.0.0.999");
            //http://ssfw.njfy.gov.cn/ssfwzx/ext/flash/flash.jsp falsh测试页面
            //禁用GPU
            settings.CefCommandLineArgs.Add("disable-gpu", "1");
            settings.CefCommandLineArgs.Add("no-proxy-server", "1");

            Cef.Initialize(settings, performDependencyCheck: true, browserProcessHandler: null);
        }
    }
}
