using Unity.Cinemachine;
using UnityEngine;

public class SwitchCameraTrigger : MonoBehaviour
{
    [SerializeField] GameObject _CameraHolderToSwitchTo;

    private CinemachineBrain _brain;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerUtils.PlayerControlledLayer)
        {
            CameraManager.Instance.ChangeCamera(_CameraHolderToSwitchTo);
        }
    }
}
