using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;

    [SerializeField] private GameObject _debugCam;

    [SerializeField] private CinemachineCamera[] _allActiveCameras;

    public CinemachineCamera CurrentLiveCamera { get; private set; }

    private CinemachineBrain _brain;
    private ICinemachineCamera _lastCamera;


    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("More than 1 CameraManager instance");
        }
        else
        {
            Instance = this;
        }

        if (_debugCam == null)
        {
            Debug.LogError("did not set debug cam");
        }

        _brain = FindAnyObjectByType<CinemachineBrain>();

    }

    private void Update()
    {
        var currentCam = _brain.ActiveVirtualCamera;
        if (currentCam != _lastCamera)
        {
            Debug.Log($"Switched to: {currentCam.Name}");
            _lastCamera = currentCam;
        }
    }

    private void Start()
    {
        if (_debugCam.activeSelf == true)
        {
            Debug.LogWarning("note that debug cam is active on start. it has priority over other cameras");
        }

    }


    public void ChangeActiveCamerasTarget(Transform newTarget)
    {
        foreach (var camera in _allActiveCameras)
        {
            camera.Target.TrackingTarget = newTarget;
        }
    }

    public void ToggleDebugCamera()
    {
        //according to cinemachine docs, the correct way to switch cameras is disabling and enabling the whole game object
        _debugCam.SetActive(!_debugCam.activeSelf);
    }

    void OnCameraChanged(ICinemachineCamera newCam, ICinemachineCamera prevCam)
    {
        CurrentLiveCamera = newCam as CinemachineCamera;
    }

    [ContextMenu("Get All Cinemachine Cameras In Scene")]
    private void GetAllCinemachineCamerasInScene()
    {
        _allActiveCameras = FindObjectsByType<CinemachineCamera>(FindObjectsSortMode.None);
    }
}
