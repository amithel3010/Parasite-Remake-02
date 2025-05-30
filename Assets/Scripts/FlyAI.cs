using System;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.Rendering;

public class FlyAI : MonoBehaviour
{
    [SerializeField] private ControllableManager controllableManager;
    private FlyControllable flyControllable;

    public float MoveAmount = 2f;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        flyControllable = GetComponent<FlyControllable>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!flyControllable.IsControlledByPlayer)
        {
            //flyControllable.HandleAllMovement(Vector3.up * MoveAmount);
        }
    }

    private void GiveControlToPlayer()
    {
        flyControllable.OnPossess();
    }

    private void GiveControlToAI()
    {
        flyControllable.OnDePossess();
    }
}
