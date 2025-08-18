using UnityEngine;
using System;
using UnityEngine.Events; // Action 콜백을 사용하기 위함

/// <summary>
/// 각 레벨 씬의 전반적인 흐름을 관리합니다.
/// GameManager로부터 데이터를 받아 씬을 구성하고, 플레이어의 행동에 따른 이벤트를 처리합니다.
/// </summary>
public class StageManager : MonoBehaviour
{
    [Header("Component References")]
    [SerializeField] private DialogueManager dialogueManager; // 대화 관리자
    [SerializeField] private Timer timerUI;                 // 타이머 UI
    [SerializeField] private GameOver gameOverUI;           // 게임오버 UI
    // [SerializeField] private GameClearUI gameClearUI;      // 게임클리어 UI (필요 시)
    // [SerializeField] private SympathyUI sympathyUI;        // 공감수치(체력) UI

    [Header("Scene Settings")]
    [SerializeField] private float baseTimeLimit = 120f; // 이 레벨의 기본 제한 시간
    [SerializeField] private float timeReductionPerRestart = 10f; // 재시작 시마다 감소할 시간
    [SerializeField] private UnityEvent levelClearScript;  // 레벨클리어시 행동할 Script

    [Header("Dialogue Assets")]
    [SerializeField] private DialogueAsset startDialogue;       // 시작 대화
    [SerializeField] private DialogueAsset gameOverDialogue;    // 게임오버 대화
    [SerializeField] private DialogueAsset levelClearDialogue;  // 레벨클리어 대화

    // --- 내부 변수 ---
    private float currentLevelTimeLimit; // 현재 레벨의 실제 제한 시간
    private bool isGameOver = false;     // 게임오버 상태 플래그 (중복 호출 방지)

    /// <summary>
    /// 씬이 로드될 때 호출됩니다.
    /// </summary>
    void Awake()
    {
        // GameManager 인스턴스가 없다면 오류 출력 후 종료
        if (GameManager.instance == null)
        {
            Debug.LogError("GameManager instance not found!");
            return;
        }

        // GameManager로부터 데이터를 받아 씬 초기 구성
        // 1. 난이도 조절: 재시작 횟수에 따라 제한 시간 조절
        currentLevelTimeLimit = baseTimeLimit - (GameManager.instance.restartCount * timeReductionPerRestart);
        if (currentLevelTimeLimit < 30f) currentLevelTimeLimit = 30f; // 최소 시간 보장

        // 2. 체력 UI 설정
        // sympathyUI.SetSympathy(GameManager.instance.sympathyValue);

        // 3. 모든 주요 UI 비활성화
        if (timerUI) timerUI.gameObject.SetActive(false);
        if (gameOverUI) gameOverUI.gameObject.SetActive(false);
        // if(gameClearUI) gameClearUI.gameObject.SetActive(false);
    }

    /// <summary>
    /// 매 프레임 호출됩니다. 타이머를 처리합니다.
    /// </summary>
    void Update()
    {
        // 게임오버 상태이거나, 타이머가 비활성화 상태이면 로직을 실행하지 않음
        if (isGameOver || !timerUI.gameObject.activeSelf)
        {
            return;
        }

        // UI 업데이트
        //timerUI.UpdateTime(currentLevelTimeLimit);

        // 시간 초과 시 게임오버 처리
        // if (currentLevelTimeLimit <= 0)
        // {
        //     HandlePlayerGameOver("시간 초과");
        // }
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
            dialogueManager.StartDialogue(startDialogue, StartGame);
        }
    }

    /// <summary>
    /// 플레이어가 '이벤트' 트리거에 닿았을 때 호출됩니다.
    /// </summary>
    /// <param name="eventDialogue">해당 이벤트에서 출력할 대화</param>
    public void PlayerEvent(DialogueAsset eventDialogue)
    {
        // 이벤트 대화 출력
        dialogueManager.StartDialogue(eventDialogue);
        // 필요 시 특정 UI 활성화 등의 로직 추가
    }

    /// <summary>
    /// 플레이어가 '낙사' 등 데미지를 입는 상황일 때 호출됩니다.
    /// </summary>
    public void OnPlayerDamaged()
    {
        if (isGameOver) return; // 이미 게임오버 상태면 무시

        // GameManager의 체력을 감소시키고, 0이하가 되었는지 확인
        bool isOutOfSympathy = GameManager.instance.DecreaseHealth();

        // 체력 UI 업데이트
        // sympathyUI.SetSympathy(GameManager.instance.sympathyValue);

        if (isOutOfSympathy)
        {
            HandlePlayerGameOver("공감 수치 소진");
        }
    }

    /// <summary>
    /// 플레이어가 '레벨 클리어' 트리거에 닿았을 때 호출됩니다.
    /// </summary>
    public void PlayerLevelClear()
    {
        if (isGameOver) return;
        isGameOver = true; // 클리어도 게임 종료 상태로 간주

        timerUI.gameObject.SetActive(false);
        // gameClearUI.SetActive(true);
        dialogueManager.StartDialogue(levelClearDialogue, ClearCallback);
        // 여기에 다음 레벨로 넘어가는 로직 추가 (예: 5초 후 씬 전환)
    }

    #endregion

    #region --- Private Logic & Callbacks ---

    /// <summary>
    /// 실제 게임 플레이를 시작하는 로직.
    /// (시작 대화가 끝난 후 또는 재시작 시 즉시 호출됨)
    /// </summary>
    private void ClearCallback()
    {
        levelClearScript.Invoke();
    }
    private void StartGame()
    {
        // 씬의 Skybox 노출값 설정
        RenderSettings.skybox.SetFloat("_Exposure", GameManager.instance.skyboxExposure);

        // 타이머 UI의 script를 호출에서 그쪽에서 자체적으로 세팅하고 시작하도록 하기

        // 타이머 UI 활성화 및 시간 세팅
        timerUI.remainingTime = currentLevelTimeLimit;
        timerUI.gameObject.SetActive(true);
    }

    /// <summary>
    /// 게임오버 상황을 최종적으로 처리하는 함수.
    /// </summary>
    /// <param name="reason">게임오버 사유</param>
    private void HandlePlayerGameOver(string reason)
    {
        // 게임오버 중복 처리 방지
        if (isGameOver) return;
        isGameOver = true;

        Debug.Log($"게임 오버! 사유: {reason}");

        // 타이머 UI 비활성화, 게임오버 UI 활성화
        timerUI.gameObject.SetActive(false);
        gameOverUI.gameObject.SetActive(true);

        // 게임오버 대화를 출력하고, 대화가 끝나면 버튼을 활성화하는 OnGameOverDialogueEnd를 콜백으로 전달
        dialogueManager.StartDialogue(gameOverDialogue, OnGameOverDialogueEnd);
    }

    /// <summary>
    /// 게임오버 대화가 모두 출력된 후 호출될 콜백 함수입니다.
    /// </summary>
    private void OnGameOverDialogueEnd()
    {
        // GameOver UI의 재시작/종료 버튼 활성화
        gameOverUI.EndingFunc();
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
