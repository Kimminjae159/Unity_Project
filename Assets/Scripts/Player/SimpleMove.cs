using UnityEngine;

// wasd에 맞춰 오브젝트 이동 
public class SimpleMove : MonoBehaviour
{
    public Vector3 dir = new Vector3(0, 0, 1);
    public float speed = 1; // m/s 

    public float jumpPower = 5f;    // 점프(수직) 힘 
    public float gravity = -9.8f;   // 중력
    public float yVelocity = 0;     // y의 변화 

    public float rotateSpeed = 10.0f;// 회전 속도를 조절할 수 있는 변수 (Inspector 창에서 수정 가능)
    CharacterController controller;
    bool isGrounded = false;
    bool wrongPanel = false;

    public Animator anim; //애니메이션 컴포넌트 변수 선언

    private bool DontMove;

    [Header("오디오 설정")]
    [SerializeField] private AudioClip jumpSound; // 점프 오디오 클립
    [SerializeField] private AudioClip fallingFloor; // 낙하 오디오 클립
    private AudioSource audioSource;
    
    void Start()
    {
        controller = GetComponent<CharacterController>();   //그릇에 데이터를 담기. 
        DontMove = false;
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        float h = 0f, v = 0f;
        // 내가 입력한 방향으로 이동하고 싶다. 
        if (!DontMove)
        {
            h = Input.GetAxis("Horizontal");  //a(-1)나 d(+1)를 누를 때 
            v = Input.GetAxis("Vertical");  //s(-1)나 w(+1)를 누를 때 
            if (isGrounded && (h != 0f || v != 0f))
            {
                if (!audioSource.isPlaying) audioSource.Play();
            }
            else
            {
                if (audioSource.isPlaying) audioSource.Stop();
            }
        }
        else
        {
            if (audioSource.isPlaying) audioSource.Stop();
        }

        dir = new Vector3(h, 0, v);
        // 정규화 Normalize = 방향을 유지하면서 벡터의 길이를 1로 고정 
        dir.Normalize();

        anim.SetFloat("BlendX", h); //BlenderX에 h값 전달(좌우)
        anim.SetFloat("BlendY", v); //BlenderY에 v값 전달(상하)

        if (controller.collisionFlags == CollisionFlags.Below)
        {
            if (!wrongPanel && !isGrounded)
            {
                anim.SetTrigger("isLanding");//Animation trigger 지정(착지 동작하도록)
            }
            isGrounded = true;
            if (wrongPanel)
            {
                isGrounded = false;
            }
            else
            {
                yVelocity = 0;
            }
        }
        // else
        // {
        //     if (isGrounded == true)
        //     {
        //         anim.SetTrigger("isFalling");
        //     }
        // }
        if (Input.GetButtonDown("Jump") && isGrounded && !wrongPanel && !DontMove)
        {
            audioSource.PlayOneShot(jumpSound);
            anim.SetTrigger("isJump");//Animation trigger 지정(점프 동작하도록)
            yVelocity = jumpPower;
            isGrounded = false;
        }
        // 기존 로직
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

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Wrong"))
        {
            audioSource.PlayOneShot(fallingFloor);
            GameManager.instance.PlayerStepPlatform(false);
            wrongPanel = true;
            Destroy(hit.gameObject);
        }
        else if (wrongPanel)
        {
            wrongPanel = false;
        }

        if (hit.gameObject.CompareTag("Goal"))
        {
            somethingFunction(); // goal에 도착시 실행할 무언가의 행동
        }
    }
    public void somethingFunction()
    {
        RenderSettings.skybox.SetFloat("_Exposure", 1.3f);
    }

    public void PlayerDontMove(bool dontMove)
    {
        DontMove = dontMove;
    }
}
