using System.Collections;
using UnityEngine;

public class ParasitePossessable : Possessable
{
    //the parasite possessable is the default possessable
    //there should be only one of these in the scene at all times
    //after depossessing every other possessable you possess parasite

    //kinda like mario in mario odyssey

    //Parastie should be able to move slowly and Jump once, has 1HP
    //TODO: where should HP be implemented?

    [Header("Possessable Check")]
    [SerializeField] float raycastLength = 1f;
    [SerializeField] LayerMask PossessableLayer;
    [SerializeField] float cooldownTimer = 3f;

    private bool isPossessing = false;

    protected override void FixedUpdate() //TODO: ask pavel about fixed update in an abstract class
    {
        if (isPossessing)
            return;

        CheckForPossessablesAndPossess();
        base.FixedUpdate();
    }

    protected override void HandleActionPressInput(IInputSource input)
    {
        if (input.ActionPressed)
        {
            Debug.Log("Action pressed, but can't depossess parasite");
        }
    }

    public override void OnPossessed()
    {
        base.OnPossessed();
        isPossessing = false;
    }

    private void CheckForPossessablesAndPossess()
    {
        Ray ray = new Ray(transform.position, Vector3.down);

        //if found possessable, possess it
        if (Physics.Raycast(ray, out RaycastHit hitInfo, raycastLength, PossessableLayer))
        {
            Possessable possessableToPossess = hitInfo.collider.GetComponentInParent<Possessable>();
            playerController.Possess(possessableToPossess);
            isPossessing = true;
        }
    }

    void OnDrawGizmosSelected()
    {
        Debug.DrawRay(transform.position, Vector3.down * raycastLength);
    }
}
