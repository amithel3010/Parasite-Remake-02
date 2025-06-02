using Unity.Mathematics;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class Possessable : MonoBehaviour
{
    protected Rigidbody _rb;
    protected IInputSource inputSource;

    [SerializeField] protected PlayerController playerController;

    [SerializeField] protected GameObject parasitePrefab; // TODO: maybe better to disable and enable instead of instantiate...
    protected Possessable cachedParasite;

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
        Debug.Log("Now Depossessed" + this);
        //  Possessable newPossessable = Instantiate(parasitePrefab, transform.position, quaternion.identity).GetComponent<Possessable>();
        SetInputSource(null); //Will change to be AI input source
    }

    public void SetInputSource(IInputSource source)
    {
        //should be called from an outside script. is this ok?
        inputSource = source;
        print("set input source to" + source);
    }

    public void CacheParasitePossessable(ParasiteAbstractTest parasitePossesable)
    {
        cachedParasite = parasitePossesable;
    }

}
