static public class GameEvents
{
    static public System.Action OnGameStarted;
    static public System.Action OnGameEnded;
    static public System.Action OnLineCleared;

    static public void RaiseOnGameStarted() => OnGameStarted?.Invoke();
    static public void RaiseOnGameEnded() => OnGameEnded?.Invoke();
    static public void RaiseOnLineCleared() => OnLineCleared?.Invoke();
}
