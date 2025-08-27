using System;

// [System.Serializable]은 이 클래스를 JSON으로 변환할 수 있게 해주는 특성입니다.
[Serializable]
public class PlayerResult
{
    public string nickName;
    public int score;
}

// PlayerResult 리스트를 감싸줄 클래스 (JsonUtility가 리스트를 직접 변환하지 못하기 때문)
[Serializable]
public class LeaderboardData
{
    public System.Collections.Generic.List<PlayerResult> results = new System.Collections.Generic.List<PlayerResult>();
}