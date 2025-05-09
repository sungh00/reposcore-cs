using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;

public class GitHubAnalyzer
{
    private readonly GitHubClient _client;

    // GitHubClient를 초기화하고, 사용자로부터 입력받은 GitHub 토큰을 인증에 사용합니다.
    public GitHubAnalyzer(string token)
    {
        _client = CreateClient("reposcore-cs", token);
    }

    // GitHubClient를 생성하고, 인증 정보를 설정합니다.
    private GitHubClient CreateClient(string productName, string token)
    {
        var client = new GitHubClient(new ProductHeaderValue(productName));

        // 인증이 제공된 경우, 사용자 토큰을 사용하여 인증
        if (!string.IsNullOrEmpty(token))
        {
            client.Credentials = new Credentials(token);
        }

        return client;
    }

    // 예외를 처리하는 메서드로, 오류 메시지를 출력하고 프로그램을 종료합니다.
    private void HandleError(Exception ex)
    {
        Console.WriteLine($"❗ 알 수 없는 오류가 발생했습니다: {ex.Message}");
        Environment.Exit(1);
    }

    // GitHub 저장소에서 PR(Pull Request) 및 Issue 정보를 분석하는 메서드
    public void Analyze(string owner, string repo)
    {
        try
        {
            // 1. 병합된 PR(Closed 상태인 PR) 목록을 가져옵니다.
            var prs = _client.PullRequest.GetAllForRepository(owner, repo, new PullRequestRequest
            {
                State = ItemStateFilter.Closed
            }).Result;  // 비동기 호출을 동기적으로 처리

            // 2. 모든 이슈(열린 상태와 닫힌 상태 모두) 목록을 가져옵니다.
            var issues = _client.Issue.GetAllForRepository(owner, repo, new RepositoryIssueRequest
            {
                State = ItemStateFilter.All
            }).Result;

            // 3. 'bug', 'documentation', 'enhancement' 레이블에 대한 카운트를 셀 딕셔너리 초기화
            var targetLabels = new[] { "bug", "documentation", "enhancement" };
            var labelCounts = targetLabels.ToDictionary(label => label, _ => 0);

            // 4. PR 중에서 병합된 PR만 필터링하여 레이블을 분석합니다.
            foreach (var pr in prs.Where(p => p.Merged == true))
            {
                // PR의 레이블을 소문자로 변환하여 리스트로 만듦
                var labels = pr.Labels.Select(l => l.Name.ToLower()).ToList();

                // 지정된 레이블이 PR의 레이블에 포함되어 있으면 카운트 증가
                foreach (var label in targetLabels)
                {
                    if (labels.Contains(label))
                        labelCounts[label]++;
                }
            }

            // 5. Issue 중에서 PR에 속하지 않은(즉, PR에서 파생되지 않은) 이슈들을 분석
            foreach (var issue in issues)
            {
                if (issue.PullRequest != null) continue;  // PR에 속하는 이슈는 건너뜁니다.

                // 이슈의 레이블을 소문자로 변환하여 리스트로 만듦
                var labels = issue.Labels.Select(l => l.Name.ToLower()).ToList();

                // 지정된 레이블이 이슈의 레이블에 포함되어 있으면 카운트 증가
                foreach (var label in targetLabels)
                {
                    if (labels.Contains(label))
                        labelCounts[label]++;
                }
            }

            // 6. 결과 출력
            Console.WriteLine("\n📊 GitHub Label 통계 결과");

            Console.WriteLine("\n✅ Pull Requests (Merged)");
            // 'bug', 'documentation', 'enhancement' 레이블에 대해 분석된 PR 수 출력
            foreach (var label in targetLabels)
            {
                Console.WriteLine($"- {char.ToUpper(label[0]) + label.Substring(1)} PRs: {labelCounts[label]}");
            }

            Console.WriteLine("\n✅ Issues");
            // 'bug', 'documentation', 'enhancement' 레이블에 대해 분석된 Issue 수 출력
            foreach (var label in targetLabels)
            {
                Console.WriteLine($"- {char.ToUpper(label[0]) + label.Substring(1)} Issues: {labelCounts[label]}");
            }
        }
        catch (RateLimitExceededException)
        {
            // Rate Limit 초과시 메시지 출력
            Console.WriteLine("❗ API 호출 한도(Rate Limit)를 초과했습니다. 잠시 후 다시 시도해주세요.");
            Environment.Exit(1);  // 프로그램 종료
        }
        catch (AuthorizationException)
        {
            // 인증 오류 발생 시 메시지 출력
            Console.WriteLine("❗ 인증 실패: 올바른 토큰을 사용했는지 확인하세요.");
            Environment.Exit(1);  // 프로그램 종료
        }
        catch (NotFoundException)
        {
            // 저장소를 찾을 수 없을 때 메시지 출력
            Console.WriteLine("❗ 저장소를 찾을 수 없습니다. owner/repo 이름을 확인하세요.");
            Environment.Exit(1);  // 프로그램 종료
        }
        catch (Exception ex)
        {
            // 그 외의 모든 예외를 처리하는 부분
            HandleError(ex);
        }
    }
}