using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(CompoundTrigger))]
public class CameraArea : MonoBehaviour
{
    //TODO: make this script a simple script for camera areas that automatically calls for activate camera and mark for de activation, so it requires almost no setup in inspector.
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
        CameraManager.Instance.ActivateCamera(_cameraForArea);
    }

    private void MarkForDeactivation()
    {
        CameraManager.Instance.MarkCameraForDeactivation(_cameraForArea);
    }
}
