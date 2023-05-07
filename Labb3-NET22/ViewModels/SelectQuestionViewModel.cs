using CommunityToolkit.Mvvm.ComponentModel;
using Labb3_NET22.DataModels;
using Labb3_NET22.Managers;
using Labb3_NET22.Stores;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;

namespace Labb3_NET22.ViewModels;

public class SelectQuestionViewModel : ObservableObject
{
    private readonly NavigationStore _navigationStore;
    private readonly NavigationStore _localNavigationStore;
    private readonly QuizManager _quizManager;

    public SelectQuestionViewModel(NavigationStore navigationStore, NavigationStore localNavigationStore, QuizManager quizManager)
    {
        _navigationStore = navigationStore;
        _localNavigationStore = localNavigationStore;
        _quizManager = quizManager;
        if (!Equals(_quizManager.CurrentQuiz, null))
        {
            _questions = new ObservableCollection<Question>(_quizManager.CurrentQuiz.Questions.ToList());
        }

        RemoveCommand = new RelayCommand(SetupRemoveCommand);
        EditCommand = new RelayCommand(SetupEditCommand);
    }

    public ICommand RemoveCommand { get; }
    public ICommand EditCommand { get; }
    private ObservableCollection<Question> _questions;
    public ObservableCollection<Question> Questions
    {
        get => new ObservableCollection<Question>(_questions);
        set => SetProperty(ref _questions, value);
    }
    private Question _selectedQuestion;
    public Question SelectedQuestion
    {
        get => _selectedQuestion;
        set => SetProperty(ref _selectedQuestion, value);
    }
    private void SetupRemoveCommand()
    {
        if (_quizManager.CurrentQuiz.QuestionCount == 1)
        {
            var userChoice = MessageBox.Show("Deleting the last question will will also delete the quiz. Delete quiz?",
                "Delete Quiz?", MessageBoxButton.YesNo, MessageBoxImage.Question);
            switch (userChoice)
            {
                case MessageBoxResult.Yes:
                    _quizManager.DeleteCurrentQuiz();
                    _localNavigationStore.CurrentViewModel = new SelectQuizViewModel(_navigationStore,
                        _localNavigationStore, _quizManager,
                        new SelectQuestionViewModel(_navigationStore, _localNavigationStore, _quizManager));
                    MessageBox.Show("Quiz successfully deleted!", "Quiz Deleted", MessageBoxButton.OK,
                        MessageBoxImage.Information);
                    return;
                case MessageBoxResult.No:
                    return;
            }
        }
        var indexToRemove = _quizManager.CurrentQuiz.GetIndexOfQuestion(_selectedQuestion);
        _quizManager.CurrentQuiz.RemoveQuestion(indexToRemove);
        _quizManager.SaveQuizToFileAsync();
        _questions = new ObservableCollection<Question>(_quizManager.CurrentQuiz.Questions.ToList());
        OnPropertyChanged(nameof(Questions));
    }

    private void SetupEditCommand()
    {
        if (_selectedQuestion != null)
        {
            _quizManager.CurrentQuestion = _selectedQuestion;
            _localNavigationStore.CurrentViewModel =
                new EditQuestionViewModel(_navigationStore, _localNavigationStore, _quizManager, true);
        }
        else
        {
            MessageBox.Show("You must select a question to proceed.", "No Question Selected", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}