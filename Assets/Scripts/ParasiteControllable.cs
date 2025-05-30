using UnityEditor.Callbacks;
using UnityEngine;

public class ParasiteControllable : MonoBehaviour, IControllable
{
    private Rigidbody _rb;

    [SerializeField] float moveSpeed = 2f;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public void HandleAllMovement(Vector3 MoveInput)
    {
        _rb.linearVelocity = MoveInput * moveSpeed;
    }

    public void OnDePossess()
    {
        //Cannot DePossess, this is the default controller.
    }

    public void OnPossess()
    {
        //cannot possess
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
