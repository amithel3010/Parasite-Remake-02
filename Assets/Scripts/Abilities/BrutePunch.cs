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

    [SerializeField] private float _damage;
    [SerializeField] private float _hitboxRadius = 1;
    [SerializeField] private float _duration = 0.2f;
    [SerializeField] private Transform _punchOrigin;

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

    void OnDrawGizmosSelected()
    {
        if (_isActive)
        {
            Gizmos.color = Color.red;
        }
        Gizmos.DrawWireSphere(_punchOrigin.position, _hitboxRadius);
    }
}
