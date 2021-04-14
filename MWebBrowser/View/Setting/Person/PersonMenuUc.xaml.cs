using System.Windows.Controls;

namespace MWebBrowser.View.Setting.Person
{
    /// <summary>
    /// Interaction logic for PersonUc.xaml
    /// </summary>
    public partial class PersonMenuUc : UserControl
    {
        public PersonMenuUc()
        {
            InitializeComponent();
        }

        private void PersonButton_OnChecked(object sender, System.Windows.RoutedEventArgs e)
        {
            PersonPop.IsOpen = true;
        }

        private void PersonButton_OnUnchecked(object sender, System.Windows.RoutedEventArgs e)
        {
            PersonPop.IsOpen = false;
        }

        private void ManagePerson_OnClick(object sender, System.Windows.RoutedEventArgs e)
        {

        }
    }
}
