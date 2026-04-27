# com.dave6.item-system

아이템과 인벤토리를 위한 컨테이너 기반 시스템.

## 아키텍처

프로젝트는 다음과 같이 구성됨

- Domain
  아이템과 컨테이너의 순수C# 로직과 데이터 구조

- Application
  컨테이너 조작 및 시스템 로직
  유니티 엔진을 사용

## 핵심 개념

- **ItemInstance**  
  런타임 아이템 객체. 내부에 하나 이상의 컨테이너를 가질 수 있음

- **IItemContainer**  
  아이템을 담는 최소 단위 (Grid, Socket 등)

- **ContainerCollection**  
  하나의 루트 컨테이너 + 동적으로 추가되는 확장 컨테이너를 관리하는 단위

- **Extension System**  
  특정 아이템(예: 장비)에 의해 컨테이너가 동적으로 추가/제거되는 구조

---

## 현재 구현된 기능
- Item / Container Domain 구조
- Grid / Socket 기반 컨테이너 시스템
- ContainerCollection 기반 확장 가능한 컨테이너 구조
- 아이템 이동 / 추가 / 제거 서비스 (ContainerService)
- 장비 기반 Extension 컨테이너 동적 추가/제거
- 컨테이너 순환 참조 방지 로직

---

## 특징

- 아이템이 컨테이너를 소유하는 구조 (Item → Container)
- 컨테이너 확장을 통한 유연한 인벤토리 구성
- UI와 분리된 순수 도메인 설계