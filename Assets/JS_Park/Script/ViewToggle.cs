using UnityEngine;

public class ViewToggle : MonoBehaviour
{
    // �ν����� â���� ������ �� ���� ī�޶� ����
    public GameObject firstPersonCam;
    public GameObject thirdPersonCam;
   
    void Start()
    {
        // ���� ���� �� �⺻�� 1��Ī���� ����
        firstPersonCam.SetActive(true);
        thirdPersonCam.SetActive(false);
    }

    void Update()
    {
        // T Ű�� ������ �� (KeyCode)
        if (Input.GetKeyDown(KeyCode.T))
        {
            // 1��Ī ī�޶��� ���� Ȱ��ȭ ���¸� ������
            bool isActive = firstPersonCam.activeSelf;

            // ������ Ȱ��ȭ ���¸� ��ȯ
            firstPersonCam.SetActive(!isActive);
            thirdPersonCam.SetActive(isActive);
        }
    }
}
