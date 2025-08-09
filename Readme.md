# 유니티 게임 제작

# 해야 할 것

1. Dialogue 시스템 기능 추가
    - 현재 모든 대사가 끝나야만 선택지가 출력되는 상황임. 이를 수정하여 Dialogue 에셋에서 하나의 리스트를 읽어오고 내부의 자료형이 어떤가에 따라 Dialogue 출력을 맞춰 변형하도록 하는 것으로
    - 스크립트 파일 입력시 이에 맞춰서 출력하도록 하는 기능 추가
    - 대사가 출력중일 때에는 이동이 불가능하도록 하는 기능
    - 버튼 출력시에도 대사가 그대로 유지되도록 하는 기능
    - 선택 결과에 따른 다른 대사 출력하도록 하는 기능
    - 선택 결과가 이후에 영향을 끼치도록 하는 기능
2. Scene 전체 관리 GameManager 생성
    - 씬 처음 불러왔을때 기본 설정 값 저장
3. UI 전체 관리 UIManager 생성
    - 특정 상황마다 UIManager를 통해 UI들을 키고끄기 하기
    - GameOverScript에서 직접 비활성화 되는 로직을 수정해서 UIManager에서 수정하도록 하기
4. 발판 랜덤 생성
    - 시드에 맞춰 랜덤 생성을 할 수 잇도록
    - 그럼 발판의 패턴같은 것은 어떻게 구현?
5. ItemInteraction에서 player의 아이템 보유 여부 관리 변경
    - 현재 player의 아이템 보유 여부를 각각의 item에서 itemHave 변수로 별개로 관리중임.
    - player가 아이템을 보유한 상태에서 다른 ItemInteraction 스크립트를 보유중인 아이템과 충돌했을 경우 문제가 발생할 수 있기에 수정해야 함.
    - 아이템 보유를 띄우는 UI나 혹은 다른 상위 Manager에서 다뤄야 할 듯함.

# 존재하는  object 목록

## 주요 object

- Player
    - Main Camera
- Platform
    - Correct
    - Wrong
    - Start & Goal
- Item
- Trigger
    - Dialogue Trigger
    - Scene Switcher
- UI
    - Timer UI
    - Game Over UI
    - Dialogue UI

---

## 모든 Script

### *Dialogue*

- Dialogue
    1. 해당 스크립트에 구현된 기능
        - 입력한 문장들과 선택지들을 public 배열 변수로 저장함
        - Choice라는 클래스를 자료형으로 가지는 배열과, String 배열 존재
        - 이 스크립트를 통해서 Dialogue 에셋을 생성할 수 있음
    2. 스크립트 구조 및 흐름
        - Choice는 존재하지 않는 클래스이기 때문에 새로 정의함
    3. 외부 의존

- DialogueController
    1. 해당 스크립트에 구현된 기능
        - Dialogue 에셋으로부터 sentences와 choices를 불러옴
        - Dialogue UI에 불러온 sentences와 choices를 text로 할당함
            - sentences에 저장된 string은 dialoguePanel에 띄움
            - choices에 저장된 Choice는 choicesLayout를 참조하여 대화창 위에 Vertical 기준으로 띄움
    2. 스크립트 구조 및 흐름
        - Start() : 시작시 Dialogue UI의 대화창 비활성화 및 선택지 비활성화
        - Update() : 대화가 활성화된 상태이면, 사용자 입력에 따라 다음의 행동을 취함
            - F키를 누를 경우 ContinueDialogue() 호출
            - Esc키 (escape키)를 누를 경우 대화 및 선택 UI 강제종료 및 EndDialogue() 호출
        - ActivateDialogue() : 외부로부터 Dialogue 에셋과 콜백 함수를 파라미터로 받음
            - StartDialogue()를 호출, 즉 ActivateDialogue()가 호출되는 순간 Dialogue 출력이 시작됨
            - 전달받은 Dialogue 에셋은 스크립트내의 Dialogue 멤버에 할당됨
            - 콜백 함수는 변수에 저장되어 EndDialogue() 함수 내부에서 실행됨.
        - StartDialogue() : Dialogue UI를 활성화 시키고 Dialogue 출력 시작
            - 대화창 활성화
            - Coroutine으로 TypeSentence() 함수를 호출해서 Dialogue 출력 시작
        - ContinueDialogue() : 대사 출력시, 타이핑 즉시 완료 혹은 넘기기 기능을 위해 존재
            - 현재 대사창에 출력중인 텍스트의 길이와, 출력해야 하는 총 텍스트의 길이를 비교
                - 길이가 다르다면 텍스트 출력 Coroutine을 종료시키고 즉시  총 텍스트를 전부 대사창에 할당
                - 길이가 같다면 다음의 조건에 따라 행동
            - 현재 출력중인 대사의 번째수를 나타내는 index값을 +1 한 뒤, index와 총 출력해야하는 대사(string) 갯수를 비교
                - index값이 더 작다면 Coroutine으로 TypeSentence() 함수를 호출해서 Dialogue 출력 시작
                - 그렇지 않다면 모든 대사 출력이 끝난 것이므로, hasChoices 변수 값에 따라 ShowChoices() 함수 호출로 선택지 표시 혹은 EndDialogue() 함수 호출로 대화 종료
        - EndDialogue() : Dialogue 출력 종료시에 항상 호출됨
            - 대사창 비활성화
            - ActivateDialogue 함수로부터 콜백함수를 할당받았다면 콜백함수를 발동 ( `?.Invoke()` 활용)
        - TypeSentence() : 코루틴으로 실행되어, index에 해당하는 번째의 대사(String 하나)를 설정한 시간만큼의 텀을 두고 한 글자씩 출력함
        - ShowChoices() : 선택지를 버튼 형태로 생성
            - 선택 UI  Layout을 활성화
            - 현재 선택지 생성하기 이전에 존재했던 버튼을 삭제
            - dialogue 에셋의 choices 배열 변수내에 존재하는 선택지 갯수 만큼 버튼을 Vertical 기준으로 생성
            - 각 버튼마다 OnChiceSelected() 함수를 호출하는 OnClick 이벤트 Listener를 할당함
        - OnChoiceSelected()
            - 선택지 버튼을 누를 경우 호출되어, 할당된 씬 이동 데이터를 따라 이동
            - 선택지 Layout 비활성화 후 EndDialogue() 호출
    3. 외부 의존
        - Dialogue 에셋을 해당 controller의 ActivateDialogue() 호출시 인자로 할당해주어야 함
        - DIalogue 에셋으로부터 읽은 내용을 띄울 Dialogue UI의 오브젝트 및 TMP를 할당해주어야 함
    

### *Trigger*

- FogTrigger
    1. 해당 스크립트에 구현된 기능
        - Player가 이 Trigger Object와 충돌해 IsTrigger를 발동시키면 Fog 생성 및 SkyBox의 Exposure를 조절한 뒤에 파괴됨
    2. 스크립트 구조 및 흐름
        - OnTriggerEnter() :  Player와 충돌시 Fog를 활성화 하고 Color를 지정한 색으로 변경한 뒤, 마지막으로 코루틴으로써 FogMakingAnimation()을 호출
        - FogMakingAnimation() : 현재 fogDensity에서, 설정한 목표 fogDensity까지 선형적으로 변화시킴. SkyBox의 Exposure 값도 동일하게 변경시킴
    3. 외부 의존
        - RenderSettings 클래스의  fogDensity와 skybox 변수
- SceneSwitch
    1. 해당 스크립트에 구현된 기능
        - Trigger로써 충돌 발생시 씬을 전환하거나
        - 외부에서 스크립트로 특정 함수를 실행할 경우
        - 설정된 씬으로 전환함
    2. 스크립트 구조 및 흐름
        - OnTriggerEnter() : Player와 충돌시 씬 전환
        - ApplySceneSwitch() : 외부에서 이 함수를 호출할 경우 씬 전환 진행
    3. 외부 의존
        - 전환할 씬 이름을 이 스크립트가 할당된 오브젝트에서 직접 설정해 주어야 함
- DialogueTrigger
    1. 해당 스크립트에 구현된 기능
    2. 스크립트 구조 및 흐름
    3. 외부 의존

### *UI*

- GameOverScript
    1. 해당 스크립트에 구현된 기능
        - EndingFunc()가 외부 스크립트에 의해 호출될 경우, 다른 UI를 비활성화 시키고 GameOverUI를 활성화 시킴
    2. 스크립트 구조 및 흐름
        - Start() : GameOver 상황에서만 UI가 나오도록 하기 위해, 시작시 GameOverUI를 비활성화
        - EndingFunc() : 외부에서 GameOverUI를 띄울 수 있도록 하기 위한 함수. 실행될 경우 GameOverUI를 활성화하고 다른 UI를 비활성화 함
        - RestartGame() : 재시작 버튼에 연결됨. 현재 씬을 처음부터 다시 로드함.
        - QuitGame() : 종료 버튼에 연결됨. 호출될 시 가장 기본 Scene(=index가 0인 Scene)인 Title Scene으로 씬 전환
    3. 외부 의존
        - GameOver UI 연결
        - Restart Button, Game Over Button 연결
- Timer
    1. 해당 스크립트에 구현된 기능
        - 설정된 시간으로부터  MM : SS 형식에 맞춰 시간 카운트 다운
        - 카운트 다운이 모두 완료될 경우 GameOver UI를 띄움
    2. 스크립트 구조 및 흐름
        - Update() : 남은 시간이 0 초과이면 감소시키고 TimeCountdown() 호출함, 남은시간이 0 미만이 되면 0으로 고정시키고 GameOver 함수를 코루틴으로 실행
        - TimeCountdown() : 현재 *남은 시간* 변수에 담긴 값을 MM : SS 형식으로 나타냄
        - GameOver() : 1초 정도 대기한 뒤에 GameOverScript의 EndingFunc를 실행
    3. 외부 의존
        - 시간에 맞춰 계산된 결과를 할당할 Text UI (TMP)
        - GameOverScript → EndingFunc()
- Title
    1. 해당 스크립트에 구현된 기능
        - 타이틀 UI에서 각 버튼마다 기능을 할당하기 위한 스크립트
    2. 스크립트 구조 및 흐름
        - OnClickStart() : Start 버튼에 연결되어 Level0으로 씬을 전환시킴
        - OnClickQuit() : Quit 버튼에 연결되어 게임 종료를 수행
    3. 외부 의존

### *Item_*공통

- ItemTrigger
    1. 해당 스크립트에 구현된 기능
        - Trigger가 발동될 시 Inspector 창에서 할당한 스크립트를 실행
        - ~~혹은 기능을 활성화 했을 경우, 오브젝트 비활성화를 실행함.~~ (사실상 안쓰는 기능)
    2. 스크립트 구조 및 흐름
        - Start() : 시작시 Renderer 속성의 컴포넌트를 가져옴
        - OnTriggerEnter() : Player 태그를 가진 물체와 부딪힌 경우,
            - OnItemCollected에 무언가가 할당되어 있는 경우
                - OnItemCollected에 할당된 스크립트 실행
                - 이 ItemTrigger 스크립트가 할당된 item은 제거
                - 만약 OnItemCollected에 할당이 이루어져 있지 않을 경우 건너 뜀
            - Object 비활성화 기능이 켜져있는 경우 DisableObjectFuc() 함수를 코루틴으로써 호출
        - DisableObjectFuc() : 할당한 오브젝트를 설정한 시간만큼 비활성화 시킨 뒤에 다시 할성화 함
    3. 외부 의존
        - 인스펙터 창을 통해 `UnityEvent OnItemCollected;`에  Object와 그 Object에 컴포넌트로 존재하는 Script를 할당해주어야 함
        - Object 비활성화를 할 것이라면 비활성화할 오브젝트를 할당해야 함
- ItemInteraction
    1. 해당 스크립트에 구현된 기능
        - 해당 스크립트를 가진 Item에 Player가 충돌해 isTrigger를 발동시켰을 때 기능함
        - Item의 MeshRender와 Collider를 비활성화 하고, I키를 누르면 아이템을 사용가능하도록 만듦
        - 아이템은 Player가 한가지만 가지도록 유도해야함.
    2. 스크립트 구조 및 흐름
        - Awake() : Start()보다 우선적으로 실행. 이 스크립트를 가진 Object의 Renderer와 Collider 컴포넌트를 가져옴.
            - 이는 추후 코루틴이 실행되는 동안 Destroy 혹은 disable 하는 대신에,
            아이템의 Renderer와 Collider를 비활성화 시키는 방식으로 Scene에 존재시기키 위함.
        - Update() : Player가 아이템을 획득한 상태이고, I키를 눌렀을 경우 UseItem() 함수를 호출
        - OnTriggerEnter() : Player 오브젝트와 충돌했고, 현재  Player가 Item을 얻지 못한 상태라면 CollectItem()을 호출
        - CollectItem() : Player가 아이템을 보유중인 상태로 변경하고 UI의 인벤토리에 아이템 로고를 띄움. 그리고 아이템의 Renderer와 Collider를 비활성화
            - Destroy()나 Disable() 대신에 굳이 Renderer와 Collider를 비활성화하는 이유는 다음과 같다.
            - 현재 이 스크립트가 존재하는 아이템에 아이템의 기능을 수행하는 Script가 공존하기 때문에 이 아이템을 비활성화 시키면 아이템의 기능이 수행되지 않는다
            - 따라서 아이템을 획득해서 사라진 것처럼 보이도록, 그리고 아이템을 획득하는 Trigger가 발동되지 않도록 Renderer와 Collider를 비활성화하는 것
        - UseItem() : Player가 아이템을 사용할 경우 실행되는 함수
            - UI의 인벤토리에 띄워진 아이템 로고를 제거
            - 아이템 미획득 상태로 설정
            - Inspector 창에서 설정한 아이템 획득시 실행할 스크립트를 실행
    3. 외부 의존
        - Item획득시 띄울 Image 파일
        - ItemImage를 띄울 UI 연결
        - 아이템 사용시 실행할 외부 함수
- Portal
    1. 해당 스크립트에 구현된 기능
        - ApplyPortal()이 호출될 경우, 설정한 오브젝트의 Position으로 Player의 Position을 갱신함
    2. 스크립트 구조 및 흐름
        - ApplyPortal() : Player의 Character Controller 컴포넌트를 비활성화 한 뒤에 Position을 변경시키고 다시 활성화함.
            - Character Controller 컴포넌트는 비활성화 해야만 Position 변경사항이 반영됨
            - 비활성화 하지 않을 경우 SimpleMove.cs에서의 controller.Move()로 인해 제대로 변경사항이 반영되지 않음
    3. 외부 의존
        - Player → Character Controller 컴포넌트
        - 포탈 순간이동 목적지에 해당하는 오브젝트

### *Item_*Level1

- DogMoveScript
    1. 해당 스크립트에 구현된 기능
        - 아이템으로부터 ApplyDog() 함수가 호출될 경우 Dog 오브젝트를 활성화하고 Script에 따라 행동하도록 함
        - Correct 발판의 이름이 Correct## 형태임을 이용, 오브젝트의 이름을 통해 해당 오브젝트를 찾아내어 다음 이동 목적지로 설정함
        - Dog 오브젝트는 isTrigger가 활성화된 오브젝트로, Player와 충돌할 경우 다음 목적지의 위치로 Dog 오브젝트를 이동시킴
        - 이때, 이동하는 방식은 현재 위치에서 포물선을 그리며 다음 위치로 이동
        - 지금 위치한 발판의 이름은 RayCast를 이용하여 알아내며, 만약 마지막 Correct 발판일 경우 Player와 충돌하면 비활성화 되고 코루틴 종료
    2. 스크립트 구조 및 흐름
        - **`Awake()`**
            - 게임 시작 시 Dog 오브젝트가 비활성화 상태여도 `Awake`는 호출될 수 있으므로, 여기서 `Correct01` 발판의 초기 위치를 미리 찾아둠
            - Dog 오브젝트에 필요한 `Collider`와 `Rigidbody`가 없거나, `Collider`의 `Is Trigger`가 활성화되지 않았을 경우를 대비하여 경고 메시지
        - 내용이 길어져서 Toggle로 대체
            - **`DogMove()` (코루틴)**:
                - **발판 감지 (`Physics.Raycast`)**: Dog 오브젝트의 발 아래에서 Raycast를 쏴서 현재 밟고 있는 발판(`currentPlatform`)을 식별합니다. Raycast 시작 지점을 `transform.position + Vector3.up * 0.1f`로 하여 Dog의 몸통 안에서 시작하지 않고 발 밑에서 시작하도록 합니다. `groundCheckDistance`와 `groundLayer`를 사용하여 특정 레이어의 발판만 감지합니다.
                - **Correct17 처리**: `currentPlatform.name == "Correct17"`인 경우 Dog 오브젝트를 비활성화하고 코루틴을 종료합니다.
                - **다음 발판 계산**: 현재 발판 이름에서 숫자를 추출하고 1을 더해 다음 발판의 이름을 생성합니다 (`Correct{숫자:D2}` 형식으로 두 자리 숫자를 유지).
                - **포물선 이동**:
                    - `Vector3.Lerp`를 사용하여 수평 이동을 처리합니다.
                    - `Mathf.Sin(progress * Mathf.PI) * jumpHeight`를 사용하여 시간의 진행(`progress`)에 따라 포물선 형태의 높이 변화를 계산합니다. `Mathf.PI`는 180도를 의미하므로, `sin` 함수는 0에서 1까지의 진행 동안 0 -> 1(최고점) -> 0(도착점)으로 변화하는 곡선을 만들어 포물선을 구현합니다.
                - `yield return null;`: 매 프레임마다 이동을 업데이트하고 다음 프레임까지 기다립니다.
                - 이동이 완료되면 `transform.position`을 목표 위치에 정확히 맞추고 `isDogMoving` 플래그를 해제합니다.
            - **`ApplyDog()`**:
                - Dog 오브젝트를 활성화하고 `Correct01` 발판의 정 중앙 상단에 위치시킵니다. `GetComponent<Collider>().bounds.extents.y`를 사용하여 발판의 높이를 고려해 표면 위에 정확히 배치합니다.
            - **`OnTriggerEnter(Collider other)`**
                - 이제 Dog 오브젝트의 `Collider`(`Is Trigger` 활성화) 영역 안으로 **다른 `Collider`가 들어올 때** 이 함수가 자동으로 호출됩니다.
                - `other` 매개변수는 Dog의 Trigger 영역에 들어온 **다른 Collider 컴포넌트**를 참조합니다.
                - `if (other.gameObject == playerObject)`: 들어온 Collider가 `playerObject`의 것인지 확인합니다.
                - `if (!isDogMoving)`: Dog가 이미 이동 중이 아니라면, `DogMove()` 코루틴을 시작합니다.
            - **헬퍼 함수 (`ExtractNumberFromName`, `FindPlatformByName`)**:
                - `Regex.Match`: 발판 이름에서 숫자를 깔끔하게 추출하기 위해 정규식을 사용합니다.
                - `platformParent.transform.Find(platformName)`: 부모 오브젝트의 자식 중에서 이름으로 발판을 찾아옵니다. 이는 `GameObject.Find`보다 효율적이며, 씬에 동명의 다른 오브젝트가 있을 때의 문제를 방지합니다.
            - 필요 설정 사항들
                1. **Correct## 발판 설정:**
                    - 발판들은 Dog가 밟고 이동하는 대상이므로, `Collider` 컴포넌트가 있어야 합니다 (일반 Collider, `Is Trigger` **해제**).
                    - 발판들이 속한 **레이어(Layer)**를 `DogMoveScript`의 `Ground Layer` 필드에 정확히 설정해야 합니다. (이전과 동일)
                2. **`DogMoveScript` 인스펙터 설정:**
                    - `Platform Parent`: Correct## 발판들의 부모 오브젝트 (예: "Platform"이라는 빈 GameObject)를 드래그앤드롭.
                    - `Player Object`: Player GameObject를 드래그앤드롭.
                    - `Jump Height`, `Jump Duration`: 원하는 대로 조절.
                    - `Ground Layer`: 발판들이 속한 레이어를 선택.
    3. 외부 의존
        - Correct 발판을 자식 오브젝트로 보유하는 Platform
        - RayCast로 발판 감지를 하므로 발판이 속한 layer
        
- FogRemover
    1. 해당 스크립트에 구현된 기능
        - ApplyFogFunc()이 호출될 경우 안개제거 기능을 수행
    2. 스크립트 구조 및 흐름
        - Start() : 이 스크립트를 보유중인 오브젝트의 Renderer를 가져옴
        - ApplyFogFunc() : 외부에서 호출될 경우 코루틴으로써 DisableFogFunc()함수를 실행
        - DisableFogFunc()
            - 지금의 Fog Color와 Density를 저장
            - Fog 기능을 비활성화한뒤 설정한 시간만큼 대기
            - Fog 기능을 활성화 한 뒤, 비활성화 하기 이전의 Fog Color와 Density를 할당시킴
            - 오브젝트 Destroy
    3. 외부 의존
        - RenderSettings → fog, fogColor, fogDensity
        - 비활성화 시간 Inspector 창에서 지정

### *Player*

- PlayerReset
    1. 해당 스크립트에 구현된 기능들
        - (기본) 유저가 일정 높이 이하로 떨어질 경우 startPos 변수에 저장된 위치로 유저 위치 초기화
        - (옵션1) Correct 발판을 밟을 때마다 startPos 해당 위치로 업데이트
        - (옵션2) 밟아온 Correct 발판에 대해  설정한 material 로 업데이트
    2. 스크립트 구조 및 흐름
        - Start() : 시작시 현재 좌표를 저장
        - Upadate() : 매 프레임마다 현재의 Y축 좌표를 체크하여, 기준 좌표보다 낮아질 경우 ResetToStart() 함수를 실행
        - ResetToStart() : startPos 변수에 담긴 좌표로  현재 Player 오브젝트의 위치를 변경, SimpleMove 함수로부터 Y축 변화량과 현재 바닥여부 저장 변수를 가져와 초기화함
        - OnControllerColliderHit() : `List<GameObject>`를 활용, Player 오브젝트가 어떠한 오브젝트와 충돌할 경우 아래의 조건에 따라 특정 함수 실행
            1. 그 오브젝트가 Inspector 창에 입력한 태그명의 오브젝트인지 확인
            2. List에 추가되어 있는 object인지 아닌지 확인
            3. List에 없는 object인 경우에만 List에 추가하고 옵션에 따라 특정 기능을 실행
                - 옵션 기능은 사용자가 체크를 통해 결정 가능
                - 옵션 1 : SavePointUpdate
                    - 그 발판의 좌표를 startPost 변수에 저장함 → ResetToStart()함수에 의해 초기화 되는 위치가 변경됨
                - 옵션2 : PastPathColorizer
                    - 충돌한 오브젝트의 Renderer 컴포넌트를 파라미터로 받아와 Unhappy case를 통과하면 material을 수정함
    3. 외부 의존 
        - SimpleMove.cs → yVelocity 값
        - objTag : 세이브 포인트 기능이 적용될 발판 태그 명 저장
- SimpleMove
    1. 해당 스크립트에 구현된 기능
        - 카메라 방향을 기준으로, wasd + space키에 맞춰 player 이동
        - Wrong 발판을 밟으면 해당 오브젝트 파괴 및 점프 불가
        - 점프시 다시 발판위에 올라오기 전까지 점프 불가
        - 마지막으로 밟은 발판이 Wrong 태그가 아닌 발판이고, 점프를 안한채로 낙하하고 있으면 공중에서 점프 가능
    2. 스크립트 구조 및 흐름
        - 변수
            - WrongPanel : 잘못된 발판을 밟았을 경우 yVelocity가 0으로 변경되지 않고, 점프도 못하는 상태로 변경하여 그대로 하락하도록 만들기 위한 변수
            - IsGrounded : 현재 지면위에 있어 **점프가 가능함**을 결정함
        - Start() : 시작시 Player 오브젝트의 CharacterController 컴포넌트를 가져옴
        - Update() : 매 프레임마다 입력키에 맞춰 player의 위치를 변화시킴
            - `controller.collisionFlags == CollisionFlags.Below`가 true일 때, 즉 character controller가 바닥에 닿아있을 때,
                - wrongPanel 변수가 false면 isGrounded 변수를 true로 설정하고 yVelocity를 0으로 설정.
                - wrongPanel 변수가 true면 isGrounded 변수를 false로 설정.
            - wrongPanel 변수가 false이고, isGrounded 변수가 true고, 점프키를 눌렀을 때
            yVelocity값을 JumpPower만큼 갱신하고 isGround를 false로 변경
        - OnControllerColliderHit() :
            - Player 오브젝트가 Wrong 태그의 오브젝트와 충돌했을 경우, 그 오브젝트가 Wrong 태그명을 가지고 있다면 해당 오브젝트를 파괴
            - Wrong 태그 오브젝트와 충돌한 게 아닌데 WrongPanel이 활성화 되어있는 경우, WrongPanel을 false로 변경
    3. 외부 의존
        - CamRot.cs에서 결정된 Camera direction Vector3 값을 사용함

### *Cam*

- CamRot
    1. 해당 스크립트에 구현된 기능
    2. 스크립트 구조 및 흐름
    3. 외부 의존

### *Playform*

- PanelHiding
    - 외부에서 ApplyPanelHide()함수를 호출할 경우 패널을 숨기는 코드가 작동
    - 현재 Platform 오브젝트의 자식으로 있는 오브젝트들 중에서, Wrong 태그가 적용된 오브젝트들을 foreach로 각각 방문
    - 방문한 오브젝트에 대해 비동기적으로 Coroutine을 실행시키고 다음 Wrong 태그 오브젝트로 이동
    - Coroutine은 HideAndShowPanel 함수를 이용해 진행
- TagColorizer1
    - Item에서 onTriggerEnter 함수를 통해 ColorPanels 함수를 호출할 경우, 아무것도 할당되어 있지 않은 Coroutine 변수에 StartCoroutine 함수의 결과를 할당하는 것으로써 비동기적으로 HighlightRoutine 함수를 실행
    - 현재 Platform 오브젝트의 자식으로 잇는 오브젝트들을 전부 순회하며 Wrong 태그가 적용되어 있는 오브젝트와 Correct 태그가 적용되어 있는 오브젝트에 대해 서로 다른 Material을 할당함

---

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