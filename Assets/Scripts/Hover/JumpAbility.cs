using System;
using UnityEngine;

public class JumpAbility : MonoBehaviour, IPossessionSensitive, IPossessionSource, IHasLandedEvent, IDeathResponse, IPlayerRespawnListener
{
    [Header("Settings")]
    [SerializeField] private HoveringCreatureSettings settings;

    [Header("Jumping")]
    [SerializeField] private int _maxJumps = 1;
    [SerializeField] private float _jumpHeight = 5f;
    [SerializeField] private float _jumpBuffer = 0.2f;
    [SerializeField] private float _coyoteTime = 0.2f;

    private float _defaultJumpHeight;
    private bool _isJumping;
    private float _timeSinceJumpPressed = 0.5f; // if it's zero character jumps on start
    private bool _jumpReady = true;
    private int _availableJumps = 1;
    private bool _wasGrounded;

    public event Action OnLanding; //more like OnFinishedJump
    public event Action OnJumpStarted;

    private bool _isActive = true;

    //refs
    private Rigidbody _rb;
    private Hover _hover;
    private MonoBehaviour _inputSourceProvider; // for seeing in inspector
    private IInputSource _inputSource;
    private IInputSource _defaultInputSource;
    private MonoBehaviour _knockbackProvider; // for seeing in inspector
    private IKnockbackStatus _knockbackStatus;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (settings != null)
        {
            _maxJumps = settings.MaxJumps;
            _jumpHeight = settings.JumpHeight;
            _jumpBuffer = settings.JumpBuffer;
            _coyoteTime = settings.CoyoteTime;
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

        _defaultJumpHeight = _jumpHeight;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!_isActive || !_hover.IsActive) return;
        if (_inputSource == null) return;
        if (_knockbackStatus.IsKnockedBack) return;

        CharacterJump(_inputSource.JumpPressed);
    }

    private void CharacterJump(bool jumpPressed)
    {

        _timeSinceJumpPressed += Time.fixedDeltaTime;

        if (_hover.IsGrounded)
        {
            if (!_wasGrounded && _isJumping)
            {
                //finished jump
                _isJumping = false;
                OnLanding?.Invoke();
                Debug.Log("Landed from a jump");
            }
            _availableJumps = _maxJumps;
        }

        if (_rb.linearVelocity.y < 0)
        {
            _jumpReady = true;
            _hover.SetMaintainHeight(true);
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
    }

    private bool CanJump()
    {
        return _timeSinceJumpPressed < _jumpBuffer && _hover.TimeSinceUngrounded < _coyoteTime && _jumpReady && _availableJumps > 0;
    }

    private void PerformJump()
    {
        //flags
        _jumpReady = false;
        _isJumping = true;
        _hover.SetMaintainHeight(false);
        _availableJumps--;

        //calc jump height from current pos
        float adjustedJumpHeight = _jumpHeight - _hover.CurrentDistanceFromGround; //still has small inconsistencies but I cant figure out why, and it's for sure good enough.

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

    public void ResetJumpHeight()
    {
        _jumpHeight = _defaultJumpHeight;
    }

    public void OnPossessed(Parasite playerParasite, IInputSource inputSource)
    {
        _inputSource = inputSource;
    }

    public void OnUnPossessed(Parasite playerParasite)
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
}
