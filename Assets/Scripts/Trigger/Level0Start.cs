using UnityEngine;

public class Level0Start : MonoBehaviour
{
    public DialogueAsset dialogue_kr;
    public DialogueAsset dialogue_en;
    private DialogueAsset dialogue;
    void LateUpdate()
    {
        if (GameManager.instance.language == 0) { dialogue = dialogue_kr; }
        else if (GameManager.instance.language == 1) { dialogue = dialogue_en; }
        
        DialogueManager.instance.StartDialogue(dialogue);
        DestroyImmediate(gameObject);
    }
}
