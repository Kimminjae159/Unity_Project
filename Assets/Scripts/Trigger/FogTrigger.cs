using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class FogTrigger : MonoBehaviour
{
    [Header("플레이어 오브젝트 설정")]
    public GameObject playerObject;      // 플레이어 오브젝트 (Hierarchy에서 드래그앤드롭)

    [Header("생성할 Fog 설정")]
    public Color FogColor;       // 최종 Fog 색상 (옵션)
    public float FogDensity;     // 최종 Fog 밀도 (옵션)
    public float transitionDuration = 2.0f;     // 안개 생성 애니메이션 길이 설정

    private float originExposure;

    void OnTriggerEnter(Collider other)
    {
        originExposure = RenderSettings.skybox.GetFloat("_Exposure");
        if (other.gameObject == playerObject)
        {
            Debug.Log("Trigger On. Fog Making start");
            RenderSettings.fog = true;
            RenderSettings.fogColor = FogColor;
            StartCoroutine(FogMakingAnimation());
        }
    }

    IEnumerator FogMakingAnimation()
    {
        float startDensity = RenderSettings.fogDensity; // 현재 Fog 밀도를 시작 밀도로 설정
        float currentTime = 0f;

        while (currentTime < transitionDuration)
        {
            // 경과 시간에 따라 밀도를 선형적으로 보간합니다.
            RenderSettings.fogDensity = Mathf.Lerp(startDensity, FogDensity, currentTime / transitionDuration);
            RenderSettings.skybox.SetFloat("_Exposure", Mathf.Lerp(originExposure, 0, currentTime / transitionDuration));
            currentTime += Time.deltaTime; // 프레임당 시간만큼 증가
            yield return null; // 다음 프레임까지 기다립니다.
        }

        // 정확히 목표 밀도에 도달하도록 마지막으로 설정합니다.
        RenderSettings.fogDensity = FogDensity;

        Destroy(gameObject);
    }
}
