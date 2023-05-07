using System;
using System.IO;
using System.Windows.Media;

namespace Labb3_NET22.Stores;

public static class Constants
{
    public static string QuizlyFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Quizly");
    public static Color CorrectColor1 = Colors.DarkGreen;
    public static Color CorrectColor2 = Colors.ForestGreen;

    public static Color IncorrectColor1 = new Color()
    {
        R = 140,
        G = 0,
        B = 53,
        A = 200
    };

    public static Color IncorrectColor2 = new Color()
    {
        R = 171,
        G = 19,
        B = 77,
        A = 200
    };
}