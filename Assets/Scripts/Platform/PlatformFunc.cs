using UnityEngine;

public class PlatformFunc : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // Wrong �±׸� �����
        if (hit.gameObject.CompareTag("Wrong"))
        {
            Destroy(hit.gameObject);
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
