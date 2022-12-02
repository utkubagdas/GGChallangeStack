using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Controller
{
    #region Property

    public bool levelStarted;
    #endregion

    private void Update()
    {
        if (!levelStarted && Input.GetMouseButtonDown(0))
        {
            levelStarted = true;
            EventManager.LevelStartEvent.Invoke();
        }
    }

    public void SetLevelStarted(bool started)
    {
        levelStarted = started;
    }
}
