using System.Collections;
using UnityEngine;

public class ParasitePossessable : Hover
{
    [SerializeField] LayerMask possessableLayer;

    private bool isPossessing = false;

    public override void OnJumpInput(bool jumpInput)
    {
        base.OnJumpInput(jumpInput);
        CheckForPossessables();
    }

    private void CheckForPossessables() //and possess
    {
        if (!isPossessing)
        {
            //raycast downards for possessables
            if (Input.GetMouseButton(0))
            {
                Camera camera = Camera.main;
                Ray ray = camera.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out RaycastHit hit, 1, possessableLayer))
                {
                    Possess(hit);
                }
            }
        }

            
    }

    private void Possess(RaycastHit hitPossessable)
    {
        //stop checking for possessables
        isPossessing = true;
        //stop reciving input from player
        
        //this.gfx.setactive(false)
        //make this a child of possessed
        //set possessed input source to player
    }
}
