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
    public bool isGround = false;   // boolean : treu(1) or false(0)
    public float gravity = -9.8f;   // 중력
    public float yVelocity = 0;     // y의 변화 

    CharacterController controller;

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Cube"))
        {
            Destroy(hit.gameObject);
        }
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


        dir = Camera.main.transform.TransformDirection(dir);
        dir.y = 0;
        dir.Normalize();

        // (캐릭터 컨트롤러가) 바닥에 닿아있는게 맞는가? 
        if(controller.collisionFlags == CollisionFlags.Below)
        {
            isGround = true;
            yVelocity = 0;  // 바닥에 닿으면 아래로 못내려가게 0
        }

        // 바닥에 닿아있는게 맞고, 점프키를 누른게 맞다면, 
        if (isGround == true && Input.GetButtonDown("Jump"))
        {
            yVelocity = jumpPower;
            isGround = false;   // 바닥에 닿은게 아니다 
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
}
