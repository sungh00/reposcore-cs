using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;

public class GitHubAnalyzer
{
    private readonly GitHubClient _client;

    public GitHubAnalyzer()
    {
        _client = CreateClient("reposcore-cs");
    }

    private GitHubClient CreateClient(string productName)
    {
        return new GitHubClient(new ProductHeaderValue(productName));
    }

    private void HandleError(Exception ex)
    {
        Console.WriteLine($"❗ 알 수 없는 오류가 발생했습니다: {ex.Message}");
        Environment.Exit(1);
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

            var targetLabels = new[] { "bug", "documentation", "enhancement" };
            var labelCounts = targetLabels.ToDictionary(label => label, _ => 0);

            // PR 분류 (병합된 것만)
            foreach (var pr in prs.Where(p => p.Merged == true))
            {
                var labels = pr.Labels.Select(l => l.Name.ToLower()).ToList();
                foreach (var label in targetLabels)
                {
                    if (labels.Contains(label))
                        labelCounts[label]++;
                }
            }

            // 이슈 분류 (PR 제외)
            foreach (var issue in issues)
            {
                if (issue.PullRequest != null) continue;
                var labels = issue.Labels.Select(l => l.Name.ToLower()).ToList();
                foreach (var label in targetLabels)
                {
                    if (labels.Contains(label))
                        labelCounts[label]++;
                }
            }

            // 결과 출력
            Console.WriteLine("\n📊 GitHub Label 통계 결과");

            Console.WriteLine("\n✅ Pull Requests (Merged)");
            foreach (var label in targetLabels)
            {
                Console.WriteLine($"- {char.ToUpper(label[0]) + label.Substring(1)} PRs: {labelCounts[label]}");
            }

            Console.WriteLine("\n✅ Issues");
            foreach (var label in targetLabels)
            {
                Console.WriteLine($"- {char.ToUpper(label[0]) + label.Substring(1)} Issues: {labelCounts[label]}");
            }
        }
        catch (RateLimitExceededException)
        {
            Console.WriteLine("❗ API 호출 한도(Rate Limit)를 초과했습니다. 잠시 후 다시 시도해주세요.");
            Environment.Exit(1);
        }
        catch (AuthorizationException)
        {
            Console.WriteLine("❗ 인증 실패: 올바른 토큰을 사용했는지 확인하세요.");
            Environment.Exit(1);
        }
        catch (NotFoundException)
        {
            Console.WriteLine("❗ 저장소를 찾을 수 없습니다. owner/repo 이름을 확인하세요.");
            Environment.Exit(1);
        }
        catch (Exception ex)
        {
            HandleError(ex);
        }
    }
}
