using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(CompoundTrigger))]
public class CameraArea : MonoBehaviour
{
    [SerializeField] private CameraHolder _cameraForArea;

    private CompoundTrigger _compoundTrigger;

    private void Awake()
    {
        _compoundTrigger = GetComponent<CompoundTrigger>();
    }

    private void OnEnable()
    {
        _compoundTrigger.onEnter += ActivateCamera;
        _compoundTrigger.onExit += MarkForDeactivation;
    }

    private void OnDisable()
    {
        _compoundTrigger.onEnter -= ActivateCamera;
        _compoundTrigger.onExit -= MarkForDeactivation;
    }

    private void ActivateCamera()
    {
        Debug.Log($"Activated {_cameraForArea.name}");
        CameraManager.Instance.ActivateCamera(_cameraForArea);
    }

    private void MarkForDeactivation()
    {
        Debug.Log($"Marked {_cameraForArea.name} for deactivation");
        CameraManager.Instance.MarkCameraForDeactivation(_cameraForArea);
    }
}
