using Cinemachine;
using UnityEngine;

public class CameraManager : Controller
{
    #region Public
    public CinemachineVirtualCamera gameplayCam;
    public CinemachineVirtualCamera successCam;
    public CinemachineVirtualCamera failCam;
    #endregion

    #region Local
    private LevelController _levelController;
    private Transform _targetTransform;
    private float _shakeTimer;
    private CinemachineBasicMultiChannelPerlin _cinemachineBasicMultiChannelPerlin;
    #endregion

    private void OnEnable()
    {
        EventManager.LevelLoadedEvent.AddListener(SelectGameplayCam);
        EventManager.LevelSuccessEvent.AddListener(SelectSuccessCam);
        EventManager.LevelFailEvent.AddListener(SelectFailCam);
    }

    private void OnDisable()
    {
        EventManager.LevelLoadedEvent.RemoveListener(SelectGameplayCam);
        EventManager.LevelSuccessEvent.RemoveListener(SelectSuccessCam);
        EventManager.LevelFailEvent.RemoveListener(SelectFailCam);
    }

    private void SelectGameplayCam(LevelLoadedEventData arg0)
    {
        SelectGameplayCam();
    }

    public void Init(Transform target)
    {
        _targetTransform = target;

        var transform1 = _targetTransform.transform;
        successCam.Follow = transform1;
        //successCam.LookAt = transform1;
        gameplayCam.Follow = transform1;
        failCam.Follow = transform1;
        failCam.LookAt = transform1;

        SelectGameplayCam();
    }

    public void SelectGameplayCam()
    {
        gameplayCam.Priority = 11;
        failCam.Priority = 10;
        successCam.Priority = 10;

    }

    public void SelectFailCam()
    {
        failCam.Priority = 11;
        gameplayCam.Priority = 10;
        successCam.Priority = 10;
    }

    public void SelectSuccessCam()
    {
        gameplayCam.Priority = 10;
        failCam.Priority = 9;
        successCam.Priority = 12;
    }
}
