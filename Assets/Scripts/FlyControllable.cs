
using UnityEditor.Callbacks;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FlyControllable : MonoBehaviour, IControllable
{
    private FlyAI AI;
    private ControllableManager controllableManager;

    private Rigidbody _rb;

    public bool IsControlledByPlayer; // im having a hard time understanding where this should change
    public Transform TargetToRotateTo; // should be set in the AI and the controllable manager... or maybe I should have logic in here to detect if AI is controlling or Player?
    //[SerializeField] LayerMask PlayerLayer;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>(); //this is the only reason i need mono behavior, is there a better way?
    }

    public void HandleAllMovement(Vector2 MoveInput, bool jumpPressed)
    {
        HorizontalMovement(MoveInput);
        RotateTowardsTarget(TargetToRotateTo);
    }

    public void OnDePossess()
    {
        Destroy(this.gameObject);
    }

    public void OnPossess()
    {
        //set parasite rb to kinematic

        //set parasite transform.position

        // parasite becomes child of object

        // disable AI
    }

    private void RotateTowardsTarget(Transform target)
    {
        //this is an AI only movement 

        if (IsControlledByPlayer)
            return;

        Vector3 targetDirection = (target.position - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        _rb.MoveRotation(targetRotation);
    }

    private void HorizontalMovement(Vector2 moveInput)
    {
        _rb.linearVelocity = new Vector3(moveInput.x, _rb.linearVelocity.y, moveInput.y);
    }

    private void HandleFlight(bool jumpPressedInput)
    {
        //TODO: Implement flight
    }
    
}
