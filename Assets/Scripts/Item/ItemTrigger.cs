using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using JetBrains.Annotations;

public class ItemTrigger : MonoBehaviour
{
    [Header("실행할 스크립트를 보유중인 아이템 설정")]
    public UnityEvent OnItemCollected;

    [Header("활성화할 옵션")]
    public bool ObjectDisable = false;
    public bool FogDisable = false;

    [Header("비활성화할 외부 오브젝트")]
    public GameObject ExternalObject; // Inspector에서 할당

    [Header("비활성화 지속 시간 (초)")]
    public float DisableDuration = 3.0f;

    private Renderer objectRenderer;

    Color originalFogColor;       // 기존 Fog 색상 (옵션)
    float originalFogDensity;     // 기존 Fog 밀도 (옵션)

    void Start()
    {
        objectRenderer = GetComponent<Renderer>();
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            OnItemCollected?.Invoke(); // OnItemCollected가 Null이 아니라면 할당된 함수를 실행
            if (ObjectDisable)
            {
                StartCoroutine(DisableObjectFuc());
            }
            if (FogDisable)
            {
                StartCoroutine(DisableFogFuc());
            }
        }
    }

    private IEnumerator DisableObjectFuc()
    {
        ExternalObject.SetActive(false);
        objectRenderer.enabled = false;
        yield return new WaitForSeconds(DisableDuration);
        ExternalObject.SetActive(true);

        Destroy(gameObject);
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
