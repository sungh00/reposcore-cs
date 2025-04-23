using Cocona;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Text.Json;

CoconaApp.Run(async(
    [Argument] string[] repos,
    [Option('v', Description = "자세한 로그 출력을 활성화합니다.")] bool verbose
) =>
{
    Console.WriteLine($"Repository: {String.Join("\n ", repos)}");

    if (verbose)
    {
        Console.WriteLine("Verbose mode is enabled.");
    }

    if (repos.Length != 2)
    {
        Console.WriteLine("❗ repository 인자는 'owner repo' 순서로 2개가 필요합니다.");
        Environment.Exit(1);  // 오류 발생 시 exit code 1로 종료
        return;
    }

    string owner = repos[0];
    string repo = repos[1];

    string Url = "https://api.github.com/repos";
    string apiUrl = $"{Url}/{owner}/{repo}";


    try
    {
        using (HttpClient client = new HttpClient())
        {
            client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("CoconaApp", "1.0"));
            HttpResponseMessage response = await client.GetAsync(apiUrl);
            string result = await response.Content.ReadAsStringAsync();

            //var jsonElement = JsonDocument.Parse(result).RootElement;
            //var prettyJson = JsonSerializer.Serialize(jsonElement, new JsonSerializerOptions { WriteIndented = true });

            Console.WriteLine($"[INFO]API 응답 코드: {response.StatusCode}");
            //Console.WriteLine($"[INFO]API 응답 내용:\n{prettyJson}");
        }
    }
    catch (Exception e)
    {
        Console.WriteLine($"❗ 오류 발생: {e.Message}");
        Environment.Exit(1);
    }


    try
    {
        // var analyzer = new GitHubAnalyzer();
        // analyzer.Analyze(repos[0], repos[1]);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❗ 오류 발생: {ex.Message}");
        Environment.Exit(1);  // 예외 발생 시 exit code 1로 종료
    }

    Environment.Exit(0);  // 정상 종료 시 exit code 0으로 종료
});
