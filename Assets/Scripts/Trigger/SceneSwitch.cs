using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitch : MonoBehaviour
{
    [Header("전환할 씬 이름")]
    public string sceneName;

    // 충돌로 Trigger가 발동될 때 발동
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(sceneName);
        }
    }

    // 외부에서 이 Trigger를 사용하도록 하기 위함
    public void ApplySceneSwitch()
    {
        // 씬 이름이 비어있지 않다면 해당 씬을 로드합니다.
        if (!string.IsNullOrEmpty(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
