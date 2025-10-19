using UnityEngine;

public class Credits : MonoBehaviour
{
    public GameObject _Credits;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerUtils.PlayerControlledLayer)
        {
            _Credits.SetActive(true);
        }
    }







}
