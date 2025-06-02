using Unity.Mathematics;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class Possessable : MonoBehaviour
{
    protected IInputSource inputSource;
    protected Rigidbody _rb;

    [SerializeField] protected Possessable cachedParasite;

    [SerializeField] protected PlayerController playerController;

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
        HandleActionPressInput(input);
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

    protected virtual void HandleActionPressInput(IInputSource input)
    {
        if (input.ActionPressed)
        {
            playerController.Possess(cachedParasite);
        }
    }

    public virtual void OnPossessed()
    {
        Debug.Log("Possessed" + this);
    }

    public virtual void OnDepossessed()
    {
        cachedParasite.transform.SetParent(null);
        Debug.Log("Now Depossessed" + this);
        SetInputSource(null); //Will change to be AI input source
    }

    public void SetInputSource(IInputSource source)
    {
        //TODO: feels wrong this should be called from an outside script. is this ok?
        inputSource = source;
        print("set input source to" + source);
    }

}
