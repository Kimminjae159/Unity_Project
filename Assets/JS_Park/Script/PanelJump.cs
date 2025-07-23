using UnityEngine;
using System.Collections; // 코루틴을 사용하기 위해 필요

public class PanelJump : MonoBehaviour
{
    public PlatformGenerator platformGenerator; // PlatformGenerator를 Inspector에서 드래그로 연결

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
        var cubes = platformGenerator.cubes;

        if (cubes == null) return;

        int cols = platformGenerator.cols;
        int rows = platformGenerator.rows;

        for (int x = 0; x < cols; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                var obj = cubes[x, y];
                if (!obj) continue; // 큐브가 null인 경우 건너뛰기

                if (obj.CompareTag("Wrong")) // 태그 비교는 CompareTag를 사용하는 것이 효율적입니다.
                {
                    // "Wrong" 태그를 가진 큐브에 대해 코루틴 시작
                    StartCoroutine(MoveWrongPanel(obj));
                }
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