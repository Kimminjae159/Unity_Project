using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class ItemTrigger : MonoBehaviour
{
    [Header("아이템 획득 시 실행할 기능")]
    public UnityEvent OnItemCollected;

    [Header("비활성화할 오브젝트")]
    public GameObject globalVolumeObject; // Inspector에서 할당

    [Header("비활성화 지속 시간 (초)")]
    public float disableDuration = 3.0f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnItemCollected?.Invoke();

            if (globalVolumeObject != null)
            {
                StartCoroutine(DisableVolumeTemporarily());
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
    private IEnumerator DisableVolumeTemporarily()
    {
        globalVolumeObject.SetActive(false);
        yield return new WaitForSeconds(disableDuration);
        globalVolumeObject.SetActive(true);
        Destroy(gameObject);
    }
}
