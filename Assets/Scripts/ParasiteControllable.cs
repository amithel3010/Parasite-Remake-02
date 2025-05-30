using UnityEditor.Callbacks;
using UnityEngine;

public class ParasiteControllable : MonoBehaviour, IControllable
{
    //this script, in addition to controlling all parasite movement
    //should also be responsible for possessing 

    private Rigidbody _rb;
    private IControllable nextControllable;

    [SerializeField] float moveSpeed = 2f;
    [SerializeField] float jumpForce = 3f;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void OnCollisionEnter(Collision collision)
    {
        //take over the collision object 

        nextControllable = collision.gameObject.GetComponent<IControllable>();
        if (nextControllable != null)
        {
            
        }
    }

    public void HandleAllMovement(Vector2 MoveInput, bool jumpPressed)
    {
        HorizontalMovement(MoveInput);
        Jump(jumpPressed);
    }

    public void OnDePossess()
    {
        //Cannot DePossess, this is the default controller.
    }

    public void OnPossess()
    {
        //cannot possess
    }

    private void HorizontalMovement(Vector2 MoveInput)
    {
        Vector3 moveDir = new Vector3(MoveInput.x, 0, MoveInput.y);
        _rb.linearVelocity = moveDir * moveSpeed; //TODO: change this so it doesnt zero Y vel every frame
    }

    private void Jump(bool jumpPressed)
    {
        if (jumpPressed)
        {
            _rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void Possess()
    {

    }
}
