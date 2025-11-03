using UnityEngine;

public class HintLangChange : MonoBehaviour
{
    public GameObject hint_kr;
    public GameObject hint_en;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        if(GameManager.instance.language == 0)
        {
            hint_kr.SetActive(true);
            hint_en.SetActive(false);
        }
        else if (GameManager.instance.language == 1)
        {
            hint_kr.SetActive(false);
            hint_en.SetActive(true);
        }



    }
}
