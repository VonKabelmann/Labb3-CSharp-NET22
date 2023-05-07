using System;
using System.Configuration;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Labb3_NET22.DataModels;
using Labb3_NET22.Managers;
using Labb3_NET22.Stores;
using Labb3_NET22.Views;

namespace Labb3_NET22.ViewModels;

public class CreateQuizViewModel : ObservableObject
{
    private readonly NavigationStore _navigationStore;
    private readonly NavigationStore _localNavigationStore;

    public CreateQuizViewModel(NavigationStore navigationStore)
    {
        _navigationStore = navigationStore;
        _localNavigationStore = new NavigationStore();
        _localNavigationStore.CurrentViewModel = new EditQuizViewModel(_navigationStore, _localNavigationStore, new QuizManager());
        _localNavigationStore.CurrentViewModelChanged += OnCurrentLocalViewModelChanged;

        CancelCommand = new RelayCommand(() =>
        {
            navigationStore.CurrentViewModel = new MainMenuViewModel(_navigationStore);
        });
    }

    #region Properties

    #region Command Properties
    public ICommand CancelCommand { get; }
    #endregion

    public ObservableObject CurrentLocalViewModel => _localNavigationStore.CurrentViewModel;

    #endregion

    #region Methods

    private void OnCurrentLocalViewModelChanged()
    {
        OnPropertyChanged(nameof(CurrentLocalViewModel));
    }

    #endregion


}