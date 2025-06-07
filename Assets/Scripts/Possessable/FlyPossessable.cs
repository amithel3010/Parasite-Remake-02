using System.Buffers.Text;
using Unity.VisualScripting;
using UnityEngine;

public class FlyPossessable : Possessable
{
    public override void OnFixedUpdate()
    {
        Debug.Log("FixedUpdate");
        base.OnFixedUpdate();
    }

    public override void OnJumpInput(bool jumpInput)
    {
        if(jumpInput)
            Debug.Log("Flying");
    }

}
