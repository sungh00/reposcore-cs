using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cocona;

public class GitHubAnalyzer
{
    private readonly GitHubClient _client;

    public GitHubAnalyzer()
    {
        _client = new GitHubClient(new ProductHeaderValue("reposcore-cs"));
    }

    public void Analyze(string owner, string repo)
    {
        try
        {
            // 병합된 PR 수집
            var prs = _client.PullRequest.GetAllForRepository(owner, repo, new PullRequestRequest
            {
                State = ItemStateFilter.Closed
            }).Result;

            // 전체 이슈 수집
            var issues = _client.Issue.GetAllForRepository(owner, repo, new RepositoryIssueRequest
            {
                State = ItemStateFilter.All
            }).Result;

            // 라벨 기준 통계 변수
            int pr_bug = 0, pr_doc = 0, pr_feat = 0;
            int issue_bug = 0, issue_doc = 0, issue_feat = 0;

            // PR 분류 (병합된 것만)
            foreach (var pr in prs.Where(p => p.Merged == true))
            {
                var labels = pr.Labels.Select(l => l.Name.ToLower()).ToList();

                if (labels.Contains("bug")) pr_bug++;
                if (labels.Contains("documentation")) pr_doc++;
                if (labels.Contains("enhancement")) pr_feat++;
            }

            // 이슈 분류 (PR 제외)
            foreach (var issue in issues)
            {
                if (issue.PullRequest != null) continue;

                var labels = issue.Labels.Select(l => l.Name.ToLower()).ToList();

                if (labels.Contains("bug")) issue_bug++;
                if (labels.Contains("documentation")) issue_doc++;
                if (labels.Contains("enhancement")) issue_feat++;
            }

            // 결과 출력
            Console.WriteLine("\n📊 GitHub Label 통계 결과");

            Console.WriteLine("\n✅ Pull Requests (Merged)");
            Console.WriteLine($"- Bug PRs: {pr_bug}");
            Console.WriteLine($"- Documentation PRs: {pr_doc}");
            Console.WriteLine($"- Enhancement PRs: {pr_feat}");

            Console.WriteLine("\n✅ Issues");
            Console.WriteLine($"- Bug Issues: {issue_bug}");
            Console.WriteLine($"- Documentation Issues: {issue_doc}");
            Console.WriteLine($"- Enhancement Issues: {issue_feat}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❗ 오류 발생: {ex.Message}");
            Environment.Exit(1);  // 예외 발생 시 exit code 1로 종료
        }
    }
}

CoconaApp.Run((
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
        Console.WriteLine("❗ repository 인자는 'owner repo' 순서로 2개가 필요합니다.");
        Environment.Exit(1);  // 오류 발생 시 exit code 1로 종료
        return;
    }

    try
    {
        var analyzer = new GitHubAnalyzer();
        analyzer.Analyze(repository[0], repository[1]);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❗ 오류 발생: {ex.Message}");
        Environment.Exit(1);  // 예외 발생 시 exit code 1로 종료
    }

    Environment.Exit(0);  // 정상 종료 시 exit code 0으로 종료
});
