using Cys_Common.Common;
using Cys_Common.Enum;
using System.Windows.Input;

namespace MWebBrowser.ViewModel.Setting
{
    public class SettingViewModel: BaseViewModel
    {
        public SettingViewModel()
        {
            menuItemCommand = new RelayCommand(MenuItemClick);
        }
        private ICommand menuItemCommand;

        public ICommand MenuItemCommand
        {
            get
            {
                return menuItemCommand;
            }
        }
        private void MenuItemClick(object para)
        {
            if(para is SettingEnum settingEnum)
            {
                switch (settingEnum)
                {
                    case SettingEnum.PersonalProfile:
                        break;
                    case SettingEnum.PrivacySearchAndServices:
                        break;
                    case SettingEnum.Appearance:
                        break;
                    case SettingEnum.Startup:
                        break;
                    case SettingEnum.NewTabPage:
                        break;
                    case SettingEnum.CookieAndSitePermissions:
                        break;
                    case SettingEnum.DefaultBrowser:
                        break;
                    case SettingEnum.SearchEngine:
                        break;
                    case SettingEnum.Download:
                        break;
                    case SettingEnum.FamilySafety:
                        break;
                    case SettingEnum.Language:
                        break;
                    case SettingEnum.Printer:
                        break;
                    case SettingEnum.System:
                        break;
                    case SettingEnum.ResetSettings:
                        break;
                    case SettingEnum.PhoneAndOtherSettings:
                        break;
                    case SettingEnum.AboutMEdge:
                        break;
                }
            }
        }
    }
}
