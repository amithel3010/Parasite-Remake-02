using System;
using UnityEngine;

public class OutlineHandler : MonoBehaviour, IDeathResponse
{
    //kinda makes no sense doing it this way but it works...
    //would have been better if highlighted object knew he was supposed to be highlighted instead of the highlighting telling it to be.

    private Outline _objToOutline;
    private bool _shouldShowOutline = true;

    private void Update()
    {
        if (_objToOutline != null && _shouldShowOutline)
        {
            _objToOutline.enabled = true;
        }
    }

    public void SetObjToHighlight(Outline obj)
    {
        _objToOutline = obj;
    }

    public void ResetOutline()
    {
        if (_objToOutline != null)
        {
            _objToOutline.enabled = false;
            _objToOutline = null;
        }
    }

    public void OnDeath()
    {
        _objToOutline = null;
        _shouldShowOutline = false;
    }
}