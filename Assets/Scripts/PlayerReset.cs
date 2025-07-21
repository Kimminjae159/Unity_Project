using UnityEngine;

public class PlayerReset : MonoBehaviour
{
    public float minY = -5f;      // 플레이어가 떨어지는 기준 y값
    private Vector3 startPos;     // 시작위치 저장
    private SimpleMove moveScript;      // 이동 스크립트 참조 (Optional)
    private CharacterController controller;


    private Vector3 tempPos; // 새로 밝은 발판의 위치와 비교를 위한 임시 위치 저장 변수
    public string objTag = "Correct"; // 세이브 포인트 기능을 추가할 발판의 태그명 저장

    void Start()
    {
        // 최초 위치 기억
        startPos= transform.position;
        // 이동 스크립트 미리 찾아두면 필요할 때 y속도 리셋 등 가능
        moveScript = GetComponent<SimpleMove>();
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

        // 추가: SimpleMove y값 리셋(낙하/점프중이면 멈춤, 중력 가속도 등 초기화)
        if (moveScript != null)
        {
            moveScript.yVelocity = 0;        // 점프/낙하시 수직 속도 초기화
            moveScript.isGround = false;     // 필요시 바닥상태 강제조정
        }
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(hit.gameObject.CompareTag(objTag))
        {
            startPos =hit.gameObject.transform.position; // 만약 닿은 발판이 올바른 발판이면, 그 발판의 좌표를 리셋 지점으로 갱신
            startPos.y = 1f; // 발판하고 겹쳐서 밀려나는 현상 방지

            // 아래는 x축 방향으로 쭉 이어져 갈 때 이전의 발판을 밟아도 현재 세이브 위치보다 뒤에 세이브가 되는 현상 방지를 위한 코드
            //tempPos = hit.gameObject.transform.position;
            //if (tempPos.x > startPos.x) startPos = tempPos;

        }
    }
}
