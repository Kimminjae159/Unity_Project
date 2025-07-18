using UnityEngine;
using System.Collections.Generic;

public class PlatformGenerator : MonoBehaviour
{
    public GameObject cubePrefab;      // 생성할 큐브 프리팹
    public int rows = 5;
    public int cols = 10;
    public float spacing = 2.0f;
    public GameObject[,] cubes;        // 생성된 큐브 저장
    public TagColorizer tagColorizer;  // 색상 입히기 컴포넌트 참조 (Inspector 연결)

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

            // 마지막 열에서는 endY에 도달할 때까지 위/아래만 가능
            if (currX == cols - 1)
            {
                if (currY < endY) candidates = new List<Vector2Int> { new Vector2Int(currX, currY + 1) };
                else if (currY > endY) candidates = new List<Vector2Int> { new Vector2Int(currX, currY - 1) };
                else break;
            }

            // 경로 중복 방지
            candidates.RemoveAll(c => path.Contains(c));
            if (candidates.Count == 0) break;

            // 목표 지점에 가까운 후보를 약간 우선시
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

        // 끝 열 미도달 시 보정
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

    // 큐브 생성 및 태그만 지정
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
                    obj.tag = "Untagged";  // 경로(파란색)가 될 태그
                else
                    obj.tag = "Cube";      // 나머지(빨간색)가 될 태그
            }
        }

        // --- 반드시 큐브 생성과 태그 지정 후에 색상 입히기 실행 ---
        if (tagColorizer != null)
            tagColorizer.ApplyTagColors();
    }
}
