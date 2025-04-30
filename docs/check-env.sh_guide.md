# node 버전 확인
if command -v node &>/dev/null; then
  node_version=$(node -v | sed 's/v//')
  if (( $(echo "$node_version >= 18.0.0" | bc -l) )); then
    echo "✔ node ≥ 18.0.0 → OK (현재: $node_version)"
  else
    echo "✖ node ≥ 18.0.0 → FAIL (현재: $node_version)"
  fi
else
  echo "✖ node ≥ 18.0.0 → FAIL (node가 설치되지 않음)"
fi

# python 버전 확인
if command -v python &>/dev/null; then
  python_version=$(python --version 2>&1 | awk '{print $2}')
  if (( $(echo "$python_version >= 3.10" | bc -l) )); then
    echo "✔ python ≥ 3.10 → OK (현재: $python_version)"
  else
    echo "✖ python ≥ 3.10 → FAIL (현재: $python_version)"
  fi
else
  echo "✖ python ≥ 3.10 → FAIL (python이 설치되지 않음)"
fi

# docker 설치 확인
if command -v docker &>/dev/null; then
  echo "✔ docker 설치됨"
else
  echo "✖ docker 설치되지 않음"
fi

# gh CLI 설치 확인
if command -v gh &>/dev/null; then
  echo "✔ gh CLI 설치됨"
else
  echo "✖ gh CLI 없음"
fi

# GitHub API 연결 확인 (예시로 GitHub API에 ping 요청)
if curl -s https://api.github.com >/dev/null; then
  echo "✔ GitHub API 연결 성공"
else
  echo "✖ GitHub API 연결 실패 (timeout)"
fi