using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using JetBrains.Annotations;
using Unity.VisualScripting;

public class ItemTrigger : MonoBehaviour
{
    [Header("실행할 스크립트를 보유중인 아이템 설정")]
    public UnityEvent OnItemCollected;

    [Header("활성화할 옵션")]
    public bool ObjectDisable = false;
    
    [Header("비활성화할 외부 오브젝트")]
    public GameObject ExternalObject; // Inspector에서 할당

    [Header("비활성화 지속 시간 (초)")]
    public float DisableDuration = 3.0f;

    private Renderer objectRenderer;

    void Start()
    {
        objectRenderer = GetComponent<Renderer>();
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            if (OnItemCollected != null)
            {
                OnItemCollected.Invoke();
                //Destroy(gameObject);
            }
            //OnItemCollected?.Invoke(); // OnItemCollected가 Null이 아니라면 할당된 함수를 실행
            if (ObjectDisable)
            {
                StartCoroutine(DisableObjectFuc());
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

    
}
