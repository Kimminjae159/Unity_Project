using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 게임의 전반적인 상태와 데이터를 관리하며, 씬 전환 시에도 파괴되지 않습니다.
/// 싱글톤 패턴을 사용하여 어디서든 하나의 인스턴스에만 접근할 수 있도록 합니다.
/// </summary>
public class GameManager : MonoBehaviour
{
    // --- 싱글톤 인스턴스 ---
    public static GameManager instance;

    // --- 게임 데이터 (씬 매니저가 참조) ---
    [Header("Game Core Data")]
    public int sympathyValue = 0;   // 공감 수치 
    public int health = 5;          // 체력
    public int restartCount = 0;    // 재시작 횟수
    public float skyboxExposure = 1.3f; // 스카이박스 노출값
    public float timeLimit = 600f;  // 시간 제한 (초 단위)
    public int score = 0;           // 점수 (필요한지 여부를 따져봐야 할듯함)
    public bool isRestart = false;  // 현재 씬이 재시작된 것인지 여부

    // <summary>
    // 싱글톤 패턴 구현
    // </summary>
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            DestroyImmediate(gameObject);
        }
    }

    /// <summary>
    /// 호출 즉시 체력을 1 감소시킴과 동시에 체력 현황 반환
    /// 체력이 0 이하(=종료)로 떨어지면 true, 0 초과(=생존)이면 false
    /// </summary>
    public bool DecreaseHealth()
    {
        health--;
        return health <= 0;
    }

    /// <summary>
    /// 새로운 씬을 로드하기 전에 호출하여 데이터를 준비하는 용도
    /// (예: UI 버튼의 OnClick 이벤트에서 호출)
    /// </summary>
    /// <param name="isThisARestart">재시작하는 경우 true, 다음 레벨로 가는 경우 false</param>
    public void PrepareForNewScene(bool isThisARestart)
    {
        isRestart = isThisARestart;
        if (isThisARestart)
        {
            restartCount++;
        }
        // 다음 씬으로 넘어갈 때 점수나 기타 데이터를 초기화하거나 변경하는 로직 추가 가능
    }

    /// <summary>
    /// UI_GameOver의 'Restart' 버튼에 대응
    /// 게임을 재시작 : "현재 씬"을 다시 로드합니다.
    /// </summary>
    public void RestartLevel()
    {
        PrepareForNewScene(true);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    } 

    /// <summary>
    /// UI_GameOver의 'Title로 이동' 버튼 대응
    /// 타이틀로 이동합니다.
    /// </summary>
    public void GoToTitle()
    {
        Debug.Log("Go too Title...");
        SceneManager.LoadScene(0);
    }
}
