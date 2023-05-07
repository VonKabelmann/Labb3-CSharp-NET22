using System;
using System.Linq.Expressions;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Labb3_NET22.DataModels;
using Labb3_NET22.Managers;
using Labb3_NET22.Stores;

namespace Labb3_NET22.ViewModels;

public class MainMenuViewModel : ObservableObject
{
    private readonly NavigationStore _navigationStore;
    private readonly QuizManager _quizManager;
    public MainMenuViewModel(NavigationStore navigationStore)
    {
        _navigationStore = navigationStore;
        _quizManager = new QuizManager();
        _quizManager.GetAllQuizzesFromFolderAsync();
        ExitCommand = new RelayCommand(() => Environment.Exit(0));
        CreateCommand = new RelayCommand(() =>
        {
            navigationStore.CurrentViewModel = new CreateQuizViewModel(_navigationStore);
        });
        PlayCommand = new RelayCommand(() =>
        {
            navigationStore.CurrentViewModel = new PlayQuizViewModel(_navigationStore, _quizManager);
        });
        EditCommand = new RelayCommand(() =>
        {
            _navigationStore.CurrentViewModel = new EditMenuViewModel(_navigationStore, _quizManager);
        });
    }
    public ICommand PlayCommand { get; }
    public ICommand CreateCommand { get; }
    public ICommand EditCommand { get; }
    public ICommand ExitCommand { get; }
}