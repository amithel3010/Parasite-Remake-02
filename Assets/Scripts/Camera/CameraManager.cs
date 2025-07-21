using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
#if UNITY_EDITOR
#endif

public class CameraManager : MonoBehaviour
{

    //TODO: this whole script fells very un optimized

    public static CameraManager Instance;

    [SerializeField] private GameObject _debugCam;

    [SerializeField] private CinemachineCamera[] _allCameras;
    [SerializeField] private GameObject[] _allCameraHolders;

    private CinemachineBrain _brain;

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

    private void Start()
    {
        if (_debugCam.activeSelf == true)
        {
            Debug.LogWarning("note that debug cam is active on start. it has priority over other cameras");
        }

    }

    public void ChangeActiveCamerasTarget(Transform newTarget)
    {
        foreach (var camera in _allCameras)
        {
            camera.Target.TrackingTarget = newTarget;
        }
    }

    public void ActivateCamera(GameObject cameraToActivate)
    {
        cameraToActivate.SetActive(true);

        foreach (var cameraHolder in _allCameraHolders)
        {
            if (cameraHolder == cameraToActivate)
                continue;

            if(cameraHolder.activeSelf == true)
            {
                cameraHolder.SetActive(false);
            }
        }
    }

    public void TryToDeactivateCamera(GameObject cameraToDeactivate)
    {
        if (!OtherValidCameraExists(cameraToDeactivate))
        {
            Debug.LogWarning($"Tried to deactivate {cameraToDeactivate}, but if it will be deactivated there would be no cameras to switch to");
            return;
        }
        cameraToDeactivate.SetActive(false);

    }

    private bool OtherValidCameraExists(GameObject toDeactivate)
    {
        if (toDeactivate == null || _brain == null)
            return true;

        foreach (var camHolder in _allCameraHolders)
        {
            if (camHolder == null || camHolder.gameObject == toDeactivate)
                continue;

            if (camHolder.activeInHierarchy)
                return true; // Something else will take over
        }

        return false; // Nothing else valid to take over
    }

    public void ToggleDebugCamera()
    {
        //according to cinemachine docs, the correct way to switch cameras is disabling and enabling the whole game object
        _debugCam.SetActive(!_debugCam.activeSelf);
    }

    [ContextMenu("Get All Cinemachine Cameras In Scene")]
    private void GetAllCinemachineCamerasInScene()
    {
        _allCameras = FindObjectsByType<CinemachineCamera>(FindObjectsInactive.Include, FindObjectsSortMode.None);
    }

    [ContextMenu("Get CameraHolders In Scene")]
    private void GetAllCameraHoldersInScene()
    {

        if (_allCameras == null || _allCameras.Length == 0)
        {
            Debug.LogWarning("No cameras found. Run 'Get All Cinemachine Cameras In Scene' first.");
            return;
        }

        var holders = new List<GameObject>();

        foreach (var cam in _allCameras)
        {
            if (cam != null && cam.transform.parent != null && cam.gameObject != _debugCam)
            {
                holders.Add(cam.transform.parent.gameObject);
            }
        }

        _allCameraHolders = holders.ToArray();
    }

    public void EnableAllCameraHolders()
    {
        foreach (var holder in _allCameraHolders)
            if (holder != null)
                holder.SetActive(true);
    }

    public void DisableAllCameraHolders()
    {
        foreach (var holder in _allCameraHolders)
            if (holder != null)
                holder.SetActive(false);
    }
}
