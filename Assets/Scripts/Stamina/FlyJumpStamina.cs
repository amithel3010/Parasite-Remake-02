using UnityEngine;

public class FlyJumpStamina : MonoBehaviour, IPossessionSensitive
{
    
    [SerializeField] private float _damageOnJump = 20f;

    private InputBasedHoverMovement _movementScript;
    private Health _health;

    private bool _isActive = false;

    private void Awake()
    {
        _movementScript = GetComponent<InputBasedHoverMovement>();
        _health = GetComponent<Health>();
    }

    private void OnEnable()
    {
    }

    private void OnDisable()
    {

    }

    private void DealDamageOnJump()
    {
        if (_isActive)
        {
            _health.ChangeHealth(-_damageOnJump);
        }
    }

    public void OnPossessed(Parasite playerParasite, IInputSource inputSource)
    {
        _isActive = true;
    }

    public void OnUnPossessed(Parasite playerParasite)
    {
        _isActive = false;
    }
}
