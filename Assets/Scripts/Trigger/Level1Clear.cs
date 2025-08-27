using UnityEngine;
using System.Collections;

public class Level1Clear : MonoBehaviour
{
    private float FogDensity = 0.2f;     // 최종 Fog 밀도 (옵션)
    public float transitionDuration = 1.0f;     // 안개 생성 애니메이션 길이 설정

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }
    private float originExposure;

    // Update is called once per frame
    void OnTriggerEnter(Collider other)
    {
        originExposure = RenderSettings.skybox.GetFloat("_Exposure");
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Trigger On. Fog Making start");
            RenderSettings.fog = true;
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
            RenderSettings.skybox.SetFloat("_Exposure", Mathf.Lerp(originExposure, 1.3f, currentTime / transitionDuration));
            currentTime += Time.deltaTime; // 프레임당 시간만큼 증가
            yield return null; // 다음 프레임까지 기다립니다.
        }

        // 정확히 목표 밀도에 도달하도록 마지막으로 설정합니다.
        RenderSettings.fogDensity = FogDensity;

        Destroy(gameObject);
    }
}
