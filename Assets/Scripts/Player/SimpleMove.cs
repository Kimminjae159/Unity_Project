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
    bool NeverCanJump = false;

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        /// 아래의 '컨트롤러와 바닥의 닿아있는 여부를 확인하는 코드'는 void Update() 내부에 있었으나,
        /// Wrong 태그의 발판을 밟았을 때의 추락을 구현하기 위해선 자신이 무엇을 밟고 있는지 ControllerColliderHit을 활용해 알아내는 방법 외에는 없었기에
        /// 해당 함수 내부로 옮긴 것임

        /// OnControllerColliderHit 함수는 CharacterController의 "Move()" 함수가 호출될 때마다 충돌 여부 확인후, 충돌이 일어났다면 호출된다. 
        /// 캐릭터 컨트롤러의 바닥이 "지면"과 닿아있을 경우
        /// 1. yVelocity를 0으로 설정
        /// 2. 현재 충돌한 것이 Wrong 태그를 가진 오브젝트가 아니면 점프키를 눌렀을 때 점프가 가능
        /// 3. 현재 충돌한 것이 Wrong 태그를 가진 오브젝트가 맞다면 닿은 오브젝트를 파괴

        if (hit.gameObject.CompareTag("Wrong"))
        {
            NeverCanJump = true;
            Destroy(hit.gameObject);
        }
        else if (NeverCanJump)
        {
            NeverCanJump = false;
        }

        if (hit.gameObject.CompareTag("Goal"))
        {
            somethingFunction(); // goal에 도착시 실행할 무언가의 행동
        }
    }
    // 필요에 맞춰 추가
    public void somethingFunction()
    {
        RenderSettings.skybox.SetFloat("_Exposure", 1);
    }

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

        if (controller.collisionFlags == CollisionFlags.Below && !NeverCanJump)
        {
            yVelocity = 0;
            if (Input.GetButtonDown("Jump"))
            {
                yVelocity = jumpPower;
            }
        }
        dir = Camera.main.transform.TransformDirection(dir);
        dir.y = 0;
        dir.Normalize();

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
    
}
