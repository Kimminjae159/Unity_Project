using UnityEngine;
using System;
using UnityEngine.Events;
using UnityEngine.Rendering; // Action 콜백을 사용하기 위함

/// <summary>
/// 각 레벨 씬의 전반적인 흐름을 관리합니다.
/// GameManager로부터 데이터를 받아 씬을 구성하고, 플레이어의 행동에 따른 이벤트를 처리합니다.
/// </summary>
public class StageManager : MonoBehaviour
{
    [Header("UI Manager")]
    [Tooltip("PlayerUIManager 스크립트가 컴포넌트로 있는 오브젝트를 할당")]
    [SerializeField] private PlayerUIManager playerUIManager;   // 플레이어 UI Manager
    [Tooltip("GameOver 스크립트가 컴포넌트로 있는 오브젝트를 할당")]
    [SerializeField] private GameOver gameOverManager;          // 게임오버 Manager
    [SerializeField] private EventDialgoueMng eventUI;          // 게임오버 Manager
    // [SerializeField] private GameClearUI gameClearUI;      // 게임클리어 UI (필요 시)
    // [SerializeField] private SympathyUI sympathyUI;        // 공감수치(체력) UI

    [Header("Scene Settings")]
    [SerializeField] private UnityEvent levelClearScript;  // 레벨클리어시 행동할 Script

    [Header("Dialogue Assets")]
    [SerializeField] private DialogueAsset startDialogue;       // 시작 대화
    [SerializeField] private DialogueAsset gameOverDialogue;    // 게임오버 대화
    [SerializeField] private DialogueAsset levelClearDialogue;  // 레벨클리어 대화

    // --- 내부 변수 ---
    public static Action<int> callUpdateHP; // 옵저버 패턴을 활용, HP가 변경되면 이벤트에 등록된 함수를 호출
    public static Action<bool> callTimer; 
    private bool isGameOver = false;     // 게임오버 상태 플래그 (중복 호출 방지)

    /// <summary>
    /// 씬이 로드될 때 호출됩니다.
    /// 싱글톤 패턴을 적용하여 어디에서든 StageManager를 호출할 수 있도록 합니다.
    /// </summary>
    public static StageManager instance;
    [Header("테스트 중이라면 false로 설정")]
    public bool MemberCheckMode = true;
    private bool AllMemberOK = true;
    void Awake()
    {
        // GameManager 인스턴스가 없거나 Inspector 창에서 할당이 안이루어져 있으면 오류 출력 후 종료
        if (GameManager.instance == null)
        {
            Debug.LogError("GameManager instance not found!");
            AllMemberOK = false;
        }
        if (!playerUIManager)
        {
            Debug.LogError("StageManager 스크립트에 PlayerUIManager 스크립트가 할당되지 않음. \n PlayerUIManager 스크립트가 Component로 존재하는 Object를 할당할 것");
            AllMemberOK = false;
        }
        if (!gameOverManager)
        {
            Debug.LogError("StageManager 스크립트에 GameOver 스크립트가 할당되지 않음. \n GameOver 스크립트가 Component로 존재하는 Object를 할당할 것");
            AllMemberOK = false;
        }
        if (MemberCheckMode && !AllMemberOK) return;

        
        // 싱글턴으로 유일 인스턴스 보장
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            DestroyImmediate(this);
        }
        // GameManager로부터 데이터를 받아 씬 초기 구성

        // 씬의 Skybox 노출값 설정
        RenderSettings.skybox.SetFloat("_Exposure", GameManager.instance.skyboxExposure);
    }

    // 활성화시 Player 낙하 신호를 구독, 비활성화시 구독해제
    void OnEnable()
    {
        PlayerReset.CallPlayerReset += OnPlayerFall;
    }
    void OnDisable()
    {
        PlayerReset.CallPlayerReset -= OnPlayerFall;
    }

    #region --- Public Methods (Called by Triggers) ---

    /// <summary>
    /// 플레이어가 '시작' 트리거에 닿았을 때 호출됩니다.
    /// </summary>
    public void PlayerLevelEnter()
    {
        // 재시작 여부에 따라 분기
        if (GameManager.instance.isRestart)
        {
            // 재시작 시에는 대화 없이 바로 게임 시작
            StartGame();
        }
        else
        {
            // 첫 시작 시에는 시작 대화를 출력하고, 대화가 끝나면 StartGame()을 콜백으로 실행
            DialogueManager.instance.StartDialogue(startDialogue, StartGame);
        }
    }

    /// <summary>
    /// 플레이어가 '이벤트' 트리거에 닿았을 때 호출됩니다.
    /// </summary>
    /// <param name="eventDialogue">해당 이벤트에서 출력할 대화</param>
    public void PlayerEvent(DialogueAsset eventDialogue)
    {
        // 이벤트 대화 출력
        eventUI.EventDialogueStart(eventDialogue);
        // 필요 시 특정 UI 활성화 등의 로직 추가
    }

    /// <summary>
    /// 플레이어 낙사시 호출될 함수
    /// </summary>
    public void OnPlayerFall()
    {
        OnPlayerDamaged();
    }

    /// <summary>
    /// 플레이어 HP가 감소하는 상황일 때 호출
    /// </summary>
    public void OnPlayerDamaged()
    {
        if (isGameOver) return; // 이미 게임오버 상태면 무시

        // GameManager의 체력을 감소시키고, 0이하가 되었는지 확인
        bool isOutOfSympathy = GameManager.instance.DecreaseHealth();

        // 체력 UI 업데이트
        callUpdateHP?.Invoke(GameManager.instance.healthPoint);

        if (isOutOfSympathy)
        {
            TriggerPlayerGameOver("체력 0이 됨");
        }
    }

    /// <summary>
    /// 플레이어가 '레벨 클리어' 트리거에 닿았을 때 호출됩니다.
    /// </summary>
    public void PlayerLevelClear()
    {
        Debug.Log($"레벨 클리어, isGameOver : {isGameOver}");
        if (isGameOver) return;
        isGameOver = true; // 클리어도 게임 종료 상태로 간주

        if(playerUIManager) playerUIManager.Hide();
        // gameClearUI.SetActive(true);

        // 여기에 다음 레벨로 넘어가는 로직 추가
        //callTimer?.Invoke(false);
        DialogueManager.instance.StartDialogue(levelClearDialogue, ClearCallback);
    }

    /// <summary>
    /// 게임오버 상황을 최종적으로 처리하는 함수.
    /// </summary>
    /// <param name="reason">게임오버 사유</param>
    public void TriggerPlayerGameOver(string reason)
    {
        // 게임오버 중복 처리 방지
        if (isGameOver) return;
        isGameOver = true;

        Debug.Log($"게임 오버! 사유: {reason}");

        // Player UI 비활성화, 
        playerUIManager.Hide();

        // 게임오버 처리를 하도록 dialogue를 전달
        gameOverManager.OnGameOver(gameOverDialogue);
    }

    #endregion

    #region --- Private Logic & Callbacks ---

    /// <summary>
    /// 실제 게임 플레이를 시작하는 로직.
    /// (시작 대화가 끝난 후 또는 재시작 시 즉시 호출됨)
    /// </summary>
    private void StartGame()
    {
        // 타이머 UI 활성화
        playerUIManager.Show();
        callTimer?.Invoke(true);
    }
    private void ClearCallback()
    {

        levelClearScript.Invoke();
    }
    

    #endregion
}

/* --- 필요한 다른 스크립트의 예시 인터페이스 ---

public class DialogueManager : MonoBehaviour
{
    // DialogueAsset: 대화 내용(대사, 화자 등)을 담는 ScriptableObject 또는 클래스
    // onDialogueEnd: 대화가 모두 끝났을 때 호출될 콜백 함수 (Action)
    public void StartDialogue(DialogueAsset dialogue, Action onDialogueEnd = null) { ... }
}

public class TimerUI : MonoBehaviour
{
    public void SetTime(float time) { ... }
    public void UpdateTime(float time) { ... }
}

public class GameOverUI : MonoBehaviour
{
    public void ActivateButtons() { ... }
}

*/
