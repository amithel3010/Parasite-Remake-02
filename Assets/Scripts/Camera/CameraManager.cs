using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;

    [SerializeField] private CinemachineCamera _debugCam;

    [SerializeField] private List<CinemachineCamera> _allCameras = new();

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

    }

    private void Start()
    {
        if (_debugCam.enabled == true)
        {
            Debug.LogWarning("note that debug cam is active on start. it has priority over other cameras");
        }
    }

    public void InitCamera(CinemachineCamera cinemachineCamera)
    {
        _allCameras.Add(cinemachineCamera);
    }

    public void ChangeAllCamerasTarget(Transform newTarget)
    {
        foreach (var camera in _allCameras)
        {
            camera.Target.TrackingTarget = newTarget;
        }
    }

    public void ToggleDebugCamera() 
    {
        _debugCam.enabled = !_debugCam.enabled;
    }
}
