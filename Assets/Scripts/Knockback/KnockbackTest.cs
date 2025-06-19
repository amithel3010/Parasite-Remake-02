
using UnityEngine;

public class KnockbackTest : MonoBehaviour
{
    [SerializeField] private float _hitDirForce;
    [SerializeField] private float _constForce;
    [SerializeField] private float _inputForce;

    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public void Knockback(Vector3 hitDir, Vector3 constForceDir, Vector3 inputDir)
    {
        Vector3 hitForce;
        Vector3 constForce;
        Vector3 knockbackForce;
        Vector3 combinedForce;

        //TODO: missing a check for angle between hitDir and const force. leads to situations Upwards knockback is overwhelming. maybe should limit max vel

        hitForce = hitDir * _hitDirForce;
        constForce = constForceDir * _constForce;

        knockbackForce = hitForce + constForce;

        if (inputDir != Vector3.zero)
        {
            combinedForce = knockbackForce + inputDir;
        }
        else
        {
            combinedForce = knockbackForce;
        }

        Vector3 finalForce = combinedForce * _rb.mass;
        print(finalForce);

        _rb.AddForce(finalForce, ForceMode.Impulse);
    }
}
