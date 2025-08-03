using System;
using UnityEngine;

public class TEMPControlsMenu : MonoBehaviour
{
   private Boolean IsMenuUp = false;
    [SerializeField] private GameObject MenuObject;

    // Update is called once per frame
    void Update()
    {
        BringUpMenu();
    }

    private void BringUpMenu()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            IsMenuUp = !IsMenuUp;
            Debug.Log("click");
        }
        if (IsMenuUp == true)
        {
            MenuObject.SetActive(true);
            
        }
        if (!IsMenuUp)
        {
            MenuObject.SetActive(false);
            
        }
    }





}
