// 파일 이름: PlayerScoreInteraction.cs (수정 버전)

using UnityEngine;

public class PlayerScoreInteraction : MonoBehaviour
{
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        string tag = hit.gameObject.tag;

        if (tag == "Correct")
        {
            // ScoreManager에게 "이 발판(hit.gameObject)을 밟았어!" 라고 알려줍니다.
            ScoreManager.instance.AddScoreForCorrectStep(hit.gameObject);
            // 발판은 유지됩니다.
        }
        else if (tag == "Wrong")
        {
            // ScoreManager에게 콤보를 리셋하라고 알립니다.
            ScoreManager.instance.ResetCombo();
            // 잘못된 발판은 사라집니다.
            hit.gameObject.SetActive(false);
        }
        else if (tag == "Finish")
        {
            ScoreManager.instance.AddScoreForLevelClear();
        }
    }
}