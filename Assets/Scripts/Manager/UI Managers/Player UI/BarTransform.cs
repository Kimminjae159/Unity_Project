using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Unity.VisualScripting;

public class BarTransform : MonoBehaviour
{
    public RectTransform targetRectTransform;
    public TMP_Text gage;
    float myValue;

    void Start()
    {
        if (targetRectTransform != null)
        {
            myValue = 16f - (float)GameManager.instance.sympathyValue;

            // 1. 현재 localScale 값을 가져옵니다.
            Vector3 currentScale = targetRectTransform.localScale;

            // 2. Y값만 새로운 값으로 변경합니다.
            currentScale.y = myValue;

            // 3. 변경된 Vector3 값을 다시 localScale에 할당합니다.
            targetRectTransform.localScale = currentScale;
        }

        if(GameManager.instance.language == 0) { gage.text = "공감 게이지"; }
        else if (GameManager.instance.language == 1) { gage.text = "Empathy LV"; }
    }

}
