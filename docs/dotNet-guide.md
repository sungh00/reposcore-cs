# .NET Guide

이 가이드는 .NET 콘솔 애플리케이션을 생성, 빌드, 실행, 테스트하는 과정을 안내합니다.

---
# Useage

## 프로젝트 생성

```bash
dotnet new console -n {프로젝트_이름}
```

해당 명령어를 실행하면 다음과 같은 폴더 및 파일 구조가 생성됩니다:

```
{프로젝트_이름}/
├── {프로젝트_이름}.csproj
└── Program.cs
```

---

## 프로젝트 빌드

```bash
dotnet build
```

빌드 결과물은 `bin/` 폴더에 생성됩니다.

---

## 프로젝트 실행

```bash
dotnet run
```

`Program.cs`의 `Main` 메서드가 실행됩니다.

---

## 테스트

```bash
dotnet test
```

> 테스트 프로젝트(`xUnit`, `NUnit` 등)가 존재할 경우 실행됩니다.  
> 예시: `dotnet new xunit -n MyApp.Tests`

---

## 참고 자료

- [.NET CLI 공식 문서 (GitHub)](https://github.com/dotnet/docs/tree/main/docs/core/tools)  
- [Microsoft Learn: .NET CLI 개요](https://learn.microsoft.com/dotnet/core/tools/)
