using System;
using UnityEngine;

[RequireComponent(typeof(Hover))]
public class InputBasedHoverMovement : MonoBehaviour, IPossessionSensitive, IPossessionSource, IDeathResponse, IPlayerRespawnListener
{
    private bool _isActive = true;

    [Header("Settings")]
    [Tooltip("Adds a layer of security to changes. Leave empty if testing and want to change settings often.")]
    [SerializeField] private HoveringCreatureSettings _settings;

    [Header("References")]
    private MonoBehaviour _knockbackProvider; // for seeing in inspector
    private IKnockbackStatus _knockbackStatus;
    private MonoBehaviour _inputSourceProvider; // for seeing in inspector
    private IInputSource _inputSource;
    private IInputSource _defaultInputSource;
    private Hover _hover;
    private Rigidbody _rb;

    [Header("Movement")]
    [SerializeField] private float _maxSpeed = 4f;
    [SerializeField] private float _acceleration = 25f;
    [SerializeField] private float _maxAccelForce = 150f;
    [SerializeField] private float _leanFactor = 0.25f;
    [SerializeField] private AnimationCurve _accelerationFactorFromDot;
    [SerializeField] private AnimationCurve _maxAccelerationForceFactorFromDot;
    [SerializeField] private Vector3 _moveForceScale = new Vector3(1f, 0f, 1f);

    private float _defaultMaxSpeed;
    private Vector3 _GoalDirFromInput;
    private Vector3 _GoalVel;
    private float _speedFactor = 1f; //Does nothing?
    private float _maxAccelForceFactor = 1f; //also does nothin?

    [Header("Debug")]
    [SerializeField] private bool _debugExpectedJumpHeight;

    private Vector3 _debugAdjustedJumpHeight;
    private Vector3 _debugJumpApex;

    void Awake()
    {
        if(_settings != null)
        {
            _maxSpeed = _settings.MaxSpeed;
            _acceleration = _settings.Acceleration;
            _maxAccelForce = _settings.MaxAccelForce;
            _leanFactor = _settings.LeanFactor;
            _accelerationFactorFromDot = _settings.AccelerationFactorFromDot;
            _maxAccelerationForceFactorFromDot = _settings.MaxAccelerationForceFactorFromDot;
        }

        _hover = GetComponent<Hover>();
        _rb = GetComponent<Rigidbody>(); // TODO: should get ref from hover?

        if (_inputSourceProvider == null)
            _inputSourceProvider = GetComponent<IInputSource>() as MonoBehaviour;

        if (_knockbackProvider == null)
            _knockbackProvider = GetComponent<IKnockbackStatus>() as MonoBehaviour;
        _knockbackStatus = _knockbackProvider as IKnockbackStatus;

        _inputSource = _inputSourceProvider as IInputSource;
        _defaultInputSource = _inputSource;

        if (_knockbackProvider != null && _knockbackStatus == null)
            Debug.LogWarning($"{name} has a KnockbackProvider that does not implement IKnockbackStatus.");
        if (_inputSource == null)
            Debug.LogError($"{name}: No IInputSource assigned or found.");

        _defaultMaxSpeed = _maxSpeed;      
    }

    void FixedUpdate()
    {
        if (!_isActive || !_hover.IsActive) return;
        if (_inputSource == null) return;
        if (_knockbackStatus.IsKnockedBack) return;

        CharacterMove(_inputSource.HorizontalMovement);
    }

    #region  Movement
    private void CharacterMove(Vector3 horizontalMoveInput)
    {
        _GoalDirFromInput = horizontalMoveInput.normalized;

        //calculate new goal vel...
        Vector3 unitDir = _GoalVel.normalized; //current vel direction

        float velDot = Vector3.Dot(_GoalDirFromInput, unitDir); // checking difference in direction in current input and current velocity direction?
        float accel = _acceleration * _accelerationFactorFromDot.Evaluate(velDot); //should be between 0 and 1?
                                                                                   // Debug.Log("VelDot:" + velDot + ", accel:" + accel);

        Vector3 goalVel = _maxSpeed * _speedFactor * _GoalDirFromInput; //velocity at its max

        //THIS IS THE ACTUAL CALCULATION OF THE NEW m_goalVel
        _GoalVel = Vector3.MoveTowards(_GoalVel, goalVel, accel * Time.fixedDeltaTime);

        //actual force
        Vector3 neededAccel = (_GoalVel - _rb.linearVelocity) / Time.fixedDeltaTime; // acceleration needed to reach max accel in 1 fixed update?

        float maxAccel = _maxAccelForce * _maxAccelerationForceFactorFromDot.Evaluate(velDot) * _maxAccelForceFactor;
        neededAccel = Vector3.ClampMagnitude(neededAccel, maxAccel);
        _rb.AddForceAtPosition(Vector3.Scale(neededAccel * _rb.mass, _moveForceScale), _rb.position + new Vector3(0f, _rb.transform.localScale.y * _leanFactor, 0f)); // Using AddForceAtPosition in order to both move the player and cause the play to lean in the direction of input.
    }
    #endregion

    #region Possesion Sensitive
    public void OnPossessed(Parasite playerParasite, IInputSource inputSource)
    {
        _inputSource = inputSource;
    }

    public void OnUnPossessed(Parasite playerParasite) => _inputSource = _defaultInputSource;

    public void OnParasitePossession() => _isActive = false;


    public void OnParasiteUnPossession()
    {
        _isActive = true;
    }
    #endregion

    #region Change Movement Parameters
    public void ChangeMovementParams(float maxSpeedChange)
    {
        //check if after change params makes sense
        if (_maxSpeed + maxSpeedChange <= 0)
        {
            Debug.LogWarning("max speed change is too large, defaulting to max speed = 1");
            _maxSpeed = 1;
        }
        else
        {
            _maxSpeed += maxSpeedChange;
        }
    }

    public void ResetMovementParams()
    {
        _maxSpeed = _defaultMaxSpeed;
    }
    #endregion

    #region On Death
    public void OnDeath()
    {
        _isActive = false;
    }
    #endregion

    #region On Player Respawn
    public void OnPlayerRespawn()
    {
        _isActive = true;
    }

    #endregion

    #region  Debug
    private void DrawJumpHeight()
    {

        Debug.DrawLine(_rb.position, _debugJumpApex, Color.yellow);
        DebugUtils.DrawSphere(_debugJumpApex, Color.cyan, 0.2f);
        DebugUtils.DrawSphere(_debugAdjustedJumpHeight, Color.red, 0.2f);

    }

    #endregion
}
