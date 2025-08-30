using System;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class TutorialMessage : MonoBehaviour
{
    public GameObject _tutorialMessage;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerUtils.PlayerControlledLayer)
        {
            _tutorialMessage.SetActive(true);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (_tutorialMessage.activeSelf) return;
        
        if (other.gameObject.layer == LayerUtils.PlayerControlledLayer)
        {
            _tutorialMessage.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerUtils.PlayerControlledLayer)
        {
            _tutorialMessage.SetActive(false);
        }
    }
}