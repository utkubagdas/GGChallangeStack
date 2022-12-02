using System;
using System.Collections;
using Cinemachine;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerFacade playerFacade;
    [SerializeField] private CamRotator camRotator;
    private LevelFacade levelFacade;
    private Transform successCamOldTransform;
    private CinemachineVirtualCamera successCam;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FinishLine"))
        {
            FinishCamAdjustments();
            playerFacade.AnimationController.SetTriggerAnimation("Dance");
            InvokeDelayEvent(5f, EventManager.LevelSuccessEvent);
        }

        if (other.CompareTag("Fall"))
        {
            EventManager.LevelFailEvent.Invoke();
        }
    }

    private void FinishCamAdjustments()
    {
        successCam = ControllerHub.Get<CameraManager>().successCam;
        successCamOldTransform = successCam.transform.parent;
        playerFacade.PlayerMovementController.SetControlable(false);
        ControllerHub.Get<CameraManager>().SelectSuccessCam();
        ControllerHub.Get<CameraManager>().successCam.Follow = null;
        ControllerHub.Get<CameraManager>().successCam.m_LookAt = null;
        successCam.transform.SetParent(camRotator.transform);
        camRotator.SetCanRotate(true);
    }

    private void InvokeDelayEvent(float delayTime, UnityEvent successEvent)
    {
        StartCoroutine(InvokeDelayEventCo(delayTime, successEvent));
    }

    private IEnumerator InvokeDelayEventCo(float delayTime, UnityEvent successEvent)
    {
        yield return new WaitForSeconds(delayTime);
        successEvent.Invoke();
        ResetCamSettings();
    }

    private void ResetCamSettings()
    {
        successCam.transform.SetParent(successCamOldTransform);
        successCam.Follow = transform;
        successCam.transform.rotation = Quaternion.Euler(Vector3.zero);
        camRotator.SetCanRotate(false);
        camRotator.transform.rotation = Quaternion.Euler(Vector3.zero);
    }
}
