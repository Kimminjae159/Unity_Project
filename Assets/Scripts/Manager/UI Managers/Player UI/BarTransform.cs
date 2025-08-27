using UnityEngine;
using UnityEngine.SceneManagement;

public class BarTransform : MonoBehaviour
{
    public RectTransform targetRectTransform;
    float myValue;

    void Start()
    {
        myValue = 16f - (float)GameManager.instance.sympathyValue;

        // 1. 현재 localScale 값을 가져옵니다.
        Vector3 currentScale = targetRectTransform.localScale;

        // 2. Y값만 새로운 값으로 변경합니다.
        currentScale.y = myValue;

        // 3. 변경된 Vector3 값을 다시 localScale에 할당합니다.
        targetRectTransform.localScale = currentScale;
    }


}
