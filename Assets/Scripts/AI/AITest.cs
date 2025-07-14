using UnityEngine;

public class AITest : MonoBehaviour, IInputSource
{
    [SerializeField] bool _action2Pressed = false; //for debugging

    public bool JumpPressed => false;

    public bool JumpHeld => false;

    public bool ActionPressed => false;

    public bool ActionHeld => false;

    public bool Action2Pressed => _action2Pressed;

    public Vector2 MovementInput => Vector2.zero;

    public Vector3 HorizontalMovement => _desiredMoveDir;

    [SerializeField] private float _playerDetectionRadius;

    [Header("Debug")]
    [SerializeField] private bool _showPlayerDetectionSphere;

    private Vector3 _desiredMoveDir;

    private void FixedUpdate()
    {
        _desiredMoveDir = Vector3.zero;

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, _playerDetectionRadius);
        foreach (var collider in hitColliders)
        {
            if (collider.transform.parent.gameObject.TryGetComponent<Parasite>(out Parasite parasite))
            {
                //detected player
                _desiredMoveDir = (parasite.transform.position - transform.position).normalized;
                return;
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (_showPlayerDetectionSphere)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(transform.position, _playerDetectionRadius);
        }
    }
}
