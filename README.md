# com.dave6.item-system

아이템과 인벤토리를 위한 컨테이너 기반 시스템.

## 아키텍처

프로젝트는 다음과 같이 구성됨

- Domain
  아이템과 컨테이너의 순수C# 로직과 데이터 구조

- Application
  컨테이너 조작 및 시스템 로직
  유니티 엔진을 사용

- UnityUI
  UI Toolkit 기반 컨테이너 뷰


## 현재 구현된 기능:
- Item / Container Domain 구조
- Container 기반 아이템 저장
- 유니티 엔진에서 사용되는 Application 레이어
- 컨테이너의 Grid/Socket 레이아웃
- 컨테이너 뷰 UI