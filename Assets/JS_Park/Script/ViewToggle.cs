using UnityEngine;

public class ViewToggle : MonoBehaviour
{
    // 인스펙터 창에서 설정할 두 개의 카메라 변수
    public GameObject firstPersonCam;
    public GameObject thirdPersonCam;
   
    void Start()
    {
        // 게임 시작 시 기본은 1인칭으로 설정
        firstPersonCam.SetActive(true);
        thirdPersonCam.SetActive(false);
    }

    void Update()
    {
        // T 키를 눌렀을 때 (KeyCode)
        if (Input.GetKeyDown(KeyCode.T))
        {
            // 1인칭 카메라의 현재 활성화 상태를 가져옴
            bool isActive = firstPersonCam.activeSelf;

            // 서로의 활성화 상태를 전환
            firstPersonCam.SetActive(!isActive);
            thirdPersonCam.SetActive(isActive);
        }
    }
}
