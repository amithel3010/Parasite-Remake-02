using UnityEngine;
using TMPro;

public class TriggerTextDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI displayText;

    private void Start()
    {
        SetDisplay(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger entered by: " + other.name);

        if (other.CompareTag("Player"))
        {
            SetDisplay(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SetDisplay(false);
        }
    }

    private void SetDisplay(bool visible)
    {
        if (displayText != null)
        {
            displayText.enabled = visible;
        }
    }
}
