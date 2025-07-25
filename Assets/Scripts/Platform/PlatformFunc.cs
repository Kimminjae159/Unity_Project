using UnityEngine;

public class PlatformFunc : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // Wrong 태그만 사라짐
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
