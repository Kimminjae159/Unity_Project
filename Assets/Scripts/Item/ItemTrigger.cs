using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using JetBrains.Annotations;

public class ItemTrigger : MonoBehaviour
{
    [Header("아이템 획득 시 실행할 기능")]
    public UnityEvent OnItemCollected;

    [Header("비활성화 옵션")]
    public bool ObjectDisable = false;
    public bool FogDisable = false;

    [Header("비활성화할 오브젝트")]
    public GameObject DisableObject; // Inspector에서 할당

    [Header("비활성화 지속 시간 (초)")]
    public float DisableDuration = 3.0f;

    private Renderer objectRenderer;

    Color originalFogColor;       // 기존 Fog 색상 (옵션)
    float originalFogDensity;     // 기존 Fog 밀도 (옵션)

    void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        if (FogDisable)
        {
            originalFogColor = RenderSettings.fogColor;
            originalFogDensity = RenderSettings.fogDensity;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            
            StartCoroutine(DisableSomething());
        }
    }
    private IEnumerator DisableSomething()
    {
        if (ObjectDisable)
        {
            DisableObject.SetActive(false);
            objectRenderer.enabled = false;
            yield return new WaitForSeconds(DisableDuration);
            DisableObject.SetActive(true);
        }
        if (FogDisable)
        {
            RenderSettings.fog = false;
            objectRenderer.enabled = false;
            yield return new WaitForSeconds(DisableDuration);
            RenderSettings.fog = true;
            RenderSettings.fogColor = originalFogColor;
            RenderSettings.fogDensity = originalFogDensity;
        }
        Destroy(gameObject);
    }
}
