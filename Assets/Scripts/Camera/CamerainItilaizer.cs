using Unity.Cinemachine;
using UnityEngine;

[RequireComponent(typeof(CinemachineCamera))]
public class CamerainItilaizer : MonoBehaviour
{
    private CinemachineCamera _camera;

    void Awake()
    {
        _camera = GetComponent<CinemachineCamera>();
    }

    void OnEnable()
    {
        if (CameraManager.Instance == null)
        {
            Debug.LogError("Camera manager is null");
        }
        CameraManager.Instance.InitCamera(_camera);
    }

    public void SetActive(bool IsActive)
    {
        _camera.enabled = IsActive;
    }

}
