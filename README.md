# reposcore-cs
A CLI for scoring student participation in an open-source class repo, implemented in C#.

**주의**: 처음 만들 때 dotnet 7.0으로 환경을 설정했다 8.0으로 설정을 바꿨기 때문에 그 이전에 Codespace를 생성한 경우 코드스페이스 vscode 에디터에서 `Ctrl-Shift-P`를 누르고 rebuild로 검색해서 Codespace rebuild를 선택하면 8.0 환경으로 다시 가상머신이 만들어집니다. (단, 기존에 컨테이너 안에 있는 파일들은 깃헙에 push하지 않은 내용들 중에 필요한 게 있으면 혹시 모르니 백업해 놓아야 ...)

## 참여조건
해당 repository는 프로젝트 참여 점수 16점 미만(즉, 15점 이하)인 학생만 참여할 수 있습니다

## Score Formula
아래는 PR 개수와 이슈 개수의 비율에 따라 점수로 인정가능한 최대 개수를 구하고 각 배점에 따라 최종 점수를 산출하는 공식이다.

- $P_{fb}$ : 기능 또는 버그 관련 Merged PR 개수 (**3점**) ($P_{fb} = P_f + P_b$)  
- $P_d$ : 문서 관련 Merged PR 개수 (**2점**)  
- $P_t$ : 오타 수정 Merged PR 개수 (**1점**)  
- $I_{fb}$ : 기능 또는 버그 관련 Open 또는 해결된 이슈 개수 (**2점**) ($I_{fb} = I_f + I_b$)  
- $I_d$ : 문서 관련 Open 또는 해결된 이슈 개수 (**1점**)

$P_{\text{valid}} = P_{fb} + \min(P_d + P_t, 3 \times \max(P_{fb}, 1)) \quad$ 점수 인정 가능 PR 개수  
$I_{\text{valid}} = \min(I_{fb} + I_d, 4 \times P_{\text{valid}}) \quad$ 점수 인정 가능 이슈 개수

PR의 점수를 최대로 하기 위해 기능/버그 PR을 먼저 계산한 후 문서 PR과 오타 PR을 계산합니다.  
($P_{fb}$이 0일 경우에도 문서 PR과 오타 PR 합산으로 최대 3개까지 인정됩니다.)

$P_{fb}^* = \min(P_{fb}, P_{\text{valid}}) \quad$ 기능/버그 PR 최대 포함  

$P_d^* = \min(P_d, P_{\text{valid}} - P_{fb}^*)$  문서 PR 포함

$P_t^* = P_{\text{valid}} - P_{fb}^* - P_d^*$  남은 개수에서 오타 PR 포함

이슈의 점수를 최대로 하기 위해 기능/버그 이슈를 먼저 계산한 후 문서 이슈를 계산합니다.

$I_{fb}^* = \min(I_{fb}, I_{\text{valid}}) \quad$ 기능/버그 이슈 최대 포함  
$I_d^* = I_{\text{valid}} - I_{fb}^* \quad$ 남은 개수에서 문서 이슈 포함

최종 점수 계산 공식:  
$S = 3P_{fb}^* + 2P_d^* + 1P_t^* + 2I_{fb}^* + 1I_d^*$

## CLI 구현 설명

아래는 본 프로젝트에 사용된 Cocona 기반 CLI 코드입니다.  
기본적인 명령행 인자 처리와 `--verbose` 옵션을 실험하는 용도로 작성되었습니다.

## 사용 방법
 
아래 명령어를 통해 CLI를 실행할 수 있습니다.
 
```bash
dotnet run -- owner repo
dotnet run -- owner repo --verbose
dotnet run -- --version
dotnet run -- --help
```

* 옵션 등을 정리해 나가는 단계이므로 실행 예시는 계속해서 변경할 것이므로 일단 삭제하였음
* 앞으로 다른 프로젝트처럼 -h나 --help 옵션으로 실행시켜 출력되는 시놉시스와 간단한 도움말이 자동으로 여기에 생성되는 방식으로 템플릿화 해야 함

## [.NET 가이드](docs/dotNet-guide.md)

## [프로젝트 기여 및 작업 규칙](docs/project_guidelines.md)

## [Github Token 생성 방법](docs/github-token-guide.md)
