namespace MWinFormsCore.Code
{
    public class UserControlHelper
    {
        public static T FindParent<T>(Control control) where T : UserControl
        {
            Control currentControl = control;

            while (currentControl.Parent != null)
            {
                currentControl = currentControl.Parent;

                // 检查父控件是否是指定类型的窗体
                if (currentControl is T parentUserControl)
                {
                    return parentUserControl;
                }
            }
            // 没有找到符合条件的父窗体
            return null;
        }
    }
}
