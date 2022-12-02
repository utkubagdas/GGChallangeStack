using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    #region Serialized
    [SerializeField] private AudioSource AudioSource;
    #endregion

    public void PlayAudio(float diff)
    {
        if (diff <= 0.15f && diff >= -0.15f)
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
}
