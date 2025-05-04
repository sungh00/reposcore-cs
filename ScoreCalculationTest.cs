using System;
using System.Collections.Generic;
using System.IO;
using GitHubActivityAnalyzer;

namespace GitHubActivityAnalyzer
{
    public record ParticipantActivity(
        int PEnhancement,
        int PBug,
        int PTypo,
        int PDocumentation,
        int IEnhancement,
        int IBug,
        int IDocumentation
    );
}

class Program
{
    static void Main()
    {
        var participants = new Dictionary<string, ParticipantActivity>
        {
            ["test_user1"] = new(2, 1, 1, 3, 2, 2, 2),
            ["test_user2"] = new(0, 0, 1, 10, 0, 0, 10),
            ["test_user3"] = new(3, 3, 1, 20, 5, 5, 5),
            ["test_user4"] = new(1, 1, 1, 1, 0, 0, 0),
            ["test_user5"] = new(2, 0, 1, 0, 0, 0, 0),
            ["test_user6"] = new(0, 0, 1, 0, 3, 3, 6),
            ["test_user7"] = new(2, 2, 1, 12, 4, 4, 20),
            ["test_user8"] = new(0, 2, 1, 6, 1, 1, 3),
            ["test_user9"] = new(0, 2, 1, 5, 2, 0, 1),
            ["test_user10"] = new(3, 0, 1, 3, 3, 0, 3),
            ["test_user11"] = new(0, 0, 0, 0, 0, 0, 1)
        };

        foreach (var (name, activity) in participants)
        {
            int total = CalculateTotal(activity);
            Console.WriteLine($"{name}: Total Score = {total}");
        }

        
        File.WriteAllText("participants_summary.csv", GenerateCsv(participants));
    }

    static int CalculateTotal(ParticipantActivity p)
    {
        return
            (p.PEnhancement + p.PBug) * 5 +
            p.PTypo * 1 +
            p.PDocumentation * 2 +
            (p.IEnhancement + p.IBug) * 3 +
            p.IDocumentation * 1;
    }

    static string GenerateCsv(Dictionary<string, ParticipantActivity> data)
    {
        var lines = new List<string> {
            "name,PEnhancement,PBug,PTypo,PDocumentation,IEnhancement,IBug,IDocumentation,total"
        };

        foreach (var (name, activity) in data)
        {
            int total = CalculateTotal(activity);
            lines.Add($"{name},{activity.PEnhancement},{activity.PBug},{activity.PTypo},{activity.PDocumentation}," +
                      $"{activity.IEnhancement},{activity.IBug},{activity.IDocumentation},{total}");
        }

        return string.Join(Environment.NewLine, lines);
    }
}
