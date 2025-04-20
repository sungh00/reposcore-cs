using Cocona;

CoconaApp.Run((
    [Argument] string[] repository,
    [Option('v', Description = "자세한 로그 출력을 활성화합니다.")] bool verbose
) =>
{
    Console.WriteLine($"Repository: {String.Join("\n ", repository)}");

    if (verbose) {
        Console.WriteLine("Verbose mode is enabled.");
    }
});

