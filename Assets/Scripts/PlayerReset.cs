using UnityEngine;

public class PlayerReset : MonoBehaviour
{
    public float minY = -5f;      // 플레이어가 떨어지는 기준 y값
    private Vector3 startPos;     // 시작위치 저장
    private SimpleMove moveScript;      // 이동 스크립트 참조 (Optional)
    private CharacterController controller;

    void Start()
    {
        // 최초 위치 기억
        startPos = transform.position;
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
}
