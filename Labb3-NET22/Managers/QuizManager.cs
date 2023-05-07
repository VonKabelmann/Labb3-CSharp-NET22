using Labb3_NET22.DataModels;
using Labb3_NET22.Stores;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Printing;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32;

namespace Labb3_NET22.Managers;

public class QuizManager
{
    public Quiz CurrentQuiz { get; set; }
    public IEnumerable<Quiz?> QuizList { get; set; }

    public Question CurrentQuestion { get; set; }

    public bool CheckIfFileExists(string fileName)
    {
        var pathToFile = Path.Combine(Constants.QuizlyFolderPath, fileName);
        return File.Exists(pathToFile);
    }
    public void CreateQuizlyFolder()
    {
        Directory.CreateDirectory(Constants.QuizlyFolderPath);
    }
    public async Task SaveQuizToFileAsync()
    {
        await Task.Run(CreateQuizlyFolder);
        var json = JsonSerializer.Serialize(CurrentQuiz, new JsonSerializerOptions() { WriteIndented = true });
        var filePath = Path.Combine(Constants.QuizlyFolderPath, $"{CurrentQuiz.Title}.json");
        if (CheckIfFileExists($"{CurrentQuiz.Title}.json"))
        {
            File.Delete(filePath);
        }
        await using var sw = new StreamWriter(filePath);
        await sw.WriteLineAsync(json);
        MessageBox.Show("Quiz successfully saved to file.", "Quiz Saved", MessageBoxButton.OK);
    }
    public Quiz? GetQuizFromFile(string filePath)
    {
        var text = string.Empty;
        string? line = string.Empty;
        using StreamReader sr = new StreamReader(filePath);
        while ((line = sr.ReadLine()) != null)
        {
            text += line;
        }

        return JsonSerializer.Deserialize<Quiz>(text);
    }
    public IEnumerable<Quiz> GetAllQuizzesFromFolder()
    {
        CreateQuizlyFolder();
        var filePaths = Directory.GetFiles(Constants.QuizlyFolderPath);
        return filePaths.Select(filePath => GetQuizFromFile(filePath)).ToList();
    }

    public void DeleteCurrentQuiz()
    {
        var pathToDelete = Path.Combine(Constants.QuizlyFolderPath, $"{CurrentQuiz.Title}.json");
        File.Delete(pathToDelete);
    }

    private async Task<Quiz?> GetQuizFromFileAsync(string filepath)
    {
        var text = string.Empty;
        var line = string.Empty;
        using var sr = new StreamReader(filepath);
        while ((line = await sr.ReadLineAsync()) != null)
        {
            text += line;
        }

        return JsonSerializer.Deserialize<Quiz>(text);
    }
    public async void GetAllQuizzesFromFolderAsync()
    {
        CreateQuizlyFolder();
        var filePaths = Directory.GetFiles(Constants.QuizlyFolderPath);
        var quizList = new List<Quiz?>();
        foreach (var filePath in filePaths)
        {
            Task<Quiz?> getQuiz = GetQuizFromFileAsync(filePath);
            quizList.Add(await getQuiz);
        }
        QuizList = quizList;
    }

    public IEnumerable<Quiz> CreateCategoricalQuizList()
    {
        var fullQuestionList = new List<Question>();
        var categoricalQuizList = new List<Quiz>();
        foreach (var quiz in QuizList)
        {
            fullQuestionList.AddRange(quiz.Questions);
        }

        var distinctList = fullQuestionList.DistinctBy((question) => question.Category);
        foreach (var question in distinctList)
        {
            var questionList = new List<Question>();
            questionList.AddRange(fullQuestionList.Where((q) => q.Category == question.Category));
            categoricalQuizList.Add(new Quiz($"{question.Category}", questionList));
        }

        return categoricalQuizList;
    }

    public void CreateDefaultQuiz()
    {
        if (!CheckIfFileExists("DefaultQuiz.json"))
        {
            var defaultQuiz = new Quiz("DefaultQuiz");
            defaultQuiz.AddQuestion("In 1768, Captain James Cook set out to explore which ocean?", 0, QuestionCategory.History, "https://i.imgur.com/JQadxyu.png",
                new string[]
                {
                    "Pacific Ocean",
                    "Atlantic Ocean",
                    "Indian Ocean",
                    "Arctic Ocean"
                });
            defaultQuiz.AddQuestion("What is actually electricity?", 2, QuestionCategory.Science, "https://i.imgur.com/koZad.jpeg",
                new string[]
                {
                    "A flow of water",
                    "A flow of air",
                    "A flow of electrons",
                    "A flow of atoms"
                });
            defaultQuiz.AddQuestion("Which of the following is a song by the German heavy metal band “Scorpions”?", 1, QuestionCategory.Music, "https://i.imgur.com/WHLqWyw.jpeg",
                new string[]
                {
                    "Stairway to heaven",
                    "Wind of Change",
                    "Don't Stop Me Now",
                    "Hey Jude"
                });
            defaultQuiz.AddQuestion("What was the first country to use tanks in combat during World War I?", 2, QuestionCategory.History, "https://i.imgur.com/k1wQs76.jpeg",
                new string[]
                {
                    "France",
                    "Japan",
                    "Britain",
                    "Germany"
                });
            CurrentQuiz = defaultQuiz;
            SaveQuizToFileAsync();
        }
    }
}