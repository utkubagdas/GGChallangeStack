using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Singleton
    private static GameManager _instance;
    public static GameManager Instance => _instance;
    #endregion
    
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
