using System;
using System.Collections.Generic;
using UnityEngine;

public class ControllerHub : PersistentSingleton<ControllerHub>
{

    [SerializeField]
    private List<Controller> controllerList;
    

    private readonly Dictionary<Type, Controller> controllers = new Dictionary<Type, Controller>();

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