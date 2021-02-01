using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Cys_Controls.Code
{
    public class ControlHelper
    {
        /// <summary>
        /// 查找父元素
        /// </summary>
        /// <typeparam name="T">要查找的父元素</typeparam>
        /// <param name="childVisual">当前元素</param>
        /// <returns></returns>
        public static T FindVisualParent<T>(DependencyObject childVisual)
            where T : DependencyObject
        {
            while (childVisual != null)
            {
                childVisual = VisualTreeHelper.GetParent(childVisual);
                if (childVisual is T visual) return visual;
            }
            return null;
        }


        /// <summary>
        /// 查找子元素
        /// </summary>
        /// <typeparam name="T">要查找的子元素</typeparam>
        /// <param name="parentVisual">当前元素</param>
        /// <returns></returns>
        public static T FindVisualChild<T>(DependencyObject parentVisual)
            where T : DependencyObject
        {
            for (var i = 0; i < VisualTreeHelper.GetChildrenCount(parentVisual); i++)
            {
                var child = VisualTreeHelper.GetChild(parentVisual, i);
                if (child is T dependencyObject)
                {
                    return dependencyObject;
                }

                var childOfChild = FindVisualChild<T>(child);
                if (childOfChild != null) return childOfChild;
            }
            return null;
        }

        /// <summary>
        /// 查找子元素
        /// </summary>
        /// <typeparam name="T">要查找的子元素</typeparam>
        /// <param name="parentVisual">当前元素</param>
        /// <param name="controlName"></param>
        /// <returns></returns>
        public static T FindVisualChild<T>(DependencyObject parentVisual, string controlName)
            where T : DependencyObject
        {
            for (var i = 0; i < VisualTreeHelper.GetChildrenCount(parentVisual); i++)
            {
                var child = VisualTreeHelper.GetChild(parentVisual, i);
                if (child is T childVisual && child is Control control && control.Name == controlName)
                {
                    return childVisual;
                }

                var childOfChild = FindVisualChild<T>(child, controlName);
                if (childOfChild != null) return childOfChild;
            }
            return null;
        }
    }
}
