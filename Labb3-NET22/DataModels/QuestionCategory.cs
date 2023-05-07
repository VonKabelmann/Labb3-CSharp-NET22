using System.ComponentModel;

namespace Labb3_NET22.DataModels;

public enum QuestionCategory
{
    [Description("Miscellaneous")]
    Miscellaneous,
    [Description("Movies")]
    Movies,
    [Description("Music")]
    Music,
    [Description("TV")]
    Tv,
    [Description("Pop Culture")]
    PopCulture,
    [Description("Games")]
    Games,
    [Description("Sports")]
    Sports,
    [Description("Religion")]
    Religion,
    [Description("Geography")]
    Geography,
    [Description("History")]
    History,
    [Description("Literature")]
    Literature,
    [Description("Science")]
    Science
}