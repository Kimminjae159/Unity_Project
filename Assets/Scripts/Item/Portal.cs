using UnityEngine;

public class Portal : MonoBehaviour
{
    public GameObject destination;
    public GameObject player;


    // Update is called once per frame
    public void ApplyPortal()
    {
        player.GetComponent<CharacterController>().enabled = false;
        player.transform.position = destination.transform.position + Vector3.up;
        Debug.Log(player.transform.position);
        player.GetComponent<CharacterController>().enabled = true;
    }
}
