using UnityEngine;
using System.Collections; // �ڷ�ƾ�� ����ϱ� ���� �ʿ�

public class PanelJump : MonoBehaviour
{
    public PlatformGenerator platformGenerator; // PlatformGenerator�� Inspector���� �巡�׷� ����

    [Header("Wrong Panel Movement Settings")]
    public float jumpHeight = 1.0f; // ť�갡 ����� ����
    public float jumpDuration = 1.0f; // ��� �� �ϰ��� �ɸ��� �� �ð� (�պ� �ð�)


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            ApplyPanelJump();
        }
    }
    public void ApplyPanelJump()
    {
        var cubes = platformGenerator.cubes;

        if (cubes == null) return;

        int cols = platformGenerator.cols;
        int rows = platformGenerator.rows;

        for (int x = 0; x < cols; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                var obj = cubes[x, y];
                if (!obj) continue; // ť�갡 null�� ��� �ǳʶٱ�

                if (obj.CompareTag("Wrong")) // �±� �񱳴� CompareTag�� ����ϴ� ���� ȿ�����Դϴ�.
                {
                    // "Wrong" �±׸� ���� ť�꿡 ���� �ڷ�ƾ ����
                    StartCoroutine(MoveWrongPanel(obj));
                }
            }
        }
    }

    // "Wrong" �±׸� ���� �г��� �����̴� �ڷ�ƾ
    private IEnumerator MoveWrongPanel(GameObject panel)
    {
        Vector3 originalPosition = panel.transform.position;
        Vector3 targetPosition = originalPosition + Vector3.up * jumpHeight;

        float elapsedTime = 0f;

        // ��� �ִϸ��̼�
        while (elapsedTime < jumpDuration / 2f) // �� �ð��� ������ ���
        {
            panel.transform.position = Vector3.Lerp(originalPosition, targetPosition, (elapsedTime / (jumpDuration / 2f)));
            elapsedTime += Time.deltaTime;
            yield return null; // ���� �����ӱ��� ���
        }
        panel.transform.position = targetPosition; // ��Ȯ�� ��ǥ ��ġ�� ���� (���� ����)

        elapsedTime = 0f; // �ð� �ʱ�ȭ

        // �ϰ� �ִϸ��̼�
        while (elapsedTime < jumpDuration / 2f) // �� �ð��� ������ ������ �ϰ�
        {
            panel.transform.position = Vector3.Lerp(targetPosition, originalPosition, (elapsedTime / (jumpDuration / 2f)));
            elapsedTime += Time.deltaTime;
            yield return null; // ���� �����ӱ��� ���
        }
        panel.transform.position = originalPosition; // ��Ȯ�� ���� ��ġ�� ���� (���� ����)
    }
}