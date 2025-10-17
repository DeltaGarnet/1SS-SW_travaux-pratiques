using System.Windows;

namespace DetectLanguageApp.Views
{
    public partial class ConfigurationWindow : Window
    {
        public ConfigurationWindow()
        {
            InitializeComponent();
            if (DataContext is ViewModels.ConfigurationViewModel vm)
            {
                vm.PropertyChanged += (sender, args) =>
                {
                    if (args.PropertyName == nameof(vm.DialogResult) && vm.DialogResult.HasValue)
                    {
                        DialogResult = vm.DialogResult;
                    }
                };
            }
        }
    }
}
