using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [Tooltip("이 트리거가 실행시킬 ScriptManager를 할당하세요.")]
    public DialogueManager scriptManager;

    [Tooltip("트리거가 한 번만 실행되도록 하려면 체크하세요.")]
    public bool triggerOnce = true;

    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        // 트리거에 들어온 오브젝트가 "Player" 태그를 가졌는지 확인
        if (other.CompareTag("Player"))
        {
            // 아직 실행된 적 없거나, 여러 번 실행 가능한 경우
            if (!hasTriggered || !triggerOnce)
            {
                // 할당된 ScriptManager의 대화 시작 함수를 호출
                if (scriptManager != null)
                {
                    scriptManager.StartDialogue();
                    hasTriggered = true; // 실행되었다고 표시
                }
                else
                {
                    Debug.LogWarning("실행할 ScriptManager가 할당되지 않았습니다!");
                }
            }
        }
    }
}

