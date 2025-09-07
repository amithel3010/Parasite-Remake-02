using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Splines;

[Serializable]
public class SmoothFollow : SplineAutoDolly.ISplineAutoDolly {
    [Tooltip("Maximum speed of the dolly along the spline.")]
    public float MaxSpeed = 0.5f;

    [Tooltip("How quickly the dolly reacts to the target's movement.")]
    public float SmoothTime = 2f;

    [Tooltip("Look-ahead distance to predict target's future position.")]
    public float LookAheadDistance = 2f;

    [Tooltip("Distance tolerance to the target before stopping.")]
    public float StoppingDistance = 0.1f;

    private float currentVelocity;

    void SplineAutoDolly.ISplineAutoDolly.Validate() {
        MaxSpeed = Mathf.Max(0, MaxSpeed);
        SmoothTime = Mathf.Max(0.01f, SmoothTime);
        LookAheadDistance = Mathf.Max(0, LookAheadDistance);
        StoppingDistance = Mathf.Max(0, StoppingDistance);
    }

    void SplineAutoDolly.ISplineAutoDolly.Reset() {
        currentVelocity = 0;
    }

    bool SplineAutoDolly.ISplineAutoDolly.RequiresTrackingTarget => true;

    float SplineAutoDolly.ISplineAutoDolly.GetSplinePosition(
        MonoBehaviour sender, Transform target, SplineContainer spline,
        float currentPosition, PathIndexUnit positionUnits, float deltaTime) 
    {
        if (target == null || deltaTime <= 0) return currentPosition;

        // Predict the target's future position based on LookAheadDistance
        Vector3 targetPosition = target.position + target.forward * LookAheadDistance;
        Vector3 localTargetPos = spline.transform.InverseTransformPoint(targetPosition);

        // Find the nearest point on the spline to the predicted target position
        SplineUtility.GetNearestPoint(spline.Spline, localTargetPos, out _, out var normalizedPos, 10, 3);
        normalizedPos = Mathf.Clamp01(normalizedPos);

        float targetPositionOnSpline = spline.Spline.ConvertIndexUnit(normalizedPos, PathIndexUnit.Normalized, positionUnits);

        // Compute the distance between the current and target positions
        float distanceToTarget = Mathf.Abs(targetPositionOnSpline - currentPosition);

        // Check if within the stopping distance
        if (distanceToTarget < StoppingDistance) return currentPosition;

        // Smoothly interpolate to the target position
        return Mathf.SmoothDamp(currentPosition, targetPositionOnSpline, ref currentVelocity, SmoothTime, MaxSpeed, deltaTime);
    }
}