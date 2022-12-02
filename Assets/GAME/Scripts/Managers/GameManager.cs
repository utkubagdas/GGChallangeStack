using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Controller
{
    #region Property
    public bool levelStarted { get; private set; }
    public bool stageStarted { get; private set; }
    public bool onStageStart { get; private set; }
    public bool levelFinished { get; private set; }
    #endregion

    private void OnEnable()
    {
        EventManager.LevelRedesignEvent.AddListener(() => SetStageStart(true));
        EventManager.LevelSuccessEvent.AddListener(SetLevelFinish);
    }
    
    private void OnDisable()
    {
        EventManager.LevelRedesignEvent.RemoveListener(() => SetStageStart(true));
        EventManager.LevelSuccessEvent.RemoveListener(SetLevelFinish);
    }

    private void Update()
    {
        if (!levelStarted && Input.GetMouseButtonDown(0))
        {
            levelStarted = true;
            levelFinished = false;
            EventManager.LevelStartEvent.Invoke();
        }

        if (levelStarted && !stageStarted && onStageStart && Input.GetMouseButtonDown(0))
        {
            onStageStart = false;
            stageStarted = true;
            levelFinished = false;
            EventManager.StageStartEvent.Invoke();
        }
    }

    public void SetLevelStarted(bool started)
    {
        levelStarted = started;
    }

    public void SetStageStart(bool isActive)
    {
        onStageStart = isActive;
        stageStarted = !isActive;
    }

    public void SetLevelFinish()
    {
        levelFinished = true;
    }
}
