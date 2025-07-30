using UnityEngine;
using System.Collections; // 코루틴을 사용하기 위해 필요

public class PanelHiding : MonoBehaviour
{

    [Header("Wrong Panel Hiding Settings")]
    public float delayBeforeHide = 1.0f; // 오브젝트가 사라지기 전 대기 시간
    public float hideDuration = 2.0f; // 오브젝트가 보이지 않는 시간 (n초)


    void Update()
    {
       
    }
    public void ApplyPanelHide()
    {
        // Wrong 태그 큐브만 탐색 
        foreach (Transform child in transform)
        {
            GameObject obj = child.gameObject;
            if (obj.CompareTag("Wrong"))
            {
                StartCoroutine(HideAndShowPanel(obj));
            }
        }
    }

    // "Wrong" 태그를 가진 패널을 숨겼다가 보이게 하는 코루틴
    private IEnumerator HideAndShowPanel(GameObject panel)
    {
        // 1. 설정된 시간만큼 대기
        yield return new WaitForSeconds(delayBeforeHide);

        // 2. 패널을 비활성화 (보이지 않게 함)
        panel.SetActive(false);

        // 3. 설정된 시간 (n초) 동안 대기
        yield return new WaitForSeconds(hideDuration);

        // 4. 패널을 다시 활성화 (보이게 함)
        panel.SetActive(true);
    }
}