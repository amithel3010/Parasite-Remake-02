using UnityEngine;

public class HoveringCreatureController : MonoBehaviour
{
    [SerializeField] private HoverSettings _hoverSettings;
    [SerializeField] private LocomotionSettings _locomotionSettings;
    
    private MaintainHeightAndUpright _hover;
    private Locomotion _locomotion;
    private Rigidbody _rb;
    private IInputSource _inputSource; //Coupled? only need this for lookDir

    void Awake()
    {
        _inputSource = GetComponent<IInputSource>();
        _rb = GetComponent<Rigidbody>();
        _hover = new MaintainHeightAndUpright(_rb, _hoverSettings);
        _locomotion = new Locomotion(_rb, _locomotionSettings);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _locomotion.Tick(_inputSource.MovementInput);

        Vector3 lookDir = GetLookDir();
        _hover.Tick(lookDir);
    }

    private Vector3 GetLookDir()
    {
        Vector3 lookDir = Vector3.zero;
        lookDir = new Vector3(_inputSource.MovementInput.x, 0, _inputSource.MovementInput.y);
        return lookDir;
    }
}
