// IDialogueAction.cs

/// <summary>
/// 대화가 종료된 후 실행될 모든 동작 스크립트가 따라야 할 인터페이스(규칙)입니다.
/// </summary>
public interface IDialogueAction
{
    /// <summary>
    /// 이 함수를 구현하여 대화 종료 후의 실제 동작을 정의합니다.
    /// </summary>
    void Apply();
}