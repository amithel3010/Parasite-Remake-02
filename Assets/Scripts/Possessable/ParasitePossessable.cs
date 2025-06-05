using System.Collections;
using UnityEngine;

public class ParasitePossessable : Hover
{
    InputHandler playerInput;


    public override void OnJumpInput(bool jumpInput)
    {
        base.OnJumpInput(jumpInput);
        //possess checks and logic
    }

    private void CheckForPossessables()
    {
        //raycast downards for possessables
    }

    private void Possess()
    {
        //stop checking for possessables
        //stop reciving input from player
        //this.gfx.setactive(false)
        //make this a child of possessed
        //set possessed input source to player
    }
}
