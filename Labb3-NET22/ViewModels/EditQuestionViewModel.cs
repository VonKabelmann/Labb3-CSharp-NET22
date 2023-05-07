using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Labb3_NET22.DataModels;
using Labb3_NET22.Managers;
using Labb3_NET22.Stores;
using Microsoft.Win32;

namespace Labb3_NET22.ViewModels;

public class EditQuestionViewModel : ObservableObject
{
    private readonly NavigationStore _navigationStore;
    private readonly NavigationStore _localNavigationStore;
    private readonly QuizManager _quizManager;
    private readonly bool _editMode;
    private readonly int _questionToEditIndex; // för att hitta till vilken fråga som ska ersättas i edit mode

    public EditQuestionViewModel(NavigationStore navigationStore, NavigationStore localNavigationStore, QuizManager quizManager, bool editMode)
    {
        _navigationStore = navigationStore;
        _localNavigationStore = localNavigationStore;
        _quizManager = quizManager;
        _editMode = editMode;
        if (_editMode == true)
        {
            _addEditButtonContent = "Edit Question";
            _completeButtonVisibility = Visibility.Collapsed;
            _questionToEditIndex = quizManager.CurrentQuiz.GetIndexOfQuestion(_quizManager.CurrentQuestion);
        }
        else
        {
            _addEditButtonContent = "Add Question";
        }
        if (!Equals(_quizManager.CurrentQuestion, null))
        {
            _questionStatement = _quizManager.CurrentQuestion.Statement;
            _selectedCategory = _quizManager.CurrentQuestion.Category;
            _questionAnswers = _quizManager.CurrentQuestion.Answers;
            _selectedAnswerArray = new bool[4];
            _selectedAnswerArray[_quizManager.CurrentQuestion.CorrectAnswer] = true;
            _imagePath = _quizManager.CurrentQuestion.Image;
        }
        else
        {
            _questionAnswers = new string[4];
            _questionStatement = string.Empty;
            _selectedAnswerArray = new bool[4];
            _selectedAnswerArray[0] = true;
            _imagePath = string.Empty;
        }
        AddEditCommand = new RelayCommand(SetupAddEditCommand);
        CompleteCommand = new RelayCommand(SetupCompleteCommand);
    }

    #region Properties

    #region Command Properties
    public ICommand AddEditCommand { get; }
    public ICommand CompleteCommand { get; }

    #endregion

    private string _addEditButtonContent;

    public string AddEditButtonContent
    {
        get => _addEditButtonContent;
        set => SetProperty(ref _addEditButtonContent, value);
    }
    private string _questionStatement;
    public string QuestionStatement
    {
        get => _questionStatement;
        set => SetProperty(ref _questionStatement, value);
    }

    private QuestionCategory _selectedCategory;
    public QuestionCategory SelectedCategory
    {
        get => _selectedCategory;
        set
        {
            if (_selectedCategory != value)
            {
                SetProperty(ref _selectedCategory, value);
            }
        }
    }

    private string[] _questionAnswers;
    public string[] QuestionAnswers
    {
        get => _questionAnswers;
        set => SetProperty(ref _questionAnswers, value);
    }
    public int CorrectAnswer { get; private set; }
    private bool[] _selectedAnswerArray;
    public bool[] SelectedAnswerArray
    {
        get => _selectedAnswerArray;
        set => SetProperty(ref _selectedAnswerArray, value);
    }

    private string _imagePath;

    public string ImagePath
    {
        get => _imagePath;
        set => SetProperty(ref _imagePath, value);
    }

    private Visibility _completeButtonVisibility;

    public Visibility CompleteButtonVisibility
    {
        get => _completeButtonVisibility;
        set => SetProperty(ref _completeButtonVisibility, value);
    }
    #endregion

    #region Command Content

    private async void SetupAddEditCommand()
    {
        if (QuestionAnswers.Any(string.IsNullOrEmpty) || string.IsNullOrEmpty(QuestionStatement))
        {
            MessageBox.Show("All fields must contain something (Including selecting an image!).", "Empty field found", MessageBoxButton.OK);
        }
        else
        {
            for (int i = 0; i < SelectedAnswerArray.Length; i++)
            {
                if (_selectedAnswerArray[i] == true) CorrectAnswer = i;
            }
            _quizManager.CurrentQuiz.AddQuestion(QuestionStatement, CorrectAnswer, SelectedCategory, ImagePath, QuestionAnswers);
            QuestionAnswers = new string[4];
            QuestionStatement = string.Empty;
            if (_editMode == true)
            {
                _quizManager.CurrentQuiz.RemoveQuestion(_questionToEditIndex);
                await _quizManager.SaveQuizToFileAsync();
                _navigationStore.CurrentViewModel = new EditMenuViewModel(_navigationStore, _quizManager);
            }
        }
    }

    private async void SetupCompleteCommand()
    {
        if (_quizManager.CurrentQuiz.QuestionCount > 0)
        {
            await _quizManager.SaveQuizToFileAsync();
            _navigationStore.CurrentViewModel = new MainMenuViewModel(_navigationStore);
        }
        else
        {
            MessageBox.Show("You cannot create a quiz with no questions!", "No Questions Added", MessageBoxButton.OK,
                MessageBoxImage.Exclamation);
        }
    }

    #endregion
}