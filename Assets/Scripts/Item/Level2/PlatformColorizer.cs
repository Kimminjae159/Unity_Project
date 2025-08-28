using UnityEngine;
using System.Collections;
using System.Collections.Generic; // List를 사용하기 위해 추가

public class PlatformColorizer : MonoBehaviour
{
    [Header("Color Settings")]
    public Material correctMat;
    public Material wrongMat;
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
            return;
        }

        if (effectCoroutine != null)
            StopCoroutine(effectCoroutine);

        effectCoroutine = StartCoroutine(HighlightRoutine());
    }

    private IEnumerator HighlightRoutine()
    {
        // (Renderer, 원본 Material 배열)을 저장할 리스트
        var originals = new List<(Renderer, Material[])>();

        // 지정된 platformToColorize의 자식들을 순회
        foreach (Transform child in platformToColorize)
        {
            var renderer = child.GetComponent<Renderer>();
            if (!renderer) continue;

            // 원본 머티리얼 배열을 그대로 저장 (중요: renderer.materials는 복사본을 반환)
            originals.Add((renderer, renderer.materials));

            // 태그별로 지정색 입힘
            if (child.CompareTag("Correct") || child.CompareTag("Wrong"))
            {
                // [수정] 자식 오브젝트의 모든 머티리얼을 변경하기 위해 새 배열 생성
                var newMaterials = new Material[renderer.materials.Length];
                Material materialToApply = child.CompareTag("Correct") ? correctMat : wrongMat;

                for (int i = 0; i < newMaterials.Length; i++)
                {
                    newMaterials[i] = materialToApply;
                }

                // [수정] 렌더러의 materials 프로퍼티에 새로운 배열을 할당해야 적용됨
                renderer.materials = newMaterials;
            }
        }

        yield return new WaitForSeconds(duration);

        // 원래 머티리얼로 복구
        foreach (var pair in originals)
        {
            var renderer = pair.Item1;
            var originalMaterials = pair.Item2;

            // [수정] 저장해두었던 원래 머티리얼 배열을 다시 할당
            renderer.materials = originalMaterials;
        }

        effectCoroutine = null;
    }
}