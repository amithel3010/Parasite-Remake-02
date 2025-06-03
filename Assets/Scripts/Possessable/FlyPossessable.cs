using UnityEngine;

public class FlyPossessable : Possessable
{
    //Should be able to fly in Bursts, has 2 HP

    public override void OnDepossessed()
    {
        base.OnDepossessed();
        Destroy(this.gameObject);
    }

}
