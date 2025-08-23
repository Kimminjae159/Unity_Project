using UnityEngine;

public class PuzzleHintContrloller : MonoBehaviour
{
    public GameObject PuzzleHintUI;

    void Start()
    {
        PuzzleHintUI.SetActive(false); // UI 비활성화
    }

    public void openHintUI()
    {
        PuzzleHintUI.SetActive(true);
    }

    // UI의 X 버튼과 연결
    public void closeHintUI()
    {
        PuzzleHintUI.SetActive(false);
        ItemManager.instance.ItemIsActiveOn();
    }
}
