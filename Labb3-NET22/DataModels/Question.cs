namespace Labb3_NET22.DataModels;

public class Question
{
    public string Statement { get; }
    public string[] Answers { get; }
    public int CorrectAnswer { get; }
    public QuestionCategory Category { get; }
    public string Image { get; }

    public string CorrectAnswerAsString
    {
        get
        {
            return Answers[CorrectAnswer];
        }
    }

    public Question(string statement, string[] answers, int correctAnswer, QuestionCategory category, string image)
    {
        Statement = statement;
        Answers = answers;
        CorrectAnswer = correctAnswer;
        Category = category;
        Image = image;
    }
}