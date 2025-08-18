using UnityEngine;

public class PlayerEnterClearTrigger : MonoBehaviour
{
    public StageManager stageManager;
    public bool IsEnterTrigger = false;
    public bool IsClearTrigger = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (IsEnterTrigger)
                stageManager.PlayerLevelEnter();
            else if (IsClearTrigger)
                stageManager.PlayerLevelClear();
        
            gameObject.SetActive(false);
        }
    }
}
