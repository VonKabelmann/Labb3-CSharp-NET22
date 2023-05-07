using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Labb3_NET22.DataModels;
using Labb3_NET22.Managers;
using Labb3_NET22.Stores;

namespace Labb3_NET22.ViewModels;

public class EditQuizViewModel : ObservableObject
{
    #region Readonly Fields
    
    private readonly NavigationStore _navigationStore;
    private readonly NavigationStore _localNavigationStore;
    private readonly QuizManager _quizManager;

    #endregion


    public EditQuizViewModel(NavigationStore navigationStore, NavigationStore localNavigationStore, QuizManager quizManager)
    {
        _navigationStore = navigationStore;
        _localNavigationStore = localNavigationStore;
        _quizManager = quizManager;
        _quizTitle = string.Empty;
        CreateCommand = new RelayCommand(() =>
        {
            if (_quizTitle != string.Empty && !_quizManager.CheckIfFileExists($"{_quizTitle}.json"))
            {
                _quizManager.CurrentQuiz = new Quiz(_quizTitle);
                _localNavigationStore.CurrentViewModel = new EditQuestionViewModel(_navigationStore, _localNavigationStore, _quizManager, false);
            }
            else
            {
                MessageBox.Show("The Title field is empty or a quiz with that name already exists.", "Title error", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        });
    }
    
    #region Properties

    #region Command Properties

    public ICommand CreateCommand { get; }

    #endregion

    private string _quizTitle;
    public string QuizTitle
    {
        get => _quizTitle;
        set => SetProperty(ref _quizTitle, value);
    }

    #endregion

}