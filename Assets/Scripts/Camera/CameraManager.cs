using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;

    [SerializeField] private GameObject _debugCam;

    [SerializeField] private List<CinemachineCamera> _allActiveCameras = new();

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
        if (_debugCam.activeSelf == true)
        {
            Debug.LogWarning("note that debug cam is active on start. it has priority over other cameras");
        }
    }

    public void RegisterActiveCamera(CinemachineCamera cinemachineCamera)
    {
        _allActiveCameras.Add(cinemachineCamera);
    }

    public void DeRegisterActiveCamera(CinemachineCamera cinemachineCamera)
    {
        if(!_allActiveCameras.Contains(cinemachineCamera))
        {
            Debug.Log("Trying to remove a camera that isnt active from all active cameras list");
        }
        _allActiveCameras.Remove(cinemachineCamera);

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
}
