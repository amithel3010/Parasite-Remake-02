using System.Collections;
using UnityEngine;

public class FlyAI : MonoBehaviour, IInputSource
{
    Possessable possessable;
    [SerializeField] Transform patrolTarget;
    [SerializeField] float jumpCooldown;

    public bool JumpPressed => jumpPressed;
    public bool JumpHeld => false;
    public bool ActionPressed => false;
    public bool ActionHeld => false;
    public Vector2 MovementInput => movementInput;

    private Vector2 movementInput;
    private bool jumpPressed;


    void Start()
    {
        possessable = GetComponent<Possessable>();
        possessable.SetInputSource(this);
    }

    private void Update()
    {
        //MoveFly(patrolTarget);
    }

    void FixedUpdate()
    {
        possessable.OnFixedUpdate();
    }

    //where is the logic for the inputs????

    private void MoveFly(Transform target)
    {
        Vector3 moveDir = target.position - transform.position;
        if (moveDir.magnitude > 2)
        {
            moveDir.Normalize();
            movementInput = new Vector2(moveDir.x, moveDir.z);
        }
        else
            movementInput = Vector2.zero;
       
    }
}
