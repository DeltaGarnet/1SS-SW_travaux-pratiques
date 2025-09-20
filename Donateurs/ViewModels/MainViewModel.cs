using Donateurs.Models;
using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace Donateurs.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private ObservableCollection<Contribution> _contributions;
        private ObservableCollection<Contribution> _filteredContributions;
        private int _contributionCount;
        private bool _showOnlyIllegal;
        private bool _hasContributions;

        public ObservableCollection<Contribution> Contributions
        {
            get => _contributions;
            set
            {
                _contributions = value;
                OnPropertyChanged(nameof(Contributions));
            }
        }

        public ObservableCollection<Contribution> FilteredContributions
        {
            get => _filteredContributions;
            set
            {
                _filteredContributions = value;
                OnPropertyChanged(nameof(FilteredContributions));
            }
        }

        public int ContributionCount
        {
            get => _contributionCount;
            set
            {
                _contributionCount = value;
                OnPropertyChanged(nameof(ContributionCount));
            }
        }

        public bool ShowOnlyIllegal
        {
            get => _showOnlyIllegal;
            set
            {
                _showOnlyIllegal = value;
                OnPropertyChanged(nameof(ShowOnlyIllegal));
                UpdateFilter();
            }
        }

        public bool HasContributions
        {
            get => _hasContributions;
            set
            {
                _hasContributions = value;
                OnPropertyChanged(nameof(HasContributions));
                ((RelayCommand)ClearContributionsCommand).RaiseCanExecuteChanged();
                ((RelayCommand)ToggleFilterCommand).RaiseCanExecuteChanged();
            }
        }

        public ICommand ImportCsvCommand { get; }
        public ICommand ClearContributionsCommand { get; }
        public ICommand ToggleFilterCommand { get; }
        public ICommand OpenConfigurationCommand { get; }

        public MainViewModel()
        {
            Contributions = new ObservableCollection<Contribution>();
            FilteredContributions = new ObservableCollection<Contribution>();
            ImportCsvCommand = new RelayCommand(ImportCsv);
            ClearContributionsCommand = new RelayCommand(ClearContributions, CanClearContributions);
            ToggleFilterCommand = new RelayCommand(ToggleFilter, CanToggleFilter);
            OpenConfigurationCommand = new RelayCommand(OpenConfiguration);
            UpdateContributionCount();
        }

        private void ImportCsv(object parameter)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Fichiers CSV (*.csv)|*.csv|Tous les fichiers (*.*)|*.*",
                InitialDirectory = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "data"))
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    var analyseur = new AnalyseurContributions(openFileDialog.FileName);
                    Contributions = new ObservableCollection<Contribution>(analyseur.Contributions);
                    HasContributions = Contributions.Count > 0;
                    UpdateFilter();
                }
                catch (FormatException ex)
                {
                    MessageBox.Show(Donateurs.Properties.translation.ErrorMessage + "\n" + ex.Message,
                        Donateurs.Properties.translation.ErrorTitle, MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (FileNotFoundException)
                {
                    MessageBox.Show("Le fichier n'a pas été trouvé.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void ClearContributions(object parameter)
        {
            Contributions.Clear();
            FilteredContributions.Clear();
            HasContributions = false;
            UpdateContributionCount();
        }

        private bool CanClearContributions(object parameter)
        {
            return HasContributions;
        }

        private void ToggleFilter(object parameter)
        {
            UpdateFilter();
        }

        private bool CanToggleFilter(object parameter)
        {
            return HasContributions;
        }

        private void UpdateFilter()
        {
            if (ShowOnlyIllegal)
            {
                var illegalContributions = new ObservableCollection<Contribution>();
                foreach (var contribution in Contributions)
                {
                    if (contribution.EstIllegale)
                    {
                        illegalContributions.Add(contribution);
                    }
                }
                FilteredContributions = illegalContributions;
            }
            else
            {
                FilteredContributions = new ObservableCollection<Contribution>(Contributions);
            }
            UpdateContributionCount();
        }

        private void UpdateContributionCount()
        {
            ContributionCount = FilteredContributions.Count;
        }

        private void OpenConfiguration(object parameter)
        {
            var configWindow = new Views.ConfigurationWindow();
            configWindow.ShowDialog();
        }
    }
}
