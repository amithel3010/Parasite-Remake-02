using UnityEngine;
using System.Collections;

public class BillboardSprite : MonoBehaviour
{
    [SerializeField] private bool alignNotLook = true;

    void LateUpdate()
    {

        if (alignNotLook)
            transform.forward = Camera.main.transform.forward;
        else
            transform.LookAt(Camera.main.transform.forward, Vector3.up);
    }
}