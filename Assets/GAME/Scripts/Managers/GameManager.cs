using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Controller
{
    #region Local
    private bool levelStarted;
    #endregion

    private void Update()
    {
        if (!levelStarted && Input.GetMouseButtonDown(0))
        {
            levelStarted = true;
            EventManager.LevelStartEvent.Invoke();
        }
    }
}
