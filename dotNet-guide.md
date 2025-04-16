# .NET guide
이 가이드는 .NET 콘솔 애플리케이션을 생성, 빌드, 실행, 테스트하는 과정을 안내합니다.

# Useage
프로젝트 생성
```bash
dotnet new console -n {프로젝트 이름}
```

해당 명령어 실행 시 아래와 같이 폴더와 파일 생성
```bash
{프로젝트 이름}/
├── {프로젝트 이름}.csproj
└── Program.cs
```

프로젝트 빌드
```bash
dotnet build
```

프로젝트 실행
```bash
dotnet run
```

테스트
```bash
dotnet test
```