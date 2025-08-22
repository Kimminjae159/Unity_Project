using UnityEngine;
using System.Collections.Generic;

public class HP : MonoBehaviour
{
    [Header("HP UI")]
    [Tooltip("복제해서 생성할 HP 이미지의 원본 프리팹을 할당 (Prefab -> UI -> UI Object 폴더 내의 HP_UI_Prefab)")]
    public GameObject hpIconPrefab; // 1. HP 아이콘으로 사용할 이미지 프리팹

    [Tooltip("생성된 HP 아이콘들이 위치할 부모 오브젝트 (Horizontal Layout Group이 있는 곳) \n Player UI -> HP Container를 할당 ")]
    public Transform iconContainer; // 2. HP 아이콘들이 생성될 부모 Transform (HP_Container)

    private int maxHp = GameManager.instance.healthPoint; // 3. 최대 체력 (GameManager에서 결정)


 
    // 생성된 HP 아이콘 오브젝트들을 순서대로 저장
    private List<GameObject> hpIcons = new List<GameObject>();


    /// <summary>
    /// 게임 시작시 UI 초기화
    /// </summary>
    void Start()
    {
        InitializeHp();
    }

    // stagemanager의 HP 변경 신호를 구독
    void OnEnable()
    {
        StageManager.callUpdateHP += UpdateHp;        
    }
    // 메모리 누수를 막기 위해 stagemanager 구독을 해제
    void OnDisable()
    {
        StageManager.callUpdateHP -= UpdateHp;
    }

    /// <summary>
    /// 설정된 최대 체력(maxHp)만큼 HP 이미지를 UI에 생성 및 관리 리스트에 저장
    /// </summary>
    private void InitializeHp()
    {
        // 혹시 이전에 생성된 아이콘이 있다면 모두 삭제 (초기화)
        foreach (Transform child in iconContainer)
        {
            Destroy(child.gameObject);
        }
        hpIcons.Clear();

        // 최대 체력만큼 반복하여 아이콘 생성
        for (int i = 0; i < maxHp; i++)
        {
            // hpIconPrefab을 iconContainer의 자식으로 생성
            GameObject newIcon = Instantiate(hpIconPrefab, iconContainer);
            hpIcons.Add(newIcon); // 생성된 아이콘을 리스트에 순서대로 추가
        }
    }

    /// <summary>
    /// 외부(SystemManager 등)에서 HP 갯수를 인자로 하여 호출시, HP UI를 현재 체력에 맞게 업데이트 함
    /// </summary>
    /// <param name="currentHp">표시할 현재 체력 값</param>
    public void UpdateHp(int currentHp)
    {
        // currentHp가 0보다 작거나 maxHp보다 큰 비정상적인 값일 경우를 대비
        currentHp = Mathf.Clamp(currentHp, 0, maxHp);

        // 모든 HP 아이콘을 순회하면서 상태(true or false)를 결정
        for (int i = 0; i < hpIcons.Count; i++)
        {
            // 아이콘의 순번(i)이 현재 체력(currentHp)보다 작으면 활성화, 그렇지 않으면 비활성화
            // 예: currentHp가 3이면, 0, 1, 2번 아이콘은 켜지고 3, 4번은 꺼짐
            hpIcons[i].SetActive(i < currentHp);
        }
    }
}
