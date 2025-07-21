using UnityEngine;

public class TagColorizer : MonoBehaviour
{
    public PlatformGenerator platformGenerator; // PlatformGenerator�� Inspector���� �巡�׷� ����
    public Material wrongMat;         // "Wrong" �±�(����)��
    public Material correctMat;     // "Correct" �±�(�Ķ�)��

    public void ApplyTagColors()
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
                if (!obj) continue;
                var renderer = obj.GetComponent<Renderer>();
                if (!renderer) continue;

                if (obj.tag == "Wrong" && wrongMat != null)
                    renderer.material = wrongMat;
                else if (obj.tag == "Correct" && correctMat != null)
                    renderer.material = correctMat;
            }
        }
    }
}
