using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class ItemTrigger : MonoBehaviour
{
    [Header("������ ȹ�� �� ������ ���")]
    public UnityEvent OnItemCollected;

    [Header("��Ȱ��ȭ�� ������Ʈ")]
    public GameObject globalVolumeObject; // Inspector���� �Ҵ�

    [Header("��Ȱ��ȭ ���� �ð� (��)")]
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
