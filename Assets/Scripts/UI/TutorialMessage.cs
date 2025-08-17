using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class TutorialMessage : MonoBehaviour
{

    public GameObject _tutorialMessage;



    private void OnTriggerStay(Collider Collision)
    {
        if (Collision.gameObject)
        {
            _tutorialMessage.SetActive(true);

        }
      
    }

    private void OnTriggerExit(Collider Collision)
    {
        if (Collision.gameObject)
        {
            _tutorialMessage.SetActive(false);

        }
    }






}
