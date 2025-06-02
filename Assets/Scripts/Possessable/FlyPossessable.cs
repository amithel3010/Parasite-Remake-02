using UnityEngine;

public class FlyPossessable : Possessable
{

    public override void OnDepossessed()
    {
        base.OnDepossessed();
        Destroy(this.gameObject);
    }
}
