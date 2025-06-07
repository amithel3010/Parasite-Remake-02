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

                if (Physics.Raycast(ray, out RaycastHit hit, 15, possessableLayer))
                {
                    Debug.Log("Hit");
                    Possess(hit);
                }
            }
        }

            
    }

    private void Possess(RaycastHit hitPossessable)
    {
        //stop checking for possessables
        isPossessing = true;
        //make this a child of possessed
        this.transform.SetParent(hitPossessable.transform);
        //stop reciving input from player

        //this.gfx.setactive(false)
        this.gameObject.SetActive(false);
        //set possessed input source to player
        hitPossessable.transform.GetComponent<Possessable>().SetInputSource(InputSource);
    }

    private void StopControllingParasite()
    {
        this.gameObject.SetActive(false);
    }
}
