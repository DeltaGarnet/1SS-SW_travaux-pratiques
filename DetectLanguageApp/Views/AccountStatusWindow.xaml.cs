using System.Windows;

namespace DetectLanguageApp.Views
{
    public partial class AccountStatusWindow : Window
    {
        public AccountStatusWindow()
        {
            InitializeComponent();
            if (DataContext is ViewModels.AccountStatusViewModel vm)
            {
                Loaded += (sender, e) => vm.LoadStatusCommand.Execute(null);
            }
        }
    }
}
