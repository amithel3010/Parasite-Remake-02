using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    //TODO: this whole script fells very un optimized

    //TODO: Because this is a static class anything it references is not deleted! this is what causes the leak

    [SerializeField] private GameObject _debugCam;
    [SerializeField] private GameObject _firstActiveCamera;

    private CameraHolder _currentCameraMarkedForDeactivation; //TODO: not clean

    private CinemachineBrain _brain;

    public static CameraManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogError($"More than one instance of CameraManager found in {name}");
            Destroy(gameObject);
            return;
        }

        Instance = this;

        _brain = FindAnyObjectByType<CinemachineBrain>();
    }


    private void Start()
    {
        if (_debugCam.activeSelf == true)
        {
            Debug.LogWarning("note that debug cam is active on start. it has priority over other cameras");
        }
    }

    public void SetAllCameraTargets(Transform newTarget)
    {
        var cams = FindAllCameras();

        foreach (var cam in cams)
        {
            if (cam != null)
            {
                cam.Target.TrackingTarget = newTarget;
            }
        }
    }


    public void ActivateCamera(CameraHolder cameraHolderToActivate)
    {
        if (cameraHolderToActivate == null)
        {
            Debug.LogError($"cameraHolderToActivate is null");
            return;
        }

        if (cameraHolderToActivate.MarkedForDeactivation)
        {
            cameraHolderToActivate.SetMarkedForDeactivation(false);
            _currentCameraMarkedForDeactivation = null;
            return;
        }

        cameraHolderToActivate.gameObject.SetActive(true);

        if (_currentCameraMarkedForDeactivation != null)
            TryToDeactivateCamera(_currentCameraMarkedForDeactivation);
        else
        {
            Debug.Log($"_currentCameraMarkedForDeactivation is null");
        }

        //disable all other active cameras.
        //might be a way to get only active ones for performance
        var holders = FindCameraHolders();
        foreach (var holder in holders)
        {
            if (holder != cameraHolderToActivate && holder.gameObject.activeSelf)
            {
                holder.gameObject.SetActive(false);
            }
        }
    }

    private IEnumerable<CameraHolder> FindCameraHolders()
    {
        //TODO: change to use camera holder script
        var cams = FindAllCameras();
        foreach (var cam in cams)
        {
            var holder = cam.transform.parent?.gameObject;
            if (holder != null)
            {
                if (holder.TryGetComponent(out CameraHolder cameraHolder))
                    yield return cameraHolder; //should be a better way 
            }
        }
    }

    public void MarkCameraForDeactivation(CameraHolder cameraHolderToDeactivate)
    {
        cameraHolderToDeactivate.SetMarkedForDeactivation(true);
        _currentCameraMarkedForDeactivation = cameraHolderToDeactivate;
    }


    private void TryToDeactivateCamera(CameraHolder cameraHolderToDeactivate)
    {
        Debug.Log($"Trying to deactivate camera {cameraHolderToDeactivate.gameObject.name}");

        if (!cameraHolderToDeactivate.MarkedForDeactivation)
        {
            Debug.Log("cameraholder to deactivate is not marked  for deactivation");
            return;
        }

        if (!OtherValidCameraExists(cameraHolderToDeactivate))
        {
            Debug.LogWarning($"Tried to deactivate {cameraHolderToDeactivate}, but no other cameras are active.");
            return;
        }

        cameraHolderToDeactivate.gameObject.SetActive(false);
        Debug.Log($"Deactivated {cameraHolderToDeactivate}");
        _currentCameraMarkedForDeactivation.SetMarkedForDeactivation(false);
        _currentCameraMarkedForDeactivation = null;
    }

    private bool OtherValidCameraExists(CameraHolder toDeactivate)
    {
        foreach (var holder in FindCameraHolders())
        {
            if (holder == toDeactivate) continue;
            if (holder.gameObject.activeInHierarchy) return true; // Something else will take over
        }

        return false; // Nothing else valid to take over
    }

    public void ToggleDebugCamera()
    {
        //according to cinemachine docs, the correct way to switch cameras is disabling and enabling the whole game object
        _debugCam.SetActive(!_debugCam.activeSelf);
    }


    private CinemachineCamera[] FindAllCameras() =>
        FindObjectsByType<CinemachineCamera>(FindObjectsInactive.Include, FindObjectsSortMode.None);


    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }
}