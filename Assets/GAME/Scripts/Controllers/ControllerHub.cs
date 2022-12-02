using System;
using System.Collections.Generic;
using UnityEngine;

public class ControllerHub : PersistentSingleton<ControllerHub>
{
    #region Serialized
    [SerializeField] private List<Controller> controllerList;
    #endregion
    
    #region Local
    private readonly Dictionary<Type, Controller> controllers = new Dictionary<Type, Controller>();
    #endregion
    
    private void Start()
    {
        PopulateDictionary();
        InitControllers();
    }

    private void PopulateDictionary()
    {
        foreach (var controller in controllerList)
        {
            controllers.Add(controller.GetType(), controller);
        }
    }

    private void InitControllers()
    {
        foreach (var controller in controllers.Values)
        {
            controller.Init();
        }
    }

    public static T Get<T>() where T : Controller
    {
        return (T)Instance.controllers[typeof(T)];
    }

}