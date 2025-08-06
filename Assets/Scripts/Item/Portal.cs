using UnityEngine;

public class Portal : MonoBehaviour
{
    public Transform destination;
    public GameObject player;

    // Update is called once per frame
    public void ApplyPortal()
    {
        Debug.Log("ApplyPortal");
        player.transform.position = new Vector3(30f, 2f, 0f);
        Debug.Log("ApplyedPortal");
    }
}
