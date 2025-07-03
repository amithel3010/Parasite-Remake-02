using UnityEditor.ShaderGraph;
using UnityEngine;

public class BrutePunch : MonoBehaviour
{
    //assumes having posssessable and input source
    private IInputSource _inputSource;
    private IInputSource _defaultInputSource;
    private Possessable _possessable;

    [SerializeField] private float _hitboxRadius;
    [SerializeField] Transform _punchOrigin;
    [SerializeField] float _damage;

    private bool _isPunching;

    void Awake()
    {
        _defaultInputSource = GetComponent<IInputSource>();
        _inputSource = _defaultInputSource;
        _possessable = GetComponent<Possessable>();

        _possessable.Possessed += OnPossess;
        _possessable.UnPossessed += OnUnpossess;
    }


    //TODO: make overlap sphere show up in play mode.
    //TODO: make overlapsphere be around fo rmore than 1 frame
    void FixedUpdate()
    {
        _isPunching = false;

        if (_inputSource.Action2Pressed)
        {
            Collider[] hits = Physics.OverlapSphere(_punchOrigin.position, _hitboxRadius); //sphere?
            
            _isPunching = true;

            foreach (var hit in hits)
            {
                if (hit.transform.parent.gameObject.TryGetComponent<Health>(out Health health))
                {
                    Debug.Log("Damaged" + hit.gameObject.name);
                    health.ChangeHealth(-_damage);
                }
                if (hit.transform.parent.gameObject.TryGetComponent<KnockbackTest>(out KnockbackTest knockback))
                {
                    Debug.Log("trying to knockback" + knockback.gameObject.name);
                    Vector3 hitDir = hit.transform.position - _punchOrigin.position;
                    knockback.Knockback(hitDir, Vector3.up, Vector3.zero);
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

    void OnDrawGizmosSelected()
    {
        if (_isPunching)
        {
            Gizmos.color = Color.red;
        }
        Gizmos.DrawWireSphere(_punchOrigin.position, _hitboxRadius);
    }
}
