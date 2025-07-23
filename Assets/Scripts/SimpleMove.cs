using UnityEngine;

// ���� ������ �������� �̵��ϰ� �ʹ�. 
// �������� ���� �ٲ��. 
public class SimpleMove : MonoBehaviour
{
    // string : "����" , integer : ����(�Ҽ��� X) 
    // float : �Ǽ�(�Ҽ��� o), Vector3 : ����(x, y, z)
    public Vector3 dir = new Vector3(0, 0, 1);
    public float speed = 1; // m/s 

    public float jumpPower = 5f;    // ����(����) �� 
    public bool isGround = false;   // boolean : treu(1) or false(0)
    public float gravity = -9.8f;   // �߷�
    public float yVelocity = 0;     // y�� ��ȭ 

    CharacterController controller;

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        /// �Ʒ��� '��Ʈ�ѷ��� �ٴ��� ����ִ� ���θ� Ȯ���ϴ� �ڵ�'�� void Update() ���ο� �־�����,
        /// Wrong �±��� ������ ����� ���� �߶��� �����ϱ� ���ؼ� �ڽ��� ������ ��� �ִ��� ControllerColliderHit�� Ȱ���� �˾Ƴ��� ��� �ܿ��� �����⿡
        /// �ش� �Լ� ���η� �ű� ����
        
        // (ĳ���� ��Ʈ�ѷ���) �ٴڿ� ����ִ°� �´°�? 
        if (controller.collisionFlags == CollisionFlags.Below)
        {
            // wrong �±��� ���ǰ� ����� ��� isGround�� Ȱ��ȭ ���� �ʰ� �Ʒ��� �������� ���ӵ� �״�� ������
            if (hit.gameObject.CompareTag("Wrong"))
            {
                Destroy(hit.gameObject);
            }
            else
            {
                isGround = true;
                yVelocity = 0;  // �ٴڿ� ������ �Ʒ��� ���������� 0
            }

        }
        if (hit.gameObject.CompareTag("Goal"))
        {
            somethingFunction(); // goal�� ������ ������ ������ �ൿ
        }
    }
    // �ʿ信 ���� �߰�
    public void somethingFunction() { }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controller = GetComponent<CharacterController>();   //�׸��� �����͸� ���. 
    }

    // Update is called once per frame
    void Update()
    {
        // ���� �Է��� �������� �̵��ϰ� �ʹ�. 
        float h = Input.GetAxis("Horizontal");  //a(-1)�� d(+1)�� ���� �� 
        float v = Input.GetAxis("Vertical");  //s(-1)�� w(+1)�� ���� �� 

        dir = new Vector3(h, 0, v);
        // ����ȭ Normalize = ������ �����ϸ鼭 ������ ���̸� 1�� ���� 
        dir.Normalize();


        dir = Camera.main.transform.TransformDirection(dir);
        dir.y = 0;
        dir.Normalize();

        

        // �ٴڿ� ����ִ°� �°�, ����Ű�� ������ �´ٸ�, 
        if (isGround == true && Input.GetButtonDown("Jump"))
        {
            yVelocity = jumpPower;
            isGround = false;   // �ٴڿ� ������ �ƴϴ� 
        }
        // �߷��� �����ض� 
        yVelocity = yVelocity + gravity * Time.deltaTime;
        dir.y = yVelocity;


        // ��ġ�� ����ؼ� �ٲ۴�.
        // P(���ο� ��ġ) = p0(������ ��ġ) + v(����) * t(�ð�) 
        // transform.position = transform.position + dir * speed * Time.deltaTime;
        controller.Move(dir * speed * Time.deltaTime);
        // transform.position += dir;
        // transform.Translate(dir * speed * Time.deltaTime);
        // ���� ������ �������� �̵��ϰ� �ʹ�. 
    }
}
