using System;
using Mono.Cecil;
using UnityEngine;

public class FillUi : MonoBehaviour, IPossessionSensitive
{
    [SerializeField] private SpriteRenderer _staminaBar;

    private string _fillPropertyName = "_Arc1";

    private IResource _resource;

    private Material _fillMaterial;

    private void Awake()
    {
        _resource = GetComponent<Stamina>();
        if (_resource == null)
        {
            Debug.LogError($"{name} has no valid IResource assigned.");
            enabled = false;
            return;
        }

        if (_staminaBar == null)
        {
            Debug.LogError("_staminaBar not assigned.");
            enabled = false;
            return;
        }
        else
        {
            _staminaBar.enabled = false; //dont show stamina on start
        }

        _fillMaterial = _staminaBar.material;
    }

    void Start()
    {
        UpdateFill(_resource.CurrentValue, _resource.MaxValue);
    }

    private void OnEnable()
    {
        _resource.OnValueChanged += UpdateFill;
    }

    private void OnDisable()
    {
        _resource.OnValueChanged -= UpdateFill;
    }

    private void UpdateFill(float current, float max)
    {
        float percent = Mathf.Clamp01(current / max);
        float fillAngle = (1f - percent) * 360f; // invert the fill

        Debug.Log($"[StaminaUIFill] Updating fill: current={current}, max={max}, angle={fillAngle}");
        _fillMaterial.SetFloat(_fillPropertyName, fillAngle);
    }

    public void OnPossessed(Parasite playerParasite, IInputSource inputSource)
    {
        _staminaBar.enabled = true;
    }

    public void OnUnPossessed(Parasite playerParasite)
    {
        _staminaBar.enabled = false;
    }
}