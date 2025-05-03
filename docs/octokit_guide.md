
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

해당 명령어를 사용하기 전
Visual Studio → 새 프로젝트 생성 → #설치 가이드 따라 Octokit설치 후 아래 명령어 입력
//결과적으로 해당 코드는 GitHub API와 통신할 준비가 된 것이기 때문에 아무것도 출력하지 않습니다.

```csharp
using Octokit;

var client = new GitHubClient(new ProductHeaderValue("MyApp"));
```

## 인증 설정 (Personal Access Token 사용)

해당 코드를 실행하면 Client객체로 실행되는 모든 요청에 대해 해당 토큰이 포함되어
인증된 사용자로부터 요청을 보낼 수 있게 됩니다.
//즉, API 요청에 사용할 인증 정보를 설정하는 코드이기 때문에 아무것도 출력하지 않습니다.

```csharp
client.Credentials = new Credentials("your_github_token");
```

## 저장소 조회

"owner_name" = 개인 사용자명 또는 조직명
"repository_name" = 저장소 이름
//가져온 저장소 객체의 이름을 반환하기 때문에 "Repository Name: linux"와 같이 출력된다.

```csharp
var repository = await client.Repository.Get("owner_name", "repository_name");
Console.WriteLine($"Repository Name: {repository.Name}");
```

## 이슈 목록 조회

해당 코드는 특정 GitHub 저장소의 이슈 목록을 가져와서 출력하는 코드이다.
//"owner_name/repository_name"에 저장된 모든 이슈 정보를 가져와서 이슈 번호와 제목을 콘솔에 출력한다. 

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
