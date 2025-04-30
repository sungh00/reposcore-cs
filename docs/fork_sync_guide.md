# 포크 동기화 가이드

이 가이드는 GitHub에서 포크한 저장소가 원본 저장소보다 뒤처졌을 때 최신 상태로 동기화하는 방법을 설명합니다.  
예: `This branch is n commits behind` 메시지가 표시되는 경우.

---

## 웹 UI에서 포크 동기화

1. GitHub에서 **포크된 리포지토리**의 메인 페이지로 이동  
2. **파일 목록 상단의 `Sync fork` 드롭다운** 클릭  
3. 업스트림 리포지토리의 변경사항 확인 후 `Update branch` 클릭

> GitHub 웹 인터페이스만으로 간편하게 동기화 가능

---

## GitHub CLI로 포크 동기화

Codespaces에는 기본적으로 `gh` (GitHub CLI)가 설치되어 있습니다. CLI 명령으로 빠르게 동기화하려면 다음을 실행하세요:

```bash
gh repo sync owner/cli-fork -b BRANCH-NAME
```

- `owner/cli-fork`: 포크한 저장소 전체 이름 (예: `yourname/project`)
- `-b`: 동기화하려는 브랜치명

---

## 원본 저장소 등록 (최초 1회만 필요)

```bash
git remote add upstream https://github.com/original-user/original-repo.git
```

> 이미 등록돼 있다면 아래 명령어로 확인 가능:
```bash
git remote -v
```

---

## 원본 저장소에서 변경 내용 가져오기

```bash
git fetch upstream
```

---

## 병합 또는 리베이스 방식으로 동기화

### 방법 1: 병합 (Merge)

```bash
git checkout main
git merge upstream/main
```

### 방법 2: 리베이스 (Rebase)

```bash
git checkout main
git rebase upstream/main
```

> 브랜치명이 `master`인 경우 `main` 대신 `master` 사용

---

## 변경사항 푸시

```bash
git push origin main
```

---

## 원본과 완전히 동일하게 만들기 (강제 리셋)

> 로컬 수정사항을 무시하고 원본 저장소와 완전히 동일하게 만들고 싶을 경우

```bash
git fetch upstream
git checkout main
git reset --hard upstream/main
git push origin main --force
```

> ⚠️ `--force`는 주의해서 사용!  
> 협업 중이라면 강제 푸시로 다른 사람의 커밋이 사라질 수 있음

---

## 🔗 참고 자료

- [GitHub 공식 포크 가이드](https://docs.github.com/en/get-started/quickstart/fork-a-repo)
- [Git 리모트 문서 (git remote)](https://git-scm.com/docs/git-remote)
- [Git rebase vs merge](https://www.atlassian.com/git/tutorials/merging-vs-rebasing)
