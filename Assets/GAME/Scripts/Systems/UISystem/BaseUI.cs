using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

public abstract class BaseUI : MonoBehaviour
{
    [SerializeField] protected RectTransform parentPanel;
    
    public void SetShow()
    {
        parentPanel.gameObject.SetActive(true);
    }

    public void SetHidden()
    {
        parentPanel.gameObject.SetActive(false);
    }
}
