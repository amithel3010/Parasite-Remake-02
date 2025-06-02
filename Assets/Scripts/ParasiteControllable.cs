using UnityEditor.Callbacks;
using UnityEngine;

public class ParasiteControllable : MonoBehaviour, IControllable
{
    //this script, in addition to controlling all parasite movement
    //should also be responsible for possessing 

    private Rigidbody _rb;
    private IControllable nextControllable;

    [SerializeField] float moveSpeed = 2f;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void OnCollisionEnter(Collision collision)
    {
        //take over the collision object 
        nextControllable = collision.gameObject.GetComponent<IControllable>();
    }

    public void HandleAllMovement(Vector2 MoveInput, bool jumpPressed)
    {
        HorizontalMovement(MoveInput);
    }

    public void OnDePossess()
    {
        Debug.Log("Depossessed Parasite");
        //Cannot DePossess, this is the default controller.
    }

    public void OnPossess()
    {
        Debug.Log("Possessed Parasite");
        //cannot possess
    }

    private void HorizontalMovement(Vector2 MoveInput)
    {
        Vector3 moveVector = new Vector3(MoveInput.x * moveSpeed, _rb.linearVelocity.y, MoveInput.y * moveSpeed);
        _rb.linearVelocity = moveVector;
    }

    private void Possess()
    {

    }

    public IControllable GetNextControllable()
    {
        return nextControllable;
    }
}
