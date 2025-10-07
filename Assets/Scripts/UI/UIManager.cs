using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    //Responsible for showing, hiding and updating UI elements

    public static UIManager Instance;

    [Header("UI Elements")] [SerializeField]
    private Image _healthBar;

    [SerializeField] private Image _collectableTracker;
    [SerializeField] private TMP_Text _collectableText;

    [Header("Canvases")] [SerializeField] private Canvas _deathScreenCanvas;

    [FormerlySerializedAs("_DebugCanvas")] [SerializeField]
    private Canvas _debugCanvas;
    [SerializeField] private Canvas _pauseMenuCanvas;

    InputHandler _playerInput;

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

        if (_playerInput == null)
        {
            _playerInput = FindAnyObjectByType<InputHandler>();
        }
    }

    private void Update()
    {

    }

    public void ChangeHealthBarImage(Image newImage)
    {
        _healthBar = newImage;
    }

    public void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        if (_healthBar != null)
        {
            //_healthBar.fillAmount = currentHealth / maxHealth;

            _healthBar.DOFillAmount(currentHealth / maxHealth, 1f).SetEase(Ease.OutCubic);
        }
    }

    public void UpdateCollectableTracker(int collected, int total)
    {
        //_collectableTracker.fillAmount = (float)collected / total;
        if (total == 0) return;

        _collectableTracker.DOFillAmount((float)collected / total, 1f).SetEase(Ease.OutCubic);
        _collectableText.text = $"{collected} / {total}";
    }

    public void ShowGameOverScreen()
    {
        _deathScreenCanvas.enabled = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void ToggleDebugMenuCanvas()
    {
        _debugCanvas.enabled = !_debugCanvas.enabled;
        Cursor.visible = !Cursor.visible;
        Cursor.lockState = Cursor.lockState == CursorLockMode.Locked ? CursorLockMode.None : CursorLockMode.Locked;
    }

    public void ShowPauseMenu()
    {
        _pauseMenuCanvas.enabled = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void HidePauseMenu()
    {
        _pauseMenuCanvas.enabled = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}