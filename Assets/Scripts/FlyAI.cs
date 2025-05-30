using System;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.Rendering;

public class FlyAI : MonoBehaviour
{
    [SerializeField] private ControllableManager controllableManager;
    private FlyControllable flyControllable;

    private bool isControlledByPlayer = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        flyControllable = GetComponent<FlyControllable>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isControlledByPlayer)
        {
            flyControllable.HandleAllMovement();
        }
    }

    private void GiveControlToPlayer()
    {
        flyControllable.OnPossess();
        isControlledByPlayer = true;
    }

    private void GiveControlToAI()
    {
        flyControllable.OnDePossess();
        isControlledByPlayer = false;
    }
}
