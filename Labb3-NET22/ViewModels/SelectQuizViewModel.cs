using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Labb3_NET22.DataModels;
using Labb3_NET22.Managers;
using Labb3_NET22.Stores;

namespace Labb3_NET22.ViewModels;

public class SelectQuizViewModel : ObservableObject
{
    private readonly NavigationStore _navigationStore;
    private readonly NavigationStore _localNavigationStore;
    private readonly QuizManager _quizManager;
    private readonly ObservableObject _nextViewModel;

    public SelectQuizViewModel(NavigationStore navigationStore, NavigationStore localNavigationStore,
        QuizManager quizManager, ObservableObject nextViewModel)
    {
        _navigationStore = navigationStore;
        _localNavigationStore = localNavigationStore;
        _quizManager = quizManager;
        _nextViewModel = nextViewModel;
        _quizList = new ObservableCollection<Quiz>(_quizManager.QuizList);
        OnPropertyChanged(nameof(QuizList));
        ConfirmCommand = new RelayCommand(SetupConfirmCommand);
        ChangeToCategoriesCommand = new RelayCommand(() => _localNavigationStore.CurrentViewModel = new SelectCategoryViewModel(_navigationStore, _localNavigationStore, _quizManager));
    }

    public ICommand ConfirmCommand { get; }
    public ICommand ChangeToCategoriesCommand { get; }
    private readonly ObservableCollection<Quiz> _quizList;
    public ObservableCollection<Quiz> QuizList
    {
        get => new ObservableCollection<Quiz>(_quizList);
    }
    private Quiz _selectedQuiz;
    public Quiz SelectedQuiz
    {
        get => _selectedQuiz;
        set => SetProperty(ref _selectedQuiz, value);
    }

    private void SetupConfirmCommand()
    {
        if (_selectedQuiz != null)
        {
            _quizManager.CurrentQuiz = _selectedQuiz;
            switch (_nextViewModel)
            {
                case QuizPlayerViewModel:
                    _localNavigationStore.CurrentViewModel =
                        new QuizPlayerViewModel(_navigationStore, _localNavigationStore, _quizManager);
                    break;
                case SelectQuestionViewModel:
                    _localNavigationStore.CurrentViewModel =
                        new SelectQuestionViewModel(_navigationStore, _localNavigationStore, _quizManager);
                    return;
            }
        }
        else
        {
            MessageBox.Show("You must select a quiz to proceed.", "No quiz selected", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}