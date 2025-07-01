using UnityEngine;

public class BrutePunch : MonoBehaviour
{
    private IInputSource _inputSource;

    [SerializeField] private float _hitboxRadius;
    [SerializeField] Transform _punchOrigin;
    [SerializeField] float _damage;

    void FixedUpdate()
    {
        if (_inputSource.Action2Pressed)
        {
            Collider[] hits = Physics.OverlapSphere(_punchOrigin.position, _hitboxRadius); //sphere?
            //TODO: find a way to debug this
            foreach (var hit in hits)
            {
                if (hit.gameObject.TryGetComponent<IDamagable>(out IDamagable health))
                {
                    Debug.Log("Damaged" + hit.gameObject.name);
                    health.ChangeHealth(-_damage);
                }
            }
        }
    }

    public void OnPossess(IInputSource newInputSource) //TODO: maybe change to a possession sesitive listener
    {

    }

    public void OnUnpossess()
    {

    }
}
