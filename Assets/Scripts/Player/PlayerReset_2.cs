using UnityEngine;

public class PlayerReset_2 : MonoBehaviour
{
    public float minY = -5f;      // 플레이어가 리셋되는 y값
    private Vector3 startPos;     // 시작위치 저장
    private SimpleMove_2 moveScript;      // 이동 스크립트 참조 (Optional)
    private CharacterController controller;


    private Vector3 tempPos; // 세이브 포인트 오브젝트와 충돌 시, 해당 오브젝트의 위치를 받기위한 임시 위치 저장 변수
    public string objTag = "Correct"; // 세이브 포인트 오브젝트에 추가할 태그를 지정

    void Start()
    {
        // 시작 위치 저장
        startPos= transform.position;
        // 이동 스크립트 컴포넌트 가져오기
        moveScript = GetComponent<SimpleMove_2>();
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (transform.position.y < minY)
        {
            ResetToStart();
        }
    }

    void ResetToStart()
    {
        if (controller != null)
        {
            controller.enabled = false;
            transform.position = startPos;
            controller.enabled = true;
        }
        else
        {
            transform.position = startPos;
        }

        // 추가: SimpleMove y축 속도 초기화 (추락/사망시 속도 누적 방지, 중력 가속도 값 초기화)
        if (moveScript != null)
        {
            moveScript.yVelocity = 0;
        }
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(hit.gameObject.CompareTag(objTag))
        {
            startPos =hit.gameObject.transform.position; // 세이브 포인트 오브젝트와 충돌했다면, 이 오브젝트의 좌표를 새로운 시작점으로 설정
            startPos.y = 1f; // 부드럽게 내려오도록 y값 조정

        }
    }
}
