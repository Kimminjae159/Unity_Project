using UnityEngine;

public class GetStar : MonoBehaviour
{
    public DialogueAsset dialogue_kr;
    public DialogueAsset dialogue_en;
    private DialogueAsset dialogue;
    public AudioSource audioSource;
    public AudioClip audioClip;
    void Start()
    {
        int languageMode = GameManager.instance.language;
        if (languageMode == 0)
        {
            dialogue = dialogue_kr;
        }
        else if(languageMode==1)
        {
            dialogue = dialogue_en;
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 중복 실행 방지를 위해 콜라이더를 즉시 끕니다.
            GetComponent<Collider>().enabled = false;
            // 아이템이 바로 사라진 것처럼 보이게 렌더러를 끕니다.
            GetComponent<Renderer>().enabled = false;
            audioSource.PlayOneShot(audioClip);
            GameManager.instance.GetScore(1000);
            StageManager.instance.PlayerEvent(dialogue);
            Destroy(gameObject, audioClip.length);
        }
    }
}
