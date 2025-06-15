using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    //Responsible for showing, hiding and updating UI elements

    public static UIManager Instance;

    [SerializeField] private Image _healthBar;
    [SerializeField] private Canvas _deathScreenCanvas;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("more than 1 instances of Ui Manager");
        }
        else
        {
            Instance = this;
        }
    }

    public void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        _healthBar.fillAmount = currentHealth / maxHealth;
    }

    public void ShowGameOverScreen()
    {
        _deathScreenCanvas.enabled = true;
    }
}
