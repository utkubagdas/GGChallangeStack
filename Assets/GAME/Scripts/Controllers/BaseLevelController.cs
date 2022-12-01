using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class BaseLevelController : Controller
{
    #region Levels
    [SerializeField] protected LevelContent[] allLevels;
    [SerializeField] protected LevelContent[] levelsToRepeat;
    public LevelContent LevelContent { get; private set; }
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
