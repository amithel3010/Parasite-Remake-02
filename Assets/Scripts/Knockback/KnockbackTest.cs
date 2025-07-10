
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class KnockbackTest : MonoBehaviour, IKnockbackStatus
{
    [HideInInspector] public bool KnockbackEnabled = true;

    [SerializeField] private float _hitDirForce;
    [SerializeField] private float _constForce;
    [SerializeField] private float _inputForce;

    [SerializeField] private float _timer = 0.5f;

    [SerializeField] private AnimationCurve _constForceScaleFromDot;

    private bool _isKnockedBack;
    public bool IsKnockedBack => _isKnockedBack;

    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public void Knockback(Vector3 hitDir, Vector3 constForceDir, Vector3 inputDir)
    {
        if (IsKnockedBack || !KnockbackEnabled) return;

        Vector3 hitForce;
        Vector3 scaledConstForce;
        Vector3 knockbackForce;
        Vector3 combinedForce;

        //TODO: still feels kinda off... maybe i should just set velocity.
        // i kinda have a vision for a tool that lets knockback be controlled with a spline.

        float dot = Vector3.Dot(hitDir.normalized, constForceDir.normalized);
        float scale = _constForceScaleFromDot.Evaluate(dot);

        hitForce = hitDir * _hitDirForce;
        scaledConstForce = constForceDir * _constForce * scale;

        knockbackForce = hitForce + scaledConstForce;

        if (inputDir != Vector3.zero)
        {
            combinedForce = knockbackForce + inputDir;
        }
        else
        {
            combinedForce = knockbackForce;
        }

        Vector3 finalForce = combinedForce * _rb.mass;

        StartCoroutine(TriggerKnockbackCooldown());
        _rb.AddForce(finalForce, ForceMode.Impulse);

    }

    private IEnumerator TriggerKnockbackCooldown()
    {
        _isKnockedBack = true;
        yield return new WaitForSeconds(_timer);
        _isKnockedBack = false;
    }
}
