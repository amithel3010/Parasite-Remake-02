using System;
using UnityEngine;

public class DisableColliderOnDeath : MonoBehaviour, IDeathResponse
{
    [SerializeField] private Collider _colliderToDisable;
     private Rigidbody _rbToDisable;

     private void Awake()
     {
         _rbToDisable = GetComponent<Rigidbody>();
     }

     public void OnDeath()
    {
        _rbToDisable.detectCollisions = false;
        _rbToDisable.isKinematic = true;
    }
}
