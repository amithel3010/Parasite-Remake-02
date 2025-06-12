using UnityEngine;

public class Parasite : MonoBehaviour
{
    //will check downwards for possessables
    // if found, will possess
    //when possess, stop checking disable hover script, and give reference to player input

    private bool _isPossessing;

    [SerializeField] private LayerMask _possessableLayer;
    [SerializeField] private float _possessRayLength;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        Ray PossessCheckRay = new(transform.position, -transform.up);
        if (Physics.Raycast(PossessCheckRay, out RaycastHit hitInfo, _possessRayLength, _possessableLayer))
        {
            //possess
        }
    }
}
