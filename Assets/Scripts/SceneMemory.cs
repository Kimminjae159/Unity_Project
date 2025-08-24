// 파일 이름: SceneMemory.cs

/// <summary>
/// 씬 이동 기록을 저장하는 static 클래스.
/// 게임 내 어디서든 접근 가능합니다.
/// </summary>
public static class SceneMemory
{
    // public static 변수는 게임이 꺼지기 전까지 값이 유지됩니다.
    public static string previousSceneName; // 이전 씬의 이름을 저장할 변수
}