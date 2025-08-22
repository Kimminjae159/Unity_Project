using UnityEngine;

public class PlayerUIManager : MonoBehaviour
{
    [Header("PlayerUI 오브젝트를 할당")]
    public GameObject playerUI;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (!playerUI)
        {
            Debug.LogError("PlayerUIManager 스크립트에 Player UI가 할당되지 않음.");
            return;
        }
        Hide();
    }

    // PlayerUI의 세부 구현을 담당하는 Script는 OnEnable로 활성화 시의 실행할 것들이 결정됨
    public void Show() { playerUI.SetActive(true); }
    public void Hide() { playerUI.SetActive(false); }
}
