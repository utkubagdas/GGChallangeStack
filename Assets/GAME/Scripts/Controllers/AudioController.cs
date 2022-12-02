using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    #region Serialized
    [SerializeField] private AudioSource AudioSource;
    #endregion

    private void OnEnable()
    {
        EventManager.LevelSuccessEvent.AddListener(ResetAudio);
    }

    private void OnDisable()
    {
        EventManager.LevelSuccessEvent.RemoveListener(ResetAudio);
    }

    public void PlayAudio(float diff)
    {
        if (Mathf.Abs(diff) <= 0.15f)
        {
            AudioSource.pitch += 0.1f;
            AudioSource.PlayOneShot(AudioSource.clip);
        }
        else
        {
            AudioSource.Stop();
            AudioSource.pitch = 0.2f;
        }
    }

    private void ResetAudio()
    {
        AudioSource.Stop();
        AudioSource.pitch = 0.2f;
    }
}
