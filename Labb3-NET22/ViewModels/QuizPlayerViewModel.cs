using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Labb3_NET22.DataModels;
using Labb3_NET22.Managers;
using Labb3_NET22.Stores;

namespace Labb3_NET22.ViewModels;

public class QuizPlayerViewModel : ObservableObject
{
    private readonly NavigationStore _navigationStore;
    private readonly NavigationStore _localNavigationStore;
    private readonly QuizManager _quizManager;
    
    public QuizPlayerViewModel(NavigationStore navigationStore, NavigationStore localNavigationStore,
        QuizManager quizManager)
    {
        _navigationStore = navigationStore;
        _localNavigationStore = localNavigationStore;
        _quizManager = quizManager;
        _questionsCorrect = 0;
        CorrectIncorrectDisplayColor1 = Constants.CorrectColor1;
        CorrectIncorrectDisplayColor2 = Constants.CorrectColor2;
        if (!Equals(_quizManager.CurrentQuiz, null))    // behövdes för att kunna skicka in QuizPlayerViewModel som nextViewModel parameter till SelectQuizViewModel
        {
            _questionsTotal = _quizManager.CurrentQuiz.QuestionCount;
            _questionsRemaining = _quizManager.CurrentQuiz.QuestionCount;
            CurrentQuestion = GetQuestion();
            RemoveCurrentQuestionFromQuiz();
        }
        SelectAnswerCommand = new RelayCommand<object>(SetupSelectAnswerCommand);
    }

    private string _correctIncorrectString;

    public string CorrectIncorrectString
    {
        get => _correctIncorrectString;
        set => SetProperty(ref _correctIncorrectString, value);
    }

    private Color _correctIncorrectDisplayColor1;

    public Color CorrectIncorrectDisplayColor1
    {
        get => _correctIncorrectDisplayColor1;
        set => SetProperty(ref _correctIncorrectDisplayColor1, value);
    }
    private Color _correctIncorrectDisplayColor2;

    public Color CorrectIncorrectDisplayColor2
    {
        get => _correctIncorrectDisplayColor2;
        set => SetProperty(ref _correctIncorrectDisplayColor2, value);
    }
    private int _questionsTotal;
    public int QuestionsTotal
    {
        get => _questionsTotal;
        set => SetProperty(ref _questionsTotal, value);
    }
    private int _questionsRemaining;
    public int QuestionsRemaining
    {
        get => _questionsRemaining;
        set => SetProperty(ref _questionsRemaining, value);
    }
    private int _questionsCorrect;
    public int QuestionsCorrect
    {
        get => _questionsCorrect;
        set => SetProperty(ref _questionsCorrect, value);
    }


    public ICommand SelectAnswerCommand { get; }
    private Question _currentQuestion;
    public Question CurrentQuestion
    {
        get => _currentQuestion;
        set => SetProperty(ref _currentQuestion, value);
    }
    private Question GetQuestion()
    {
        return _quizManager.CurrentQuiz.GetRandomQuestion();
    }

    private void RemoveCurrentQuestionFromQuiz()
    {
        var indexToRemove = _quizManager.CurrentQuiz.GetIndexOfQuestion(CurrentQuestion);
        _quizManager.CurrentQuiz.RemoveQuestion(indexToRemove);
    }

    private void SetupSelectAnswerCommand(object? state)
    {
        var indexStr = state as string;
        var indexInt = Int32.Parse(indexStr);
        if (indexInt == _currentQuestion.CorrectAnswer)
        {
            QuestionsCorrect++;
            CorrectIncorrectString = "Correct!";
            CorrectIncorrectDisplayColor1 = Constants.CorrectColor1;
            CorrectIncorrectDisplayColor2 = Constants.CorrectColor2;
        }
        else
        {
            CorrectIncorrectString = "Incorrect!";
            CorrectIncorrectDisplayColor1 = Constants.IncorrectColor1;
            CorrectIncorrectDisplayColor2 = Constants.IncorrectColor2;
        }
        QuestionsRemaining--;
        if (_quizManager.CurrentQuiz.QuestionCount < 1)
        {
            MessageBox.Show("That's the quiz! Thanks for playing!", "Thanks For Playing", MessageBoxButton.OK, MessageBoxImage.Information);
            _navigationStore.CurrentViewModel = new MainMenuViewModel(_navigationStore);
        }
        else
        {
            CurrentQuestion = GetQuestion();
            RemoveCurrentQuestionFromQuiz();
        }
    }
}