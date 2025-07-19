using System.Collections.Generic;
using UnityEngine;

public class BrutePunch : MonoBehaviour, IPossessionSensitive
{
    //assumes having posssessable and input source
    private IInputSource _inputSource;
    private IInputSource _defaultInputSource;
    private Possessable _possessable;

    [Header("Punch Settings")]
    [SerializeField] private BreakableType _canBreak;
    [SerializeField] private float _damage;
    [SerializeField] private float _hitboxRadius = 1;
    [SerializeField] private float _duration = 0.2f;
    [SerializeField] private float _punchCost = 5f;
    [SerializeField] private Transform _punchOrigin;

    [Header("Debug")]
    [SerializeField] private bool _showHitboxInGame = true;
    [SerializeField] private Material _debugMaterial; // A transparent material for visualization
    [SerializeField] private Mesh _debugMesh; // Optional: default to a Sphere mesh

    private Health _bruteHealth;
    private bool _shouldConsumeHealth;
    private bool _isPunching;
    private bool _isActive;
    private float _timer;
    private HashSet<GameObject> _alreadyHit = new HashSet<GameObject>();

    void Awake()
    {
        _defaultInputSource = GetComponent<IInputSource>();
        _inputSource = _defaultInputSource;
        _possessable = GetComponent<Possessable>();
        _bruteHealth = GetComponent<Health>();
    }

    void FixedUpdate()
    {
        //DebugDraw();

        if (_inputSource.Action2Pressed && !_isActive)
        {
            _isActive = true;
            _timer = _duration;
            _alreadyHit.Clear();
        }

        if (!_isActive) return;

        _timer -= Time.deltaTime;
        if (_timer <= 0f)
        {
            _isActive = false;
            return;
        }

        CheckforHittablesAndPreformHits();

    }

    private void CheckforHittablesAndPreformHits()
    {
        Collider[] hits = Physics.OverlapSphere(_punchOrigin.position, _hitboxRadius); //sphere?

        foreach (var hit in hits)
        {
            GameObject target = hit.gameObject;

            if (target == gameObject || _alreadyHit.Contains(target)) continue;

            _alreadyHit.Add(target);

            if (hit.transform.parent.gameObject.TryGetComponent<WoodenBox>(out WoodenBox box))
            {
                return;
            }

            if (hit.transform.parent.gameObject.TryGetComponent<Health>(out Health health))
            {
                Debug.Log("Damaged" + hit.gameObject.name);
                health.ChangeHealth(-_damage);
            }
            if (hit.transform.parent.gameObject.TryGetComponent<Knockback>(out Knockback knockback))
            {
                Debug.Log("trying to knockback" + knockback.gameObject.name);
                Vector3 hitDir = (hit.transform.position - _punchOrigin.position).normalized;
                knockback.ApplyKnockback(hitDir, Vector3.up, Vector3.zero);
            }
            if (hit.transform.parent.gameObject.TryGetComponent<Breakable>(out Breakable breakable))
            {
                if (breakable._type == this._canBreak)
                {
                    breakable.Break();
                }
            }
        }

        if (_shouldConsumeHealth)
            {
                _bruteHealth.ChangeHealth(-_punchCost);
            }
    }

    #region Possession Sensitive
    public void OnPossessed(Parasite playerParasite, IInputSource newInputSource)
    {
        _shouldConsumeHealth = true;
        _inputSource = newInputSource;
    }

    public void OnUnPossessed(Parasite playerParasite)
    {
        _shouldConsumeHealth = false;
        _inputSource = _defaultInputSource;
    }
    #endregion

    #region Debug
    void LateUpdate()
    {
        RenderDebugHitbox();
    }

    void OnDrawGizmosSelected()
    {
        if (_isActive)
        {
            Gizmos.color = Color.red;
        }
        Gizmos.DrawWireSphere(_punchOrigin.position, _hitboxRadius);
    }

    void RenderDebugHitbox()
    {
        if (!_showHitboxInGame || !_isActive) return;
        if (_debugMaterial == null || _debugMesh == null) return;

        Graphics.DrawMesh(_debugMesh, Matrix4x4.TRS(_punchOrigin.position, Quaternion.identity, Vector3.one * _hitboxRadius * 2f), _debugMaterial, 0);
    }
    #endregion

}

