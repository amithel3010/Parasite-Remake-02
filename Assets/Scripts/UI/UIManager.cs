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

    private bool _lastDebugPressed;

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
        if (_playerInput._debugPressed && !_lastDebugPressed)
        {
            ToggleDebugMenu();
        }

        _lastDebugPressed = _playerInput._debugPressed;
    }

    public void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        _healthBar.fillAmount = currentHealth / maxHealth;
    }

    public void UpdateCollectableTracker(int collected, int total)
    {
        _collectableTracker.fillAmount = (float)collected / total;
        _collectableText.text = $"{collected} / {total}";
    }

    public void ShowGameOverScreen()
    {
        _deathScreenCanvas.enabled = true;
    }

    private void ToggleDebugMenu()
    {
            _DebugCanvas.enabled = !_DebugCanvas.enabled;
    }
}
