using System;
using DG.Tweening;
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

        //_fillMaterial.SetFloat(_fillPropertyName, fillAngle);
        _fillMaterial.DOFloat(fillAngle, _fillPropertyName, 0.8f).SetEase(Ease.InOutCubic);
    }

    public void OnPossessed(Parasite playerParasite, IInputSource inputSource)
    {
        _staminaBar.enabled = true;
        _staminaBar.transform.DOScale(0, 0);
        _staminaBar.transform.DOScale(0.7f, 2f).SetEase(Ease.InOutCubic); // breakable
    }

    public void OnUnPossessed(Parasite playerParasite)
    {
        _staminaBar.transform.DOScale(0f, 2f).SetEase(Ease.InOutCubic);
        _staminaBar.enabled = false;
    }
}