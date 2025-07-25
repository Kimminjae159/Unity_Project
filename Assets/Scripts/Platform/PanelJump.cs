using UnityEngine;
using System.Collections; // 코루틴을 사용하기 위해 필요

public class PanelJump : MonoBehaviour
{

    [Header("Wrong Panel Movement Settings")]
    public float jumpHeight = 1.0f; // 큐브가 상승할 높이
    public float jumpDuration = 1.0f; // 상승 및 하강에 걸리는 총 시간 (왕복 시간)


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            ApplyPanelJump();
        }
    }
    public void ApplyPanelJump()
    {
       // Wrong 태그 큐브만 탐색 
       foreach (Transform child in transform)
       {
            GameObject obj = child.gameObject;
            if (obj.CompareTag("Wrong"))
            {
                StartCoroutine(MoveWrongPanel(obj));
            }
        }
    }

    // "Wrong" 태그를 가진 패널을 움직이는 코루틴
    private IEnumerator MoveWrongPanel(GameObject panel)
    {
        Vector3 originalPosition = panel.transform.position;
        Vector3 targetPosition = originalPosition + Vector3.up * jumpHeight;

        float elapsedTime = 0f;

        // 상승 애니메이션
        while (elapsedTime < jumpDuration / 2f) // 총 시간의 절반은 상승
        {
            panel.transform.position = Vector3.Lerp(originalPosition, targetPosition, (elapsedTime / (jumpDuration / 2f)));
            elapsedTime += Time.deltaTime;
            yield return null; // 다음 프레임까지 대기
        }
        panel.transform.position = targetPosition; // 정확한 목표 위치로 설정 (오차 방지)

        elapsedTime = 0f; // 시간 초기화

        // 하강 애니메이션
        while (elapsedTime < jumpDuration / 2f) // 총 시간의 나머지 절반은 하강
        {
            panel.transform.position = Vector3.Lerp(targetPosition, originalPosition, (elapsedTime / (jumpDuration / 2f)));
            elapsedTime += Time.deltaTime;
            yield return null; // 다음 프레임까지 대기
        }
        panel.transform.position = originalPosition; // 정확한 원래 위치로 설정 (오차 방지)
    }
}