# 유니티 게임 제작

# 해야 할 것

1. 세이브 포인트 역할 오브젝트 생성
    - Platform 위에 그대로 존재할 경우 경로 유추가 가능
    - 특정 지점을 넘을 때 마다 세이브를 할 수 있도록?
        - 지점은 보이지 않는 하나의 긴 직사각형 벽을 두어서 이곳을 통과하면 세이브 + 오브젝트 비활성화
    - 아니면 경로를 기억할 수 있도록 표시를 한다?
        - 지나온 경로의 색을 표시하거나 하는 등으로?
2. 추락시 판정 수정
    - 발판에서 발이 떼지는 순간 점프 불가
        - 매 프레임마다 확인?
        - 충돌이 해제되는 순간을 사용가능?
3. 발판 랜덤 생성
    - 시드에 맞춰 랜덤 생성을 할 수 잇도록
    - 그럼 발판의 패턴같은 것은 어떻게 구현?

# 존재하는  object 목록

## 주요 object

- Player
    - Main Camera
    - Main Camera 3D
- Platform
    - Correct
    - Wrong
- Item
- Global Volume

## 모든 Script

### *Player*

- PlayerReset
    - 시작시 현재 좌표를 저장
    - 매 프레임마다 현재의 Y축 좌표를 체크하여, 기준 좌표보다 낮아질 경우 ResetToStart() 함수를 실행
    - ResetToStart() 함수는 startPos 변수에 담긴 좌표로  현재 Player 오브젝트의 위치를 변경, SimpleMove 함수로부터 Y축 변화량과 현재 바닥여부 저장 변수를 가져와 초기화함
    - OnControllerColliderHit 함수를 통해 이 Player 오브젝트가 어떠한 오브젝트와 충돌할 경우
        - 그 오브젝트가 Inspector 창에 입력한 태그명의 오브젝트라면
        - 그 발판의 좌표를 startPost 변수에 저장함 → ResetToStart()함수에 의해 초기화 되는 위치가 변경됨
- SimpleMove
    - 시작시 Player 오브젝트의 CharacterController 컴포넌트를 가져옴
    - 매 프레임마다 입력키에 맞춰 player의 위치를 변화시킴
        - 현재 바닥에 닿아있는지 여부는 isGround 변수에 true 또는 false 형태로 저장하여 판단함
        - isGround 변수가 true고 점프키를 눌렀다면 점프를 하고 isGround를 false로 변경
    - OnControllerColliderHit 함수를 통해 이 Player 오브젝트가 어떠한 오브젝트와 충돌할 경우
        - 그 오브젝트가 Wrong 태그명을 가지고 있다면 해당 오브젝트를 파괴
        - 아니라면 isGround를 true로 변경하고 Y축 변화량을 0으로 만들어 하강을 막음
- ViewToggle

### *Cam*

- CamRot

### *Playform*

- ~~PlatformFuc~~
    - 잘못된 발판과 부딛힐 경우 해당 발판 제거
    - SimpleMove 함수와 겹치므로 해당 스크립트 제거 필요
- PanelJump
    - 현재 Platform 오브젝트의 자식으로 있는 오브젝트들 중에서, Wrong 태그가 적용된 오브젝트들을 foreach로 각각 방문
    - 방문한 오브젝트에 대해 비동기적으로 Coroutine을 실행시키고 다음 Wrong 태그 오브젝트로 이동
    - Coroutine은 MoveWrongPanel 함수를 이용해 진행
- TagColorizer1
    - Item에서 onTriggerEnter 함수를 통해 ColorPanels 함수를 호출할 경우, 아무것도 할당되어 있지 않은 Coroutine 변수에 StartCoroutine 함수의 결과를 할당하는 것으로써 비동기적으로 HighlightRoutine 함수를 실행
    - 현재 Platform 오브젝트의 자식으로 잇는 오브젝트들을 전부 순회하며 Wrong 태그가 적용되어 있는 오브젝트와 Correct 태그가 적용되어 있는 오브젝트에 대해 서로 다른 Material을 할당함

### *Item*

- ItemTrigger
    - onTriggerEnter 함수를 이용, Player 태그를 가진 물체와 부딪힌 경우에 발동
    - 널 조건부 연산자 `?.`를 이용, OnitemCollected 변수에 씬에 존재하는 오브젝트를 할당
        - 해당 오브젝트의 inspector 중에서 적절한 Script 내부의 코드 중 적절한 코드(함수)를 호출하여 실행
        - 만약 이 오브젝트 할당이 이루어져 있지 않을 경우 건너 뜀
    - GlobalVolumeObject에 오브젝트가 할당되어 있다면 DisableVolumeTemporarily함수를 실행
        - 적용되어 있는 globalVolume을 해제후 설정한 시간만큼 대기하다가 다시 활성화
    - player와 부딛힌 경우 item은 무조건 제거

### *SceneChange*

- SceneSwitch
    - onTriggerEnter 함수를 이용, Player 태그를 가진 물체와 부딪힌 경우에 발동
    - 다른 Scene을 Load함

### *etc.*

- SpawnTimer

# Scene별 object와 script 매치

## - level 0 -

### Player

- **Script**
    - SimpleMove
    - Player Reset
    - View Toggle
- Tag : Player

### Main Camera

- **Script**
    - Cam Rot
- Tag : MainCamera

### Main Camera 3D

- Tag : MainCamera

### Scene Switch

- 설명
    - 닿으면 Scence을 level 1, 2, 3로 각각 변경하는 동일명의 오브젝트 3개 존재
- **Script**
    - SceneSwitch

### 그외

- Wall
    - 약국 벽
- Plane
    - 약국으로 향하는 바닥
- Cube
    - 약국 내부 가구
- NPC
    - 약사

## - level 1 -

<aside>
💡

시각 장애인의 불편을 구현

Global Volume 을 통해 Player의 MainCamera 의 시야를 제한함

Volume의 Vignette 기능으로 시야 제한

아이템은 발판의 위치를 보여주는 아이템과 시야를 일시적으로 정상화 하는 아이템

</aside>

### Player

- **Script**
    - SimpleMove
    - Player Reset
    - View Toggle
- Tag : Player

### Platform

- **Script**
    - PlatformFunc
    - PanelJump

### Global Volume

- **Volume Profile**
    - Global Volume Profile

### Item(1)

- **Script**
    - Item Trigger
        - Pannel Jump 할당
- 설명
    - 플레이어 정면 기준 왼쪽에 위치

### Item(2)

- **Script**
    - Item Trigger
        - Global Volume 비활성화
        - n초간
- 설명
    - 플레이어 정면 기준 오른쪽에 위치

### Start

### Goal

### SceneSwitch

- **Script**
    - SceneSwitch
        - level 0으로 씬 전환

## - level 2 -

<aside>
💡

색각이상 모드

아이템은  발판의 위치를 보여주는 아이템과 올바른 발판을 구분하는 색을 일시적으로 보여주는 아이템

</aside>

### Player

- **Script**
    - SimpleMove
    - Player Reset
    - View Toggle
- Tag : Player

### Platform

- **Script**
    - PlatformFunc
    - PanelJump

### Global Volume

- **Volume Profile**
    - Global Volume Profile

### Item(1)

- **Script**
    - Item Trigger
        - Pannel Jump 할당
- 설명
    - 플레이어 정면 기준 왼쪽에 위치

### Item(2)

- **Script**
    - Item Trigger
        - Tagcolorizer1 활성화
        - n초간
- 설명
    - 플레이어 정면 기준 오른쪽에 위치

### Start

### Goal

### SceneSwitch

- **Script**
    - SceneSwitch
        - level 0으로 씬 전환

## - level 3 -

<aside>
💡

문맹

추가 기능을 고려해야 하는 사항

</aside>