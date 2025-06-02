using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class Possessable : MonoBehaviour
{
    protected Rigidbody _rb;
    protected IInputSource inputSource;



    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;


    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    protected virtual void FixedUpdate()
    {
        if (inputSource != null)
        {
            HandleAllInputs(inputSource);
        }
    }

    protected virtual void HandleAllInputs(IInputSource input)
    {
        HandleHorizontalInput(input);
        HandleJumpInput(input);
        HandleActionInput(input);
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
            _rb.AddForce(Vector3.up * 5f, ForceMode.Impulse);
        }
    }

    protected abstract void HandleActionInput(IInputSource input);

    public virtual void OnPossessed()
    {
        Debug.Log("Possessed" + this);
    }

    public virtual void OnDepossessed()
    {
        Debug.Log("Now Depossessed" + this);
        SetInputSource(null); //Will change to be AI input source
    }

    public void SetInputSource(IInputSource source)
    {
        //should be called from an outside script. is this ok?
        inputSource = source;
    }

}
