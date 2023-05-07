using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Labb3_NET22.DataModels;
using Labb3_NET22.Managers;
using Labb3_NET22.Stores;

namespace Labb3_NET22.ViewModels;

public class SelectCategoryViewModel : ObservableObject
{
    private readonly NavigationStore _navigationStore;
    private readonly NavigationStore _localNavigationStore;
    private readonly QuizManager _quizManager;

    public SelectCategoryViewModel(NavigationStore navigationStore, NavigationStore localNavigationStore,
        QuizManager quizManager)
    {
        _navigationStore = navigationStore;
        _localNavigationStore = localNavigationStore;
        _quizManager = quizManager;
        _categoricalQuizList = new ObservableCollection<Quiz>(_quizManager.CreateCategoricalQuizList());
        OnPropertyChanged(nameof(CategoricalQuizList));
        ConfirmCommand = new RelayCommand(SetupConfirmCommand);
        ChangeToQuizCommand = new RelayCommand(() => _localNavigationStore.CurrentViewModel = new SelectQuizViewModel(_navigationStore, _localNavigationStore, _quizManager,
            new QuizPlayerViewModel(_navigationStore, _localNavigationStore, _quizManager)));
    }

    public ICommand ConfirmCommand { get; }
    public ICommand ChangeToQuizCommand { get; }

    private readonly ObservableCollection<Quiz> _categoricalQuizList;

    public ObservableCollection<Quiz> CategoricalQuizList
    {
        get => new ObservableCollection<Quiz>(_categoricalQuizList);
    }
    private Quiz _selectedCategory;
    public Quiz SelectedCategory
    {
        get => _selectedCategory;
        set => SetProperty(ref _selectedCategory, value);
    }
    private void SetupConfirmCommand()
    {
        if (_selectedCategory != null)
        {
            _quizManager.CurrentQuiz = _selectedCategory;

            _localNavigationStore.CurrentViewModel =
                new QuizPlayerViewModel(_navigationStore, _localNavigationStore, _quizManager);
        }
        else
        {
            MessageBox.Show("You must select a quiz to proceed.", "No Quiz Selected", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}