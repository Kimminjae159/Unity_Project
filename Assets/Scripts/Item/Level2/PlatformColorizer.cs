using UnityEngine;
using System.Collections;
using System.Collections.Generic; // List를 사용하기 위해 추가

public class PlatformColorizer : MonoBehaviour
{
    [Header("Color Settings")]
    public Color correctColor = Color.green;
    public Color wrongColor = Color.red;
    public float duration = 3f;

    // [수정] 색상을 변경할 대상을 외부에서 지정받기 위한 변수
    [Header("Target Object")]
    [Tooltip("색상을 변경할 부모 오브젝트 (Platform)를 여기에 연결하세요.")]
    public Transform platformToColorize;

    private Coroutine effectCoroutine;


    public void PanelsColorizer()
    {
        // [추가] platformToColorize 변수가 인스펙터에서 할당되었는지 확인
        if (platformToColorize == null)
        {
            Debug.LogError("PlatformColorizer: 색상을 변경할 대상(platformToColorize)이 할당되지 않았습니다!");
            return;
        }

        if (effectCoroutine != null)
            StopCoroutine(effectCoroutine);

        effectCoroutine = StartCoroutine(HighlightRoutine());
    }

    private IEnumerator HighlightRoutine()
    {
        // (Renderer, Color[]) 튜플을 사용하기 위해 System.Collections.Generic 네임스페이스 필요
        var originals = new List<(Renderer, Color[])>();

        // [수정] 기존 transform 대신 지정된 platformToColorize의 자식들을 순회
        foreach (Transform child in platformToColorize)
        {
            var renderer = child.GetComponent<Renderer>();
            if (!renderer) continue;

            // 원본 색상 저장
            Color[] originalColors = new Color[renderer.materials.Length];
            for (int i = 0; i < originalColors.Length; i++)
                originalColors[i] = renderer.materials[i].color;

            originals.Add((renderer, originalColors));

            // 태그별로 지정색 입힘
            if (child.CompareTag("Correct"))
            {
                foreach (var mat in renderer.materials)
                    mat.color = correctColor;
            }
            else if (child.CompareTag("Wrong"))
            {
                foreach (var mat in renderer.materials)
                    mat.color = wrongColor;
            }
        }

        yield return new WaitForSeconds(duration);

        // 원래 색상 복구
        foreach (var pair in originals)
        {
            var renderer = pair.Item1;
            var prevColors = pair.Item2;
            for (int i = 0; i < renderer.materials.Length; i++)
            {
                renderer.materials[i].color = prevColors[i];
            }
        }

        effectCoroutine = null;
    }
}