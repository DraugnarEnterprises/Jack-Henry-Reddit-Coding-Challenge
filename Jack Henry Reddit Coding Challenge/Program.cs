// See https://aka.ms/new-console-template for more information
using Reddit_Polling_Service;

Console.WriteLine("Instantiating JHRCC_API_Service");
PollRedditAPIService apiQuery = new PollRedditAPIService();
Console.WriteLine("JHRCC_API_Service ready");

DateTime? lastPolled = null;

while (true)
{

    SubredditStatistics retrievedStats = apiQuery.subredditStatistics;

    if (lastPolled == null || lastPolled != retrievedStats.datePolled)
    {
        lastPolled = retrievedStats.datePolled;
        string polledFormatted = retrievedStats.datePolled.HasValue ? retrievedStats.datePolled.Value.ToString() : string.Empty;
        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine($"Polling results for {retrievedStats.subredditName}");
        Console.WriteLine("================================================================================");
        Console.WriteLine($"Last polled on {polledFormatted}");
        Console.WriteLine();
        Console.WriteLine("Most upvoted Post");
        Console.WriteLine($"    Title:    {retrievedStats.postTitle}");
        Console.WriteLine($"    Author:   {retrievedStats.postAuthor}");
        Console.WriteLine($"    UpVotes:  {retrievedStats.upVotes}");
        Console.WriteLine();
        Console.WriteLine("Most Prolific Contributor/Author");
        Console.WriteLine($"    Author:   {retrievedStats.subredditAuthor}");
        Console.WriteLine($"    Postings: {retrievedStats.subredditAuthorPostCount}");
        Console.WriteLine( "================================================================================");
        Console.WriteLine();
        ConsoleKeyInfo? cki = null;
        Console.Write("Check the latest results (Y/N): ");
        while (cki == null || !cki.HasValue || (cki.HasValue && cki.Value.Key.ToString().ToUpperInvariant() != "N" && cki.Value.Key.ToString().ToUpperInvariant() != "Y"))
        {
            if (cki != null && cki.HasValue)
                Console.Beep();

            cki = Console.ReadKey(true);
        }

        Console.WriteLine(cki.Value.Key.ToString());

        if (cki.Value.Key.ToString().ToUpperInvariant() == "N")
        {
            break;
        }
    }
}
