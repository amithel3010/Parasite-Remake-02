using Unity.Mathematics;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class Possessable : MonoBehaviour
{
    //base class for handling and controlling possessables
    //should be able to recieve inputs from player OR from AI
    //TODO: maybe should reconsider? faking inputs to move AI seems hard

    protected IInputSource inputSource;
    protected Rigidbody _rb;
    protected PlayerController playerController;

    [SerializeField] protected GameObject cachedParasite; //TODO: I need this cached on every Possessable EXCEPT parasite. how should I approach that?


    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 5f;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        playerController = FindAnyObjectByType<PlayerController>();
    }

    protected virtual void FixedUpdate()
    {
        HandleAllInputs(inputSource);
    }

    protected virtual void HandleAllInputs(IInputSource input)
    {
        if (inputSource != null)
        {
            HandleHorizontalInput(input);
            HandleJumpInput(input);
            HandleActionPressInput(input);
        }
    }

    protected virtual void HandleHorizontalInput(IInputSource input)
    {
        Vector3 horizontalMoveDir = new Vector3(input.MovementInput.x, 0f, input.MovementInput.y);

        _rb.AddForce(horizontalMoveDir * moveSpeed);
    }

    protected virtual void HandleJumpInput(IInputSource input)
    {
        if (input.JumpPressed)
        {
            _rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    protected virtual void HandleActionPressInput(IInputSource input)
    {
        if (input.ActionPressed)
        {
            Possessable parasite = Instantiate(cachedParasite, transform.position + transform.forward * 3f, quaternion.identity).GetComponent<Possessable>();
            playerController.Possess(parasite);
        }
    }

    public virtual void OnPossessed()
    {
        Debug.Log("Now Possessed" + this);
    }

    public virtual void OnDepossessed()
    {
        Debug.Log("Now Depossessed" + this);
        //  SetInputSource(null); //Will change to be AI input source
        Destroy(this.gameObject);
    }

    public void SetInputSource(IInputSource source)
    {
        //TODO: feels wrong this should be called from an outside script. is this ok?
        inputSource = source;
        print("set input source of" + this + "to" + source);
    }
}
