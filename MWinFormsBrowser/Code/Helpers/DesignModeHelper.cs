using System.Windows.Controls;

namespace MWinFormsBrowser.Code.Helpers
{
    public static class DesignModeHelper
    {
        public static bool IsInDesignMode(this Control control)
        {
            return System.ComponentModel.DesignerProperties.GetIsInDesignMode(control);
        }
    }
}
