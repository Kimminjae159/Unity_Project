using UnityEngine;

public class PlayerReset : MonoBehaviour
{
    public float minY = -5f;      // �÷��̾ �������� ���� y��
    private Vector3 startPos;     // ������ġ ����
    private SimpleMove moveScript;      // �̵� ��ũ��Ʈ ���� (Optional)
    private CharacterController controller;

    void Start()
    {
        // ���� ��ġ ���
        startPos = transform.position;
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
}
