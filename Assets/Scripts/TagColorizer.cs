using UnityEngine;

public class TagColorizer : MonoBehaviour
{
    public PlatformGenerator platformGenerator; // PlatformGenerator를 Inspector에서 드래그로 연결
    public Material cubeMat;         // "Cube" 태그(빨간)용
    public Material untaggedMat;     // "Untagged" 태그(파랑)용

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

                if (obj.tag == "Cube" && cubeMat != null)
                    renderer.material = cubeMat;
                else if (obj.tag == "Untagged" && untaggedMat != null)
                    renderer.material = untaggedMat;
            }
        }
    }
}
