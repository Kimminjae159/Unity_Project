using UnityEngine;

public class PlayerEnterClearTrigger : MonoBehaviour
{
    public bool IsEnterTrigger = false;
    public bool IsClearTrigger = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (IsEnterTrigger)
                StageManager.instance.PlayerLevelEnter();
            else if (IsClearTrigger)
                StageManager.instance.PlayerLevelClear();
        
            gameObject.SetActive(false);
        }
    }
}
