using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class BaseLevelController : Controller
{
    #region Serialized
    [SerializeField] protected LevelContent[] allLevels;
    [SerializeField] protected LevelContent[] levelsToRepeat;
    #endregion
    
    #region Property
    public LevelContent LevelContent { get; private set; }
    #endregion
    
    #region Local
    private int levelNo = 1;
    private int currentLevelIndex;
    #endregion



    [SerializeField] private Transform levelParent;

    private readonly List<Object> destroyOnResetList = new List<Object>();


    protected virtual void OnEnable()
    {
        EventManager.LevelSuccessEvent.AddListener(OnLevelSuccess);
    }

    protected virtual void OnDisable()
    {
        EventManager.LevelSuccessEvent.RemoveListener(OnLevelSuccess);
    }

    public override void Init()
    {
        base.Init();

        LoadLevel();
    }

    protected virtual void LoadLevel()
    {
        LevelContent = GetLevelContent();
    }

    private LevelContent GetLevelContent()
    {
        if (levelNo - 1 < allLevels.Length)
        {
            return allLevels[currentLevelIndex];
        }
        return levelsToRepeat[currentLevelIndex];
    }
    
    private void IncreaseLevelNo()
    {
        
        
        if (levelNo >= allLevels.Length)
        {
            //if the levels are over
            
            //set random level
            // int randomLvlIndex = Random.Range(0, levelsToRepeat.Length);
            // currentLevelIndex = randomLvlIndex;
            
            //or back to first level
            currentLevelIndex = 0;
            levelNo = 1;
        }
        else
        {
            currentLevelIndex++;
            levelNo++;
        }
    }


    protected T InstantiateAsDestroyable<T>(Object obj) where T : Object
    {
        T t = Instantiate(obj, levelParent) as T;
        destroyOnResetList.Add(t);
        return t;
    }

    private void OnLevelSuccess()
    {
        IncreaseLevelNo();
    }

    protected void SendLevelLoadedEvent(LevelFacade facade)
    {
        EventManager.LevelLoadedEvent.Invoke(new LevelLoadedEventData(
            LevelContent,
            facade,
            levelNo));
    }
    
    public void RestartLevel()
    {
        ResetLevel();
        EventManager.LevelResetEvent.Invoke();
        LoadLevel();
    }

    public void RedesignLevel()
    {
        //LevelContent.LevelFacade.PlatformController.RedesignLevel();
        //EventManager.LevelRedesignEvent.Invoke();
    }

    private void ResetLevel()
    {
        foreach (Object obj in destroyOnResetList)
        {
            switch (obj)
            {
                case GameObject go:
                    Destroy(go);
                    break;
                case Component component:
                    Destroy(component.gameObject);
                    break;
            }
        }
        destroyOnResetList.Clear();
    }
}
