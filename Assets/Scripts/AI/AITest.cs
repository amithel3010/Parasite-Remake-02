using UnityEngine;

namespace AI
{
    public class AITest : MonoBehaviour, IInputSource, IPossessionSensitive
    {
        [SerializeField] bool _action2Pressed = false; //for debugging
        private bool _isPossessedByPlayer;

        public bool JumpPressed => false;

        public bool JumpHeld => false;

        public bool ActionPressed => false;

        public bool ActionHeld => false;

        public bool Action2Pressed => _action2Pressed;

        public Vector2 MovementInput => Vector2.zero;

        public Vector3 HorizontalMovementVector => _desiredMoveDir;

        [SerializeField] private float _playerDetectionRadius;

        [Header("Debug")] [SerializeField] private bool _showPlayerDetectionSphere;

        private Vector3 _desiredMoveDir;
        private bool _detectedPlayer;

        private void FixedUpdate()
        {
            if (_isPossessedByPlayer) return;
            
            _desiredMoveDir = Vector3.zero;
            _detectedPlayer = false;

            Collider[] hitColliders = Physics.OverlapSphere(transform.position, _playerDetectionRadius, LayerUtils.PlayerControlledLayerMask);
            foreach (var col in hitColliders)
            {
                //TODO: Doesn't detect possessables right now
                
                if (col.transform.parent.gameObject.TryGetComponent<Parasite>(out Parasite parasite))
                {
                    //detected player
                    _detectedPlayer = true;
                    _desiredMoveDir = (parasite.transform.position - transform.position).normalized;
                    return;
                }
            }
        }

        private void OnDrawGizmos()
        {
            if (_showPlayerDetectionSphere)
            {
                Gizmos.color = _detectedPlayer ? Color.green : Color.red;
                Gizmos.DrawWireSphere(transform.position, _playerDetectionRadius);
            }
        }

        public void OnPossessed(Parasite playerParasite, IInputSource inputSource)
        {
            _isPossessedByPlayer = true;
        }

        public void OnUnPossessed(Parasite playerParasite)
        {
            _isPossessedByPlayer = false;
        }
    }
}