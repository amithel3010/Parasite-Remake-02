using UnityEngine;

public class ParasiteAbstractTest : Possessable
{
    protected override void HandleActionInput(IInputSource input)
    {
        if (input.ActionPressed)
        {
            Debug.Log("Action pressed");
        }
    }
}
