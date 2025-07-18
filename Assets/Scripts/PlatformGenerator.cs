using UnityEngine;
using System.Collections.Generic;

public class PlatformGenerator : MonoBehaviour
{
    public GameObject cubePrefab;      // ������ ť�� ������
    public int rows = 5;
    public int cols = 10;
    public float spacing = 2.0f;
    public GameObject[,] cubes;        // ������ ť�� ����
    public TagColorizer tagColorizer;  // ���� ������ ������Ʈ ���� (Inspector ����)

    void Start()
    {
        Generate();
    }

    List<Vector2Int> GenerateRandomPath()
    {
        var path = new List<Vector2Int>();
        int currX = 0;
        int currY = Random.Range(0, rows);
        path.Add(new Vector2Int(currX, currY));
        int endY = Random.Range(0, rows);
        Vector2Int goal = new Vector2Int(cols - 1, endY);

        var rnd = new System.Random();

        while (currX < cols - 1 || currY != endY)
        {
            List<Vector2Int> candidates = new List<Vector2Int>();
            if (currX < cols - 1) candidates.Add(new Vector2Int(currX + 1, currY));
            if (currY < rows - 1) candidates.Add(new Vector2Int(currX, currY + 1));
            if (currY > 0) candidates.Add(new Vector2Int(currX, currY - 1));

            // ������ �������� endY�� ������ ������ ��/�Ʒ��� ����
            if (currX == cols - 1)
            {
                if (currY < endY) candidates = new List<Vector2Int> { new Vector2Int(currX, currY + 1) };
                else if (currY > endY) candidates = new List<Vector2Int> { new Vector2Int(currX, currY - 1) };
                else break;
            }

            // ��� �ߺ� ����
            candidates.RemoveAll(c => path.Contains(c));
            if (candidates.Count == 0) break;

            // ��ǥ ������ ����� �ĺ��� �ణ �켱��
            candidates.Sort((a, b) =>
            {
                int da = Mathf.Abs(goal.x - a.x) + Mathf.Abs(goal.y - a.y);
                int db = Mathf.Abs(goal.x - b.x) + Mathf.Abs(goal.y - b.y);
                return rnd.Next(0, 2) + da.CompareTo(db);
            });

            var next = candidates[0];
            currX = next.x;
            currY = next.y;
            path.Add(next);
        }

        // �� �� �̵��� �� ����
        while (currX < cols - 1)
        {
            currX++;
            path.Add(new Vector2Int(currX, currY));
        }
        while (currY != endY)
        {
            currY += (currY < endY) ? 1 : -1;
            path.Add(new Vector2Int(currX, currY));
        }
        return path;
    }

    // ť�� ���� �� �±׸� ����
    public void Generate()
    {
        if (cubes != null)
            foreach (var obj in cubes)
                if (obj) Destroy(obj);

        cubes = new GameObject[cols, rows];
        var path = GenerateRandomPath();
        var pathSet = new HashSet<Vector2Int>(path);

        for (int x = 0; x < cols; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                Vector3 pos = new Vector3(x * spacing, 0, y * spacing);
                var obj = Instantiate(cubePrefab, pos, Quaternion.identity, transform);
                cubes[x, y] = obj;

                Vector2Int cell = new Vector2Int(x, y);

                if (pathSet.Contains(cell))
                    obj.tag = "Untagged";  // ���(�Ķ���)�� �� �±�
                else
                    obj.tag = "Cube";      // ������(������)�� �� �±�
            }
        }

        // --- �ݵ�� ť�� ������ �±� ���� �Ŀ� ���� ������ ���� ---
        if (tagColorizer != null)
            tagColorizer.ApplyTagColors();
    }
}
