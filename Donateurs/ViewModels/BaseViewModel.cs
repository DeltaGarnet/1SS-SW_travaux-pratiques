using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Donateurs.ViewModels
{
    public enum ACTIONMODE { DISPLAY, ADD, EDIT }

    public class BaseViewModel : INotifyPropertyChanged
    {
        private ACTIONMODE _actionModeActuel = ACTIONMODE.DISPLAY;
        public ACTIONMODE ActionModeActuel
        {
            get => _actionModeActuel;
            set
            {
                if (_actionModeActuel != value)
                {
                    _actionModeActuel = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(IsDisplayMode));
                }
            }
        }

        public bool IsDisplayMode
        {
            get => ActionModeActuel == ACTIONMODE.DISPLAY;
        }

        public bool CanBeginEdit(object param) => IsDisplayMode;
        public bool CanEndEdit(object param) => !IsDisplayMode;

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
