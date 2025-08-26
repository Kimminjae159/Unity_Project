using UnityEngine;
using System.Collections;

public class FogRemover : MonoBehaviour
{

    [Header("비활성화 지속 시간 (초)")]
    public float DisableDuration = 3.0f;

    Color originalFogColor;       // 기존 Fog 색상 (옵션)
    float originalFogDensity;     // 기존 Fog 밀도 (옵션)

    // 외부에서 코루틴 함수 실행이 가능하도록 생성한 함수
    public void ApplyFogFunc()
    {
        StartCoroutine(DisableFogFunc());
    }

    private IEnumerator DisableFogFunc()
    {
        originalFogColor = RenderSettings.fogColor;
        originalFogDensity = RenderSettings.fogDensity;

        RenderSettings.fog = false;
        yield return new WaitForSeconds(DisableDuration);
        RenderSettings.fog = true;
        RenderSettings.fogColor = originalFogColor;
        RenderSettings.fogDensity = originalFogDensity;
    }
}
