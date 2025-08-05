// SceneChangeAction.cs

using UnityEngine;
using UnityEngine.SceneManagement; // 씬 관리를 위해 필요

public class SceneChange : MonoBehaviour, IDialogueAction
{
    [Tooltip("대화 종료 후 이동할 씬의 이름을 입력하세요.")]
    public string sceneNameToLoad;

    public void Apply()
    {
        // 씬 이름이 비어있지 않다면 해당 씬을 로드합니다.
        if (!string.IsNullOrEmpty(sceneNameToLoad))
        {
            Debug.Log($"{sceneNameToLoad} 씬으로 이동합니다.");
            SceneManager.LoadScene(sceneNameToLoad);
        }
        else
        {
            Debug.LogWarning("이동할 씬 이름이 지정되지 않았습니다!");
        }
    }
}