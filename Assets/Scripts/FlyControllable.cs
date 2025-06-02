
using UnityEditor.Callbacks;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FlyControllable : MonoBehaviour, IControllable
{
    //this class describes the behavior of the fly enemy.
    //it can be controlled by player input via controllable manager,
    //or through AI.
    //TODO: is this overcomplicating things? wouldn't it be easier to fake the behavior of enemies? the way this is set up now ill have to fake PLAYER INPUT which seems hard.

    private Rigidbody _rb;

    public bool IsControlledByPlayer; // im having a hard time understanding where this should change

    [SerializeField] float flightForce = 5f;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>(); //this is the only reason i need mono behavior, is there a better way?
    }

    public void HandleAllMovement(Vector2 MoveInput, bool jumpPressed)
    {
        HorizontalMovement(MoveInput);
        HandleFlight(jumpPressed);
    }

    public void OnDePossess()
    {
        Debug.Log("Depossessed Fly");
        Destroy(this.gameObject);
    }

    public void OnPossess()
    {
        Debug.Log("Possessed Fly");
        //set parasite rb to kinematic

        //set parasite transform.position

        // parasite becomes child of object

        // disable AI
    }

    private void HorizontalMovement(Vector2 moveInput)
    {
        _rb.linearVelocity = new Vector3(moveInput.x, _rb.linearVelocity.y, moveInput.y);
    }

    private void HandleFlight(bool jumpPressed)
    {
        if (jumpPressed)
        {
            _rb.AddForce(Vector3.up * flightForce, ForceMode.Impulse);
        }
    }
    
}
