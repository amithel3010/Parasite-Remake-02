using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    //class responsible for setting the current active checkpoint
    //should trigger if parasite goes through it or a possessed possessable

    [Tooltip("if set to none, it uses the transform.position of this checkpoint")]
    [SerializeField] private Transform _respawnPoint;


    [Header("Debugging")]
    private Renderer _renderer;

    private bool _isActive = false;
    private TriggerChanneler _trigger;

    private static readonly int BaseColor = Shader.PropertyToID("_BaseColor");

    private void Awake()
    {
        _renderer = GetComponentInChildren<Renderer>();
        _trigger = GetComponentInChildren<TriggerChanneler>();

        if (_trigger == null)
        {
            Debug.LogError("No Channeler In Child");
        }
    }

    private void OnEnable()
    {
        if (_trigger != null)
        {
            _trigger.OnTriggerEnterEvent += HandleTriggerEnter;
        }
    }

    private void OnDisable()
    {
        if (_trigger != null)
        {
            _trigger.OnTriggerEnterEvent -= HandleTriggerEnter;
        }
    }

    private void HandleTriggerEnter(Collider other)
    {
        if (_isActive) return;
        if (other.gameObject.layer != LayerUtils.PlayerControlledLayer) return; //Checks only for player. not versatile
        
        CheckpointManager.Instance.SetActiveCheckpoint(this);

    }

    public void SetActive(Color activeColor) //called from manager
    {
        _isActive = true;
        _renderer.material.SetColor(BaseColor, activeColor);
    }

    public void SetInactive(Color inactiveColor) // called from manager
    {
        _isActive = false;
        _renderer.material.SetColor(BaseColor, inactiveColor);
    }

    public Vector3 GetRespawnPoint()
    {
        return _respawnPoint != null ? _respawnPoint.position : transform.position;
    }
}
