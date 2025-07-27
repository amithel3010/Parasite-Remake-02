using System;
using UnityEngine;

public class BaseJumpAbility : MonoBehaviour, IPossessionSensitive, IPossessionSource, IHasLandedEvent, IDeathResponse, IPlayerRespawnListener
{
    [Header("Settings")]
    [Tooltip("Adds a layer of security to changes. Leave empty if testing and want to change settings often.")]
    [SerializeField] protected HoveringCreatureSettings settings;

    [Header("Jump Settings")]
    [SerializeField] protected float _jumpHeight = 5f;
    [SerializeField] protected float _jumpBuffer = 0.2f;
    [SerializeField] protected float _coyoteTime = 0.2f;

    [Header("Debug")]
    [SerializeField] protected bool _showExcpectedJumpHeight;
    protected Vector3 _debugAdjustedJumpHeight;
    protected Vector3 _debugJumpApex;

    protected virtual int MaxJumps => 1;

    protected float _defaultJumpHeight;
    protected bool _isJumping;
    protected float _timeSinceJumpPressed = 0.5f; // if it's zero character jumps on start
    protected bool _jumpReady = true;
    protected int _availableJumps = 1;
    protected bool _wasGrounded;
    protected float _prevYVelocity;

    public event Action OnLanding; //more like OnFinishedJump
    public event Action OnJumpStarted;

    protected bool _isActive = true;

    //refs
    protected Rigidbody _rb;
    protected Hover _hover;
    protected MonoBehaviour _inputSourceProvider; // for seeing in inspector
    protected IInputSource _inputSource;
    protected IInputSource _defaultInputSource;
    protected MonoBehaviour _knockbackProvider; // for seeing in inspector
    protected IKnockbackStatus _knockbackStatus;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Awake()
    {
        if (settings != null)
        {
            _jumpHeight = settings.JumpHeight;
            _jumpBuffer = settings.JumpBuffer;
        }

        _rb = GetComponent<Rigidbody>();
        _hover = GetComponent<Hover>();

        if (_inputSourceProvider == null)
        {
            _inputSourceProvider = GetComponent<IInputSource>() as MonoBehaviour;
            _inputSource = _inputSourceProvider as IInputSource;
            _defaultInputSource = _inputSource;
        }

        if (_knockbackProvider == null)
        {
            _knockbackProvider = GetComponent<IKnockbackStatus>() as MonoBehaviour;
            _knockbackStatus = _knockbackProvider as IKnockbackStatus;
        }

        _availableJumps = MaxJumps;
        _defaultJumpHeight = _jumpHeight;
    }

    // Update is called once per frame
    protected virtual void FixedUpdate()
    {
        if (!_isActive || !_hover.IsActive) return;
        if (_inputSource == null) return;
        if (_knockbackStatus.IsKnockedBack) return;

        CharacterJump(_inputSource.JumpPressed);

        if (_showExcpectedJumpHeight)
        {
            //TODO: must be better way


            DrawJumpHeight();
        }
    }

    protected virtual void CharacterJump(bool jumpPressed)
    {

        _timeSinceJumpPressed += Time.fixedDeltaTime;

        if (_hover.IsGrounded)
        {
            if (!_wasGrounded && _isJumping)
            {
                //finished jump
                _isJumping = false;
                OnLanding?.Invoke();
                _availableJumps = MaxJumps;
                Debug.Log("Landed from a jump");
            }
        }

        if (_rb.linearVelocity.y < 0)
        {
            if (_prevYVelocity >= 0 && _jumpReady == false)
            {
                //reached apex
                _hover.SetMaintainHeight(true);
                OnReachedJumpApex();
                _jumpReady = true;
            }
        }

        if (jumpPressed)
        {
            _timeSinceJumpPressed = 0f;
        }

        if (CanJump())
        {
            PerformJump();
        }
        _wasGrounded = _hover.IsGrounded;
        _prevYVelocity = _rb.linearVelocity.y;
    }

    protected virtual bool CanJump()
    {
        return _timeSinceJumpPressed < _jumpBuffer &&
            _jumpReady &&
            _availableJumps > 0;
    }

    protected virtual void PerformJump()
    {
        //flags
        _jumpReady = false;
        _isJumping = true;
        _hover.SetMaintainHeight(false);
        _availableJumps--;

        //calc jump height from current pos
        float adjustedJumpHeight = CalcAdjustedJumpHeight();
        _debugAdjustedJumpHeight = _rb.position + Vector3.up * adjustedJumpHeight;

        //difference in velocity needed to be applied this frame to reach that height
        float goalVel = Mathf.Sqrt(adjustedJumpHeight * -2 * Physics.gravity.y);
        float currentVel = _rb.linearVelocity.y;

        float requiredVel = goalVel - currentVel;

        //then , we need the force need for that velocity change (acceleration)
        float jumpForce = requiredVel * _rb.mass;

        //add impulse force;
        _rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        OnJumpStarted?.Invoke();

        _timeSinceJumpPressed = _jumpBuffer; //to make sure jump only happens once per input
    }

    protected virtual float CalcAdjustedJumpHeight()
    {
        return _jumpHeight - _hover.CurrentDistanceFromGround;
        //still has small inconsistencies but I cant figure out why, and it's for sure good enough.
    }

    public void ChangeJumpHeight(float jumpHeightChange)
    {
        if (_jumpHeight + jumpHeightChange <= 0)
        {
            Debug.LogWarning("jump height change is too large, defaulting to jump height = 1");
            _jumpHeight = 1;
        }
        else
        {
            _jumpHeight += jumpHeightChange;
        }
    }

    protected virtual void OnReachedJumpApex()
    {
        //relevant only to cat?
    }

    public void ResetJumpHeight()
    {
        _jumpHeight = _defaultJumpHeight;
    }

    public virtual void OnPossessed(Parasite playerParasite, IInputSource inputSource)
    {
        _inputSource = inputSource;
    }

    public virtual void OnUnPossessed(Parasite playerParasite)
    {
        _inputSource = _defaultInputSource;
    }

    //if component is on parasite:
    public void OnParasitePossession()
    {
        _isActive = false;
    }

    public void OnParasiteUnPossession()
    {
        _isActive = true;
    }

    //OnDeath and OnRespawn
    public void OnDeath()
    {
        _isActive = false;
    }

    public void OnPlayerRespawn()
    {
        _isActive = true;
    }

    protected virtual void DrawJumpHeight()
    {
        float adjustedJumpHeight = CalcAdjustedJumpHeight(); //can't get this from the other var?
        _debugJumpApex = _rb.position + Vector3.up * adjustedJumpHeight;

        Debug.DrawLine(_rb.position, _debugJumpApex, Color.yellow);
        DebugUtils.DrawSphere(_debugJumpApex, Color.cyan, 0.2f);
        DebugUtils.DrawSphere(_debugAdjustedJumpHeight, Color.red, 0.2f);
    }
}
