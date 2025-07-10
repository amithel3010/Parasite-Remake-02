using UnityEngine;
using UnityEngine.Events;

public class Button : MonoBehaviour
{
    //for now this is a purely functional script
    //just for grey box testing and level design

    //Should be able to be pressed by brute standing on it, or a wooden box on it
    //on press, raise a unity event

    private CanPushButtons _pusher;
    private bool _isPushed = false;

    [SerializeField] private UnityEvent OnButtonPress;
    [SerializeField] private UnityEvent OnButtonUp;

    public void OnChildTriggerEnter(Collider other)
    {
        print("Triggered");
        if (_pusher == null)
        {
            if (other.transform.parent.gameObject.TryGetComponent<CanPushButtons>(out _pusher))
            {
                _isPushed = true;
                OnButtonPress?.Invoke();
                Debug.Log("Button Pushed");
            }

        }
    }

    public void OnChildTriggerExit(Collider other)
    {
        if (_pusher != null)
        {
            if (other.transform.parent.gameObject == _pusher.gameObject)
            {
                _pusher = null;
                _isPushed = false;
                OnButtonUp?.Invoke();
                Debug.Log("Button Released");
            }

        }
    }
}
