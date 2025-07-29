using UnityEngine;

// 내가 지정한 방향으로 이동하고 싶다. 
// 포지션의 값이 바뀐다. 
public class SimpleMove : MonoBehaviour
{
    // string : "내용" , integer : 정수(소수점 X) 
    // float : 실수(소수점 o), Vector3 : 벡터(x, y, z)
    public Vector3 dir = new Vector3(0, 0, 1);
    public float speed = 1; // m/s 

    public float jumpPower = 5f;    // 점프(수직) 힘 
    // controller의 isgrounded 속성으로 대체
    // public bool isGround = false;   // boolean : treu(1) or false(0)
    public float gravity = -9.8f;   // 중력
    public float yVelocity = 0;     // y의 변화 

    CharacterController controller;

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        /// 아래의 '컨트롤러와 바닥의 닿아있는 여부를 확인하는 코드'는 void Update() 내부에 있었으나,
        /// Wrong 태그의 발판을 밟았을 때의 추락을 구현하기 위해선 자신이 무엇을 밟고 있는지 ControllerColliderHit을 활용해 알아내는 방법 외에는 없었기에
        /// 해당 함수 내부로 옮긴 것임

        // (캐릭터 컨트롤러가) 바닥에 닿아있는게 맞는가? 
        // if (controller.collisionFlags == CollisionFlags.Below)
        // {
        // wrong 태그의 발판과 닿았을 경우 isGround가 활성화 되지 않고 아래로 내려가는 가속도 그대로 내려감
        if (hit.gameObject.CompareTag("Wrong"))
        {
            Destroy(hit.gameObject);
        }
        // else
        // {
        //     isGround = true;
        //     yVelocity = 0;  // 바닥에 닿으면 아래로 못내려가게 0
        // }

        // }
        if (hit.gameObject.CompareTag("Goal"))
        {
            somethingFunction(); // goal에 도착시 실행할 무언가의 행동
        }
    }
    // 필요에 맞춰 추가
    public void somethingFunction() { }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controller = GetComponent<CharacterController>();   //그릇에 데이터를 담기. 
    }

    // Update is called once per frame
    void Update()
    {
        // 내가 입력한 방향으로 이동하고 싶다. 
        float h = Input.GetAxis("Horizontal");  //a(-1)나 d(+1)를 누를 때 
        float v = Input.GetAxis("Vertical");  //s(-1)나 w(+1)를 누를 때 

        dir = new Vector3(h, 0, v);
        // 정규화 Normalize = 방향을 유지하면서 벡터의 길이를 1로 고정 
        dir.Normalize();


        dir = Camera.main.transform.TransformDirection(dir);
        dir.y = 0;
        dir.Normalize();



        // // 바닥에 닿아있는게 맞고, 점프키를 누른게 맞다면, 
        // if (isGround == true && Input.GetButtonDown("Jump"))
        // {
        //     yVelocity = jumpPower;
        //     isGround = false;   // 바닥에 닿은게 아니다 
        // }

        // controller.isGrounded를 사용하여 지면 여부를 판단.
        // 이렇게 하면 절벽에서 떨어질 때도 정확하게 공중 상태를 감지할 수 있습니다.
        if (controller.isGrounded)
        {
            // 지면에 있을 때는 yVelocity를 아주 살짝 아래로 향하게 하여 바닥에 붙어있도록 함
            yVelocity = -0.1f;

            // 점프 버튼을 누르면 yVelocity에 점프 힘을 할당
            if (Input.GetButtonDown("Jump"))
            {
                yVelocity = jumpPower;
            }
        }
        // 중력을 적용해라 
        yVelocity = yVelocity + gravity * Time.deltaTime;
        dir.y = yVelocity;


        // 위치를 계속해서 바꾼다.
        // P(새로운 위치) = p0(기존의 위치) + v(방향) * t(시간) 
        // transform.position = transform.position + dir * speed * Time.deltaTime;
        controller.Move(dir * speed * Time.deltaTime);
        // transform.position += dir;
        // transform.Translate(dir * speed * Time.deltaTime);
        // 내가 지정한 방향으로 이동하고 싶다. 
    }
    private bool IsCheckGrounded()
    {
        // CharacterController.IsGrounded가 true라면 Raycast를 사용하지 않고 판정 종료
        if (controller.isGrounded) return true;        
        // 발사하는 광선의 초기 위치와 방향
        // 약간 신체에 박혀 있는 위치로부터 발사하지 않으면 제대로 판정할 수 없을 때가 있다.
        var ray = new Ray(this.transform.position + Vector3.up * 0.1f, Vector3.down);
        // 탐색 거리
        var maxDistance = 1.5f;
        // 광선 디버그 용도
        Debug.DrawRay(transform.position + Vector3.up * 0.1f, Vector3.down * maxDistance, Color.red);
        // Raycast의 hit 여부로 판정
        // 지상에만 충돌로 레이어를 지정
        return Physics.Raycast(ray, maxDistance, _fieldLayer);
    }
}
