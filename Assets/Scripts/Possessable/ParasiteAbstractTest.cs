using UnityEngine;

public class ParasiteAbstractTest : Possessable
{
    [SerializeField] PlayerController playerController;

    [Header("Possessable Check")]
    [SerializeField] float raycastLength = 1f;
    [SerializeField] LayerMask PossessableLayer;

    private bool isPossessing = false; //TODO: ask pavel if this is a good way to do it.

    protected override void FixedUpdate() //TODO: ask pavel about fixed update in an abstract class
    {
        CheckForPossessables();
        if (isPossessing)
        {
            return;
        }
        base.FixedUpdate();
    }

    protected override void HandleActionInput(IInputSource input)
    {
        if (input.ActionPressed)
        {
            Debug.Log("Action pressed");
        }
    }

    public override void OnDepossessed()
    {
        base.OnDepossessed();
        _rb.isKinematic = true;
    }

    public override void OnPossessed()
    {
        base.OnPossessed();
        isPossessing = false;
    }

    private void CheckForPossessables()
    {
        Ray ray = new Ray(transform.position, Vector3.down);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, raycastLength, PossessableLayer))
        {
            print(hitInfo.collider.gameObject.name);
            
            Possessable possessableToPossess = hitInfo.collider.GetComponent<Possessable>();
            playerController.Possess(possessableToPossess);
            isPossessing = true;
        }
    }

    void OnDrawGizmosSelected()
    {
        Debug.DrawRay(transform.position, Vector3.down * raycastLength);
    }
}
