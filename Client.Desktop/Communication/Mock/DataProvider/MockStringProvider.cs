using System;
using System.Text;

namespace Client.Desktop.Communication.Mock.DataProvider;

public static class MockStringProvider
{
    private static readonly Random Rand = new();

    private static readonly string[] SprintPrefixes = { "Sprint", "Iteration", "Release", "Phase" };

    private static readonly string[] SprintGoals =
    {
        "Refactor", "Feature Development", "Bug Fixes", "Performance",
        "UI Improvement", "Backend Optimization", "Integration", "Testing"
    };

    private static readonly string[] SprintModifiers =
    {
        "Alpha", "Beta", "Gamma", "Critical", "Hotfix", "Experimental", "Stable"
    };

    private static readonly string[] TicketPrefixes =
        { "Fix", "Implement", "Refactor", "Update", "Investigate", "Add" };

    private static readonly string[] TicketSubjects =
        { "Login Flow", "UI Layout", "API Endpoint", "Database Migration", "Performance Issue", "Authorization Logic" };

    private static readonly string[] TagsPool =
    {
        "Pair Programming",
        "Not enough information",
        "Needs refactoring",
        "Critical Bug",
        "Performance issue",
        "Test coverage missing",
        "Documentation outdated",
        "Dependency update required",
        "Code review pending"
    };

    private static readonly string[] NoteTypesPool =
    {
        "Technical",
        "Task",
        "Result",
        "Analysis",
        "Decision",
        "Observation",
        "Bug Report",
        "Recommendation"
    };

    private static readonly string[] LoremIpsumWords =
    [
        "lorem", "ipsum", "dolor", "sit", "amet", "consectetuer",
        "adipiscing", "elit", "sed", "diam", "nonummy", "nibh", "euismod",
        "tincidunt", "ut", "laoreet", "dolore", "magna", "aliquam", "erat",
        "pharetra", "vitae", "posuere", "mollis", "nulla", "facilisis"
    ];

    public static string RandomTicketName()
    {
        return $"{TicketPrefixes[Rand.Next(TicketPrefixes.Length)]} {TicketSubjects[Rand.Next(TicketSubjects.Length)]}";
    }

    public static string RandomTag()
    {
        return TagsPool[Rand.Next(TagsPool.Length)];
    }

    public static string RandomNoteType()
    {
        return NoteTypesPool[Rand.Next(NoteTypesPool.Length)];
    }

    public static string LoremIpsum(int minWords = 5, int maxWords = 20, int minSentences = 1, int maxSentences = 5,
        int numParagraphs = 1)
    {
        var result = new StringBuilder();
        for (var p = 0; p < numParagraphs; p++)
        {
            var sentencesInParagraph = Rand.Next(minSentences, maxSentences + 1);
            for (var s = 0; s < sentencesInParagraph; s++)
            {
                var wordsInSentence = Rand.Next(minWords, maxWords + 1);
                var sentence = new StringBuilder();
                for (var w = 0; w < wordsInSentence; w++)
                {
                    var word = LoremIpsumWords[Rand.Next(LoremIpsumWords.Length)];
                    if (w == 0) word = Capitalize(word);
                    sentence.Append(word + (w < wordsInSentence - 1 ? " " : ""));
                }

                sentence.Append(GetRandomSentenceEnding());
                result.Append(sentence + " ");
            }
        }

        return result.ToString();
    }

    public static string RandomSprintName()
    {
        var prefix = SprintPrefixes[Rand.Next(SprintPrefixes.Length)];
        var goal = SprintGoals[Rand.Next(SprintGoals.Length)];
        var modifier = SprintModifiers[Rand.Next(SprintModifiers.Length)];

        if (Rand.NextDouble() < 0.5)
            return $"{prefix} {goal} {modifier}";
        return $"{prefix} {goal}";
    }

    private static string Capitalize(string word)
    {
        return string.IsNullOrEmpty(word) ? word : char.ToUpper(word[0]) + word.Substring(1);
    }

    private static string GetRandomSentenceEnding()
    {
        return new[] { ".", "!", "?" }[Rand.Next(3)];
    }
}