using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    //Responsible for showing, hiding and updating UI elements

    public static UIManager Instance;

    [SerializeField] InputHandler _playerInput;

    [SerializeField] private Image _healthBar;
    [SerializeField] private Image _collectableTracker;
    [SerializeField] private TMP_Text _collectableText;

    [Header("Canvases")]
    [SerializeField] private Canvas _deathScreenCanvas;
    [SerializeField] private Canvas _DebugCanvas;

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

    private void Update()
    {
        if (_playerInput._debugPressed)
        {
            ToggleDebugMenu();
        }
    }

    public void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        _healthBar.fillAmount = currentHealth / maxHealth;
    }

    public void UpdateCollectableTracker(int collected, int total)
    {
        _collectableTracker.fillAmount = (float)collected / total;
        Debug.Log(_collectableTracker.fillAmount);
        _collectableText.text = $"{collected} / {total}";
    }

    public void ShowGameOverScreen()
    {
        _deathScreenCanvas.enabled = true;
    }

    private void ToggleDebugMenu()
    {
        if (_DebugCanvas.enabled)
        {
            _DebugCanvas.enabled = false;
        }

        else if (_DebugCanvas.enabled == false)
        {
            _DebugCanvas.enabled = true; ;
        }
    }
}
