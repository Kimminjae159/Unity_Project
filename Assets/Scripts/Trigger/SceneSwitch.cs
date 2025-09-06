using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitch : MonoBehaviour
{
    [Header("전환할 씬 이름")]
    public string sceneName;
    [Header("자동으로 다음 씬 이동을 할 경우 True")]
    public bool nextScene = true;

    // 외부에서 이 Trigger를 사용하도록 하기 위함
    public void ApplySceneSwitch()
    {
        // 씬 이름이 비어있지 않다면 해당 씬을 로드합니다.
        if (nextScene) {
            GameManager.instance.PrepareForNewScene(SceneManager.GetActiveScene().buildIndex - 1);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        } else {
            if (!string.IsNullOrEmpty(sceneName))
            {
                SceneManager.LoadScene(sceneName);
            }
        }
    }
}
