using UnityEngine.Events;

public static class EventManager
{
    #region GameCycleEvents
    public static readonly UnityEvent LevelStartEvent = new UnityEvent();
    #endregion
}