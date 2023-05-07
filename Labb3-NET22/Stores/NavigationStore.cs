using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Labb3_NET22.Stores;

public class NavigationStore
{
    private ObservableObject _currentViewModel;

    public ObservableObject CurrentViewModel
    {
        get => _currentViewModel;
        set
        {
            _currentViewModel = value; 
            OnCurrentViewModelChanged();
        }
    }

    public event Action CurrentViewModelChanged;

    private void OnCurrentViewModelChanged()
    {
        CurrentViewModelChanged?.Invoke();
    }
}