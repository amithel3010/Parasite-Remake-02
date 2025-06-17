using UnityEngine;

public class Locomotion
{
    private readonly Rigidbody _rb;

    public Locomotion(Rigidbody rb, LocomotionSettings settings)
    {
        _rb = rb;

        _maxSpeed = settings.MaxSpeed;
        _acceleration = settings.Acceleration;
        _maxAccelForce = settings.MaxAccelForce;
        _leanFactor = settings.LeanFactor;
        _accelerationFactorFromDot = settings.AccelerationFactorFromDot;
        _maxAccelerationForceFactorFromDot = settings.MaxAccelerationForceFactorFromDot;
        _moveForceScale = settings.MoveForceScale;
    }

    private float _maxSpeed = 4f;
    private float _acceleration = 25f;
    private float _maxAccelForce = 150f;
    private float _leanFactor = 0.25f;
    private AnimationCurve _accelerationFactorFromDot;
    private AnimationCurve _maxAccelerationForceFactorFromDot;
    private Vector3 _moveForceScale = new Vector3(1f, 0f, 1f);

    private Vector3 _GoalDirFromInput;
    private Vector3 _GoalVel;
    private float _speedFactor = 1f; //Does nothing?
    private float _maxAccelForceFactor = 1f; //also does nothin?

    public void Tick(Vector2 moveInput)
    {
        CharacterMove(moveInput);
    }

    private void CharacterMove(Vector2 moveInput) //might be better if moveInput was a field
    {
        _GoalDirFromInput = new Vector3(moveInput.x, 0, moveInput.y).normalized;

        //calculate new goal vel...
        Vector3 unitDir = _GoalVel.normalized; //current vel direction

        float velDot = Vector3.Dot(_GoalDirFromInput, unitDir); // checking difference in direction in current input and current velocity direction?
        float accel = _acceleration * _accelerationFactorFromDot.Evaluate(velDot); //should be between 0 and 1?
                                                                                   // Debug.Log("VelDot:" + velDot + ", accel:" + accel);

        Vector3 goalVel = _maxSpeed * _speedFactor * _GoalDirFromInput; //velocity at its max

        //THIS IS THE ACTUAL CALCULATION OF THE NEW m_goalVel
        _GoalVel = Vector3.MoveTowards(_GoalVel, goalVel, accel * Time.fixedDeltaTime);

        //actual force
        Vector3 neededAccel = (_GoalVel - _rb.linearVelocity) / Time.fixedDeltaTime; // acceleration needed to reach max accel in 1 fixed update?

        float maxAccel = _maxAccelForce * _maxAccelerationForceFactorFromDot.Evaluate(velDot) * _maxAccelForceFactor;
        neededAccel = Vector3.ClampMagnitude(neededAccel, maxAccel);
        _rb.AddForceAtPosition(Vector3.Scale(neededAccel * _rb.mass, _moveForceScale), _rb.position + new Vector3(0f, _rb.transform.localScale.y * _leanFactor, 0f)); // Using AddForceAtPosition in order to both move the player and cause the play to lean in the direction of input.
    }
}
