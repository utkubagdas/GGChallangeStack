using UnityEngine;

public class LevelController : BaseLevelController
{
    #region Property
    public LevelFacade LevelFacade => levelFacade;
    #endregion

    #region Local
    private LevelFacade levelFacade;
    #endregion
    protected override void LoadLevel()
    {
        base.LoadLevel();
        PrepareLevel();
    }
	
    private void PrepareLevel()
    {
        levelFacade = InstantiateAsDestroyable<LevelFacade>(LevelContent.LevelFacade);
        Transform target = levelFacade.Player.transform;
        if (target != null)
        {
            ControllerHub.Get<CameraManager>().Init(target);
        }
        SendLevelLoadedEvent(levelFacade);
    }
}