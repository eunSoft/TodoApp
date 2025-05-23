# 정은의 Todo List 데스크탑 앱
> WPF & SQLite 기반의 간단하고 직관적인 할 일 관리 데스크탑 애플리케이션

## 주요 기능
- [x] 할 일 추가, 기한 지정
- [x] 할 일 완료 상태 체크
- [x] 삭제 기능
- [x] 기한 지난 항목 색상 표시 (빨간색)
- [x] 완료 항목 보기 토글 (필터 기능)
- [x] SQLite 로컬 DB를 사용하여 데이터 영구 저장
- [x] 실시간 UI 반영 (바인딩 기반 ListView 사용)

## 사용 기술 스택
| 구분 | 내용 |
|------|------|
| 언어 | C# (.NET 8.0) |
| UI 프레임워크 | WPF (Windows Presentation Foundation) |
| 데이터베이스 | SQLite |
| 패키지 매니저 | NuGet (`System.Data.SQLite`) |
| 기타 | MVVM 일부 도입, Data Binding, Event Handling |


## 개발하며 배운 점

- WPF의 ListView + GridView 바인딩 구조 이해
- SQLite와 C# 연동을 통한 로컬 데이터 저장
- 조건부 스타일링 (기한 지난 항목 색상 처리)
- 사용자 중심의 UX 개선: 유효성 검사, 필터 기능 추가

## 향후 개선 방향

- [ ] 데이터 정렬 (기한순, 등록순)
- [ ] 검색 기능
- [ ] 간단한 통계 페이지 (오늘 할 일 수, 완료 비율 등)
- [ ] 앱 아이콘 및 실행파일 배포 포장 개선
