using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class GitHubAnalyzer
{
    private readonly GitHubClient _client;

    public GitHubAnalyzer()
    {
        _client = new GitHubClient(new ProductHeaderValue("reposcore-cs"));
    }

    public async Task Analyze(string owner, string repo)
    {
        IReadOnlyList<PullRequest> prs;
        IReadOnlyList<Issue> issues;

        try
        {
            prs = await _client.PullRequest.GetAllForRepository(owner, repo, new PullRequestRequest
            {
                State = ItemStateFilter.Closed
            });
        }
        catch (RateLimitExceededException)
        {
            Console.WriteLine("❗ GitHub API 요청 한도를 초과했습니다. 잠시 후 다시 시도해주세요.");
            return;
        }
        catch (AuthorizationException)
        {
            Console.WriteLine("❗ 인증 오류가 발생했습니다. Access Token 또는 권한을 확인해주세요.");
            return;
        }
        catch (NotFoundException)
        {
            Console.WriteLine("❗ 요청한 저장소를 찾을 수 없습니다. 저장소 이름 또는 소유자를 확인하세요.");
            return;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❗ PR 정보 불러오기 중 오류 발생: {ex.Message}");
            return;
        }

        try
        {
            issues = await _client.Issue.GetAllForRepository(owner, repo, new RepositoryIssueRequest
            {
                State = ItemStateFilter.All
            });
        }
        catch (RateLimitExceededException)
        {
            Console.WriteLine("❗ GitHub API 요청 한도를 초과했습니다. 잠시 후 다시 시도해주세요.");
            return;
        }
        catch (AuthorizationException)
        {
            Console.WriteLine("❗ 인증 오류가 발생했습니다. Access Token 또는 권한을 확인해주세요.");
            return;
        }
        catch (NotFoundException)
        {
            Console.WriteLine("❗ 요청한 저장소를 찾을 수 없습니다. 저장소 이름 또는 소유자를 확인하세요.");
            return;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❗ 이슈 정보 불러오기 중 오류 발생: {ex.Message}");
            return;
        }

        int pr_bug = 0, pr_doc = 0, pr_feat = 0;
        int issue_bug = 0, issue_doc = 0, issue_feat = 0;

        foreach (var pr in prs.Where(p => p.Merged == true))
        {
            var labels = pr.Labels.Select(l => l.Name.ToLower()).ToList();

            if (labels.Contains("bug")) pr_bug++;
            if (labels.Contains("documentation")) pr_doc++;
            if (labels.Contains("enhancement")) pr_feat++;
        }

        foreach (var issue in issues)
        {
            if (issue.PullRequest != null) continue;

            var labels = issue.Labels.Select(l => l.Name.ToLower()).ToList();

            if (labels.Contains("bug")) issue_bug++;
            if (labels.Contains("documentation")) issue_doc++;
            if (labels.Contains("enhancement")) issue_feat++;
        }

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
}
