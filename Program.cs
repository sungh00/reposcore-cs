using Cocona;
using System;
using System.Threading.Tasks;

CoconaApp.Run(async (
    [Argument] string[] repository,
    [Option('v', Description = "자세한 로그 출력을 활성화합니다.")] bool verbose
) =>
{
    Console.WriteLine($"Repository: {String.Join("\n ", repository)}");

    if (verbose)
    {
        Console.WriteLine("Verbose mode is enabled.");
    }

    if (repository.Length != 2)
    {
        Console.WriteLine("repository 인자는 'owner repo' 순서로 2개가 필요합니다.");
        return;
    }

    var analyzer = new GitHubAnalyzer();
    await analyzer.Analyze(repository[0], repository[1]);
});
