using UnityEngine;
using System.Collections;

public class FogRemover : MonoBehaviour
{

    [Header("비활성화 지속 시간 (초)")]
    public float DisableDuration = 3.0f;

    private Renderer objectRenderer;

    Color originalFogColor;       // 기존 Fog 색상 (옵션)
    float originalFogDensity;     // 기존 Fog 밀도 (옵션)
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        objectRenderer = GetComponent<Renderer>();
    }
    // 외부에서 코루틴 함수 실행이 가능하도록 생성한 함수
    public void ApplyFogFuc()
    {
        StartCoroutine(DisableFogFuc());
    }

    private IEnumerator DisableFogFuc()
    {
        originalFogColor = RenderSettings.fogColor;
        originalFogDensity = RenderSettings.fogDensity;

        RenderSettings.fog = false;
        objectRenderer.enabled = false;
        yield return new WaitForSeconds(DisableDuration);
        RenderSettings.fog = true;
        RenderSettings.fogColor = originalFogColor;
        RenderSettings.fogDensity = originalFogDensity;

        Destroy(gameObject);
    }
}
