using UnityEngine;

public class Level0Start : MonoBehaviour
{
    public DialogueAsset dialogue;
    void LateUpdate()
    {
        DialogueManager.instance.StartDialogue(dialogue);
        DestroyImmediate(gameObject);
    }
}
