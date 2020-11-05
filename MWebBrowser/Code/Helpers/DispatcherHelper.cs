using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Threading;

namespace MWebBrowser.Code.Helpers
{
    public static class DispatcherHelper
    {
        public static Dispatcher UIDispatcher { get; }
        
        static DispatcherHelper()
        {
            UIDispatcher = Application.Current.Dispatcher;
        }
    }
}
