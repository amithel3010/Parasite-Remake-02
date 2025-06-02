using UnityEngine;

public class ParasiteAbstractTest : Possessable
{
    [Header("Possessable Check")]
    [SerializeField] float raycastLength = 1f;
    [SerializeField] LayerMask PossessableLayer;

    private bool isPossessing = false; //TODO: ask pavel if this is a good way to do it.

    protected void OnEnable()
    {
        playerController.Possess(this);
    }

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
        base.OnDepossessed();
        _rb.isKinematic = true;
    }

    public override void OnPossessed()
    {
        base.OnPossessed();
        _rb.isKinematic = false;
        isPossessing = false;
    }

    private void CheckForPossessables()
    {
        Ray ray = new Ray(transform.position, Vector3.down);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, raycastLength, PossessableLayer))
        {   
            Possessable possessableToPossess = hitInfo.collider.GetComponentInParent<Possessable>();
            possessableToPossess.CacheParasitePossessable(this);
            playerController.Possess(possessableToPossess);
            isPossessing = true;
        }
    }

    void OnDrawGizmosSelected()
    {
        Debug.DrawRay(transform.position, Vector3.down * raycastLength);
    }
}
