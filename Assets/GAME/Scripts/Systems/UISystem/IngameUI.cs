public class IngameUI : BaseUI
{
    private void OnEnable()
    {
        EventManager.LevelLoadedEvent.AddListener(OnLevelLoaded);
        EventManager.LevelFailEvent.AddListener(OnLevelFail);
        EventManager.LevelSuccessEvent.AddListener(OnLevelSuccess);
    }

    private void OnDisable()
    {
        EventManager.LevelLoadedEvent.RemoveListener(OnLevelLoaded);
        EventManager.LevelFailEvent.RemoveListener(OnLevelFail);
        EventManager.LevelSuccessEvent.RemoveListener(OnLevelSuccess);
    }

    private void OnLevelLoaded(LevelLoadedEventData eventData)
    {
        SetShow();
    }

    private void OnLevelFail()
    {
        SetHidden();
    }

    private void OnLevelSuccess()
    {
        SetHidden();
    }
}
