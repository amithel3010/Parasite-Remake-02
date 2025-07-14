using Unity.Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;

    [SerializeField] private CinemachineCamera _debugCam;
    [SerializeField] private CinemachineCamera _inGameCam;

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

        if (_debugCam == null || _inGameCam == null)
        {
            Debug.LogError("Camera Manager is missing references!!!");
        }

    }

    private void Start()
    {
        //safety check if both cameras are the same state
        if ((_debugCam.enabled && _inGameCam.enabled) || (!_debugCam.enabled && !_inGameCam.enabled))
        {
            Debug.LogWarning("Both cameras were enabled or sidabled on start. defaulting to ingame camera");
            _inGameCam.enabled = true;
            _debugCam.enabled = false;
        }
    }


    public void ChangeCameraTarget(Transform newTarget)
    {
        _debugCam.Target.TrackingTarget = newTarget;
        _inGameCam.Target.TrackingTarget = newTarget;
    }

    public void ToggleActiveCamera() //TODO: later when i have a bunch of cameras this won't do. probably need an enum. also this is super breakable if both cameras are enabled on start
    {
        _debugCam.enabled = !_debugCam.enabled;
        _inGameCam.enabled = !_inGameCam.enabled;
    }
}
