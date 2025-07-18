using Unity.Cinemachine;
using UnityEngine;

[RequireComponent(typeof(CinemachineCamera))]
public class ActiveCameraFlag : MonoBehaviour
{
    //private CinemachineCamera _camera;

    //void Awake()
    //{
    //    _camera = GetComponent<CinemachineCamera>();
    //}

    //void OnEnable()
    //{
    //    if (CameraManager.Instance == null)
    //    {
    //        Debug.LogError("Camera manager is null");
    //    }
    //    CameraManager.Instance.RegisterActiveCamera(_camera);
    //}

    //private void OnDisable()
    //{
    //    CameraManager.Instance.DeRegisterActiveCamera(_camera);
    //}

}
