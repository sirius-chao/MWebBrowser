using Cys_Common.Enum;
using System.Windows;

namespace Cys_CustomControls.Code
{
    public static class MenuItemProperties
    {
        public static readonly DependencyProperty SettingEnumValueProperty =
            DependencyProperty.RegisterAttached(
                "SettingEnumValue",
                typeof(SettingEnum),
                typeof(MenuItemProperties),
                new PropertyMetadata(SettingEnum.PersonalProfile));

        public static SettingEnum GetSettingEnumValue(DependencyObject obj)
        {
            return (SettingEnum)obj.GetValue(SettingEnumValueProperty);
        }

        public static void SetSettingEnumValue(DependencyObject obj, SettingEnum value)
        {
            obj.SetValue(SettingEnumValueProperty, value);
        }
    }

}
