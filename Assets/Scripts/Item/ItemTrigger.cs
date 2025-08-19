using UnityEngine;
using UnityEngine.Events;
public class ItemTrigger : MonoBehaviour
{
    [Header("실행할 스크립트를 보유중인 아이템 설정")]
    public UnityEvent OnItemCollected;

    [Header("즉발 아이템이면 true (check)")]
    public bool IsAutoUsing = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //OnItemCollected?.Invoke();로 대체 가능했으나 Destory 때문에 조건문으로 대체
            if (OnItemCollected != null)
            {
                ItemManager.instance.ItemGet(OnItemCollected, IsAutoUsing);
                Destroy(gameObject);
            }
        }
    }
}
