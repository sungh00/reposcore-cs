
# Octokit.NET guide

이 가이드는 Octokit.NET을 이용해 GitHub API를 호출하고, 저장소 및 이슈를 관리하는 과정을 안내합니다.

# 설치

```bash
dotnet add package Octokit
```

또는  
Visual Studio ➔ NuGet 패키지 관리자 ➔ `Octokit` 검색 후 설치

# 기본 사용법

## GitHubClient 생성

```csharp
using Octokit;

var client = new GitHubClient(new ProductHeaderValue("MyApp"));
```

## 인증 설정 (Personal Access Token 사용)

```csharp
client.Credentials = new Credentials("your_github_token");
```

## 저장소 조회

```csharp
var repository = await client.Repository.Get("owner_name", "repository_name");
Console.WriteLine($"Repository Name: {repository.Name}");
```

## 이슈 목록 조회

```csharp
var issues = await client.Issue.GetAllForRepository("owner_name", "repository_name");

foreach (var issue in issues)
{
    Console.WriteLine($"#{issue.Number} - {issue.Title}");
}
```

# 참고 자료

- [Octokit.NET GitHub Repository](https://github.com/octokit/octokit.net)
- [GitHub REST API 공식 문서](https://docs.github.com/en/rest)
