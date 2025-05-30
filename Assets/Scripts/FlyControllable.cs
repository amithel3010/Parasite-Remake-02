
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FlyControllable : MonoBehaviour, IControllable
{
    private FlyAI AI;
    private ControllableManager controllableManager;

    private Rigidbody _rb;

    public Transform TargetToRotateTo; // should be set in the AI and the controllable manager... or maybe I should have logic in here to detect if AI is controlling or Player?
    //[SerializeField] LayerMask PlayerLayer;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>(); //this is the only reason i need mono behavior, is there a better way?
    }

    public void HandleAllMovement(Vector3 MoveAmount)
    {
        Debug.Log("Moving");
        RotateTowardsTarget(TargetToRotateTo);
    }

    public void OnDePossess()
    {
        Destroy(this.gameObject);
    }

    public void OnPossess()
    {
        throw new System.NotImplementedException();
    }

    private void RotateTowardsTarget(Transform target)
    {
        Vector3 targetDirection = (target.position - transform.position).normalized;
        Debug.DrawLine(target.position, transform.position);
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);   
        _rb.MoveRotation(targetRotation);
    }

    public void HandleAllMovement()
    {
        throw new System.NotImplementedException();
    }
}
