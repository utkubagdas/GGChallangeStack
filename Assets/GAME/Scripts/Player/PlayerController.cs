using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerFacade playerFacade;
    private LevelFacade levelFacade;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FinishLine"))
        {
            FinishCamAdjustments();
            playerFacade.AnimationController.SetTriggerAnimation("Dance");
            InvokeDelayEvent(3f, EventManager.LevelSuccessEvent);
        }

        if (other.CompareTag("Fall"))
        {
            EventManager.LevelFailEvent.Invoke();
        }
    }

    private void FinishCamAdjustments()
    {
        var sucCam = ControllerHub.Get<CameraManager>().successCam;
        var camRotator = sucCam.GetComponentInChildren<CamRotator>();
        Transform transform1;
        (transform1 = camRotator.transform).SetParent(null);
        sucCam.transform.SetParent(transform1);
        playerFacade.PlayerMovementController.SetControlable(false);
        ControllerHub.Get<CameraManager>().SelectSuccessCam();
        ControllerHub.Get<CameraManager>().successCam.Follow = null;
        ControllerHub.Get<CameraManager>().successCam.m_LookAt = null;
        camRotator.transform.position = transform.position;
        sucCam.transform.rotation = Quaternion.Euler(new Vector3(0, -180, 0));
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
    }
}
