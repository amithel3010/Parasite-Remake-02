using System.Collections;
using UnityEngine;

public class ParasitePossessable : Possessable
{
    //the parasite possessable is the default possessable
    //there should be only one of these in the scene at all times
    //after depossessing every other possessable you possess parasite

    //kinda like mario in mario odyssey

    [Header("Possessable Check")]
    [SerializeField] float raycastLength = 1f;
    [SerializeField] LayerMask PossessableLayer;
    [SerializeField] float cooldownTimer = 3f;

    private bool isPossessing = false; //TODO: ask pavel if this is a good way to do it.

    protected override void FixedUpdate() //TODO: ask pavel about fixed update in an abstract class
    {
        if (isPossessing)
        {
            return;
        }
        CheckForPossessables();
        base.FixedUpdate();
    }

    protected override void HandleActionPressInput(IInputSource input)
    {
        if (input.ActionPressed)
        {
            Debug.Log("Action pressed, but can't depossess parasite");
        }
    }

    public override void OnDepossessed()
    {
        Debug.Log("Now Depossessed" + this);
        Destroy(this.gameObject);
        // SetInputSource(null);
        // _rb.isKinematic = true;
        // _rb.detectCollisions = false;
    }

    public override void OnPossessed()
    {
        base.OnPossessed();

        _rb.isKinematic = false;
        _rb.detectCollisions = true;
        
        //StartCoroutine(PossessCooldown(cooldownTimer));
        isPossessing = false;
    }

    private void CheckForPossessables()
    {
        Ray ray = new Ray(transform.position, Vector3.down);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, raycastLength, PossessableLayer))
        {
            Possessable possessableToPossess = hitInfo.collider.GetComponentInParent<Possessable>();
            playerController.Possess(possessableToPossess);
            isPossessing = true;
        }
    }

    private IEnumerator PossessCooldown(float cooldownTimer)
    {
        yield return new WaitForSeconds(cooldownTimer);
        isPossessing = false;
    }

    void OnDrawGizmosSelected()
    {
        Debug.DrawRay(transform.position, Vector3.down * raycastLength);
    }
}
