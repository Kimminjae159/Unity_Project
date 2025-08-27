using UnityEngine;

public class GameEndCallTrigger : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        StageManager.instance.PlayerLevelClear();

        gameObject.SetActive(false);
    }
}
