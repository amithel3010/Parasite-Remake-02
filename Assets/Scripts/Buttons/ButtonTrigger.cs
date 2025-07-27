using UnityEngine;

public class ButtonTrigger : MonoBehaviour
{
    private Button _button;
    void Awake()
    {
        _button = GetComponentInParent<Button>();
    }

    private void OnTriggerEnter(Collider other)
    {
        _button.OnChildTriggerEnter(other);
    }
    
    private void OnTriggerExit(Collider other)
    {
        _button.OnChildTriggerExit(other);
    }
}
