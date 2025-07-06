using System.Collections.Generic;
using System.Threading;
using UnityEditor.ShaderGraph;
using UnityEngine;

public class BrutePunch : MonoBehaviour
{
    //assumes having posssessable and input source
    private IInputSource _inputSource;
    private IInputSource _defaultInputSource;
    private Possessable _possessable;

    [SerializeField] private BreakableType _canBreak;
    [SerializeField] private float _damage;
    [SerializeField] private float _hitboxRadius = 1;
    [SerializeField] private float _duration = 0.2f;
    [SerializeField] private Transform _punchOrigin;

    [SerializeField] private bool _showHitboxInGame = true;
    [SerializeField] private Material _debugMaterial; // A transparent material for visualization
    [SerializeField] private Mesh _debugMesh; // Optional: default to a Sphere mesh

    private bool _isPunching;
    private bool _isActive;
    private float _timer;
    private HashSet<GameObject> _alreadyHit = new HashSet<GameObject>();

    void Awake()
    {
        _defaultInputSource = GetComponent<IInputSource>();
        _inputSource = _defaultInputSource;
        _possessable = GetComponent<Possessable>();

        _possessable.Possessed += OnPossess;
        _possessable.UnPossessed += OnUnpossess;
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

        Collider[] hits = Physics.OverlapSphere(_punchOrigin.position, _hitboxRadius); //sphere?

        foreach (var hit in hits)
        {
            GameObject target = hit.gameObject;

            if (target == gameObject || _alreadyHit.Contains(target)) continue;

            _alreadyHit.Add(target);

            if (hit.transform.parent.gameObject.TryGetComponent<Health>(out Health health))
            {
                Debug.Log("Damaged" + hit.gameObject.name);
                health.ChangeHealth(-_damage);
            }
            if (hit.transform.parent.gameObject.TryGetComponent<KnockbackTest>(out KnockbackTest knockback))
            {
                Debug.Log("trying to knockback" + knockback.gameObject.name);
                Vector3 hitDir = (hit.transform.position - _punchOrigin.position).normalized;
                knockback.Knockback(hitDir, Vector3.up, Vector3.zero);
            }
            if (hit.transform.parent.gameObject.TryGetComponent<Breakable>(out Breakable breakable))
            {
                if (breakable._type == this._canBreak)
                {
                    breakable.Break();
                }
            }
        }


    }

    public void OnPossess(IInputSource newInputSource)
    {
        _inputSource = newInputSource;
    }

    public void OnUnpossess()
    {
        _inputSource = _defaultInputSource;
    }

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

}

