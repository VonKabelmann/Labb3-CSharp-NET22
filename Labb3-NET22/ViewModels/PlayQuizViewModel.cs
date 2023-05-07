using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Labb3_NET22.DataModels;
using Labb3_NET22.Managers;
using Labb3_NET22.Stores;

namespace Labb3_NET22.ViewModels;

public class PlayQuizViewModel : ObservableObject
{
    #region Readonly Fields
    
    private readonly NavigationStore _navigationStore;
    private readonly NavigationStore _localNavigationStore;
    private readonly QuizManager _quizManager;

    #endregion


    public PlayQuizViewModel(NavigationStore navigationStore, QuizManager quizManager)
    {
        _navigationStore = navigationStore;
        _quizManager = quizManager;
        _localNavigationStore = new NavigationStore();
        _localNavigationStore.CurrentViewModel = new SelectQuizViewModel(_navigationStore, _localNavigationStore, _quizManager,
            new QuizPlayerViewModel(_navigationStore, _localNavigationStore, _quizManager));
        _localNavigationStore.CurrentViewModelChanged += OnCurrentLocalViewModelChanged;
        CancelCommand = new RelayCommand(() =>
        {
            navigationStore.CurrentViewModel = new MainMenuViewModel(_navigationStore);
        });
    }

    public ICommand CancelCommand { get; }
    public ObservableObject CurrentLocalViewModel => _localNavigationStore.CurrentViewModel;
    private void OnCurrentLocalViewModelChanged()
    {
        OnPropertyChanged(nameof(CurrentLocalViewModel));
    }
}