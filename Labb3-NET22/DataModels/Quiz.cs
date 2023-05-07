using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace Labb3_NET22.DataModels;   

public class Quiz
{
    private IEnumerable<Question> _questions;
    private string _title = string.Empty;
    public IEnumerable<Question> Questions => _questions;
    public string Title => _title;
    public int QuestionCount => _questions.Count();

    public Quiz()
    {
        _questions = new List<Question>();
    }

    public Quiz(string title)
    {
        _questions = new List<Question>();
        _title = title;
    }
    [JsonConstructor]
    public Quiz(string title, IEnumerable<Question> questions)
    {
        _questions = questions;
        _title = title;
    }

    public Question GetRandomQuestion()
    {
        var rand = new Random();
        var index = rand.Next(0, _questions.Count());
        return _questions.ElementAt(index);
    }

    public void AddQuestion(string statement, int correctAnswer, QuestionCategory category, string image, params string[] answers)
    {
        List<Question> toList = _questions.ToList();
        toList.Add(new Question(statement, answers, correctAnswer, category, image));
        _questions = toList;
    }

    public void RemoveQuestion(int index)
    {
        var questionToRemove = _questions.ElementAt(index);
        _questions = _questions.Where(q => q != questionToRemove);
    }

    public int GetIndexOfQuestion(Question question)
    {
        return _questions.ToList().IndexOf(question);
    }
}