using Cocona;

CoconaApp.Run(([Argument] string[] repository, bool verbose) =>
        {
            Console.WriteLine($"Repository: {String.Join("\n ", repository)}");
            if (verbose)
            {
                Console.WriteLine("Verbose mode is enabled.");
            }
        });