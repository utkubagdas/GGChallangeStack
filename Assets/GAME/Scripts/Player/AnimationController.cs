using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private void OnEnable()
    {
        EventManager.LevelRedesignEvent.AddListener(() => SetTriggerAnimation(Consts.AnimatorKeywords.IDLE));
    }

    private void OnDisable()
    {
        EventManager.LevelRedesignEvent.RemoveListener(() => SetTriggerAnimation(Consts.AnimatorKeywords.IDLE));
    }

    public void SetTriggerAnimation(string animID)
    {
        animator.SetTrigger(animID);
    }
}
