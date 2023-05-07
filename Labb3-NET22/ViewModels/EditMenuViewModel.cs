using CommunityToolkit.Mvvm.ComponentModel;
using Labb3_NET22.Managers;
using Labb3_NET22.Stores;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;

namespace Labb3_NET22.ViewModels;

public class EditMenuViewModel : ObservableObject
{
    private readonly NavigationStore _navigationStore;
    private readonly NavigationStore _localNavigationStore;
    private readonly QuizManager _quizManager;

    public EditMenuViewModel(NavigationStore navigationStore, QuizManager quizManager)
    {
        _navigationStore = navigationStore;
        _quizManager = quizManager;
        _localNavigationStore = new NavigationStore();
        _localNavigationStore.CurrentViewModel = new SelectQuizViewModel(_navigationStore, _localNavigationStore, _quizManager,
            new SelectQuestionViewModel(_navigationStore, _localNavigationStore, _quizManager));
        _localNavigationStore.CurrentViewModelChanged += OnCurrentLocalViewModelChanged;
        CancelCommand =
            new RelayCommand(() => _navigationStore.CurrentViewModel = new MainMenuViewModel(_navigationStore));
    }
    public ICommand CancelCommand { get; }
    public ObservableObject CurrentLocalViewModel => _localNavigationStore.CurrentViewModel;
    private void OnCurrentLocalViewModelChanged()
    {
        OnPropertyChanged(nameof(CurrentLocalViewModel));
    }
}