using UnityEngine;

public class PlayerReset : MonoBehaviour
{
    public float minY = -5f;      // �÷��̾ �������� ���� y��
    private Vector3 startPos;     // ������ġ ����
    private SimpleMove moveScript;      // �̵� ��ũ��Ʈ ���� (Optional)
    private CharacterController controller;


    private Vector3 tempPos; // ���� ���� ������ ��ġ�� �񱳸� ���� �ӽ� ��ġ ���� ����
    public string objTag = "Correct"; // ���̺� ����Ʈ ����� �߰��� ������ �±׸� ����

    void Start()
    {
        // ���� ��ġ ���
        startPos= transform.position;
        // �̵� ��ũ��Ʈ �̸� ã�Ƶθ� �ʿ��� �� y�ӵ� ���� �� ����
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

        // �߰�: SimpleMove y�� ����(����/�������̸� ����, �߷� ���ӵ� �� �ʱ�ȭ)
        if (moveScript != null)
        {
            moveScript.yVelocity = 0;        // ����/���Ͻ� ���� �ӵ� �ʱ�ȭ
            moveScript.isGround = false;     // �ʿ�� �ٴڻ��� ��������
        }
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(hit.gameObject.CompareTag(objTag))
        {
            startPos =hit.gameObject.transform.position; // ���� ���� ������ �ùٸ� �����̸�, �� ������ ��ǥ�� ���� �������� ����
            startPos.y = 1f; // �����ϰ� ���ļ� �з����� ���� ����

            // �Ʒ��� x�� �������� �� �̾��� �� �� ������ ������ ��Ƶ� ���� ���̺� ��ġ���� �ڿ� ���̺갡 �Ǵ� ���� ������ ���� �ڵ�
            //tempPos = hit.gameObject.transform.position;
            //if (tempPos.x > startPos.x) startPos = tempPos;

        }
    }
}
