using UnityEngine;

public static class MathUtils
{
    /// <summary>
    /// Calculates the shortest rotation between two Quaternions.
    /// </summary>
    /// <param name="a">The first Quaternion, from which the rotation is to be calculated.</param>
    /// <param name="b">The second Quaternion, for which the rotation is the goal.</param>
    /// <returns>The shortest rotation from a to b.</returns>
    public static Quaternion ShortestRotation(Quaternion a, Quaternion b)
    {
        if (Quaternion.Dot(a, b) < 0)
        {
            return a * Quaternion.Inverse(Multiply(b, -1));
        }

        else return a * Quaternion.Inverse(b);
    }

    /// <summary>
    /// Calculates the multiplication of a Quaternion by a scalar, such as to alter the scale of a rotation.
    /// </summary>
    /// <param name="input">The Quaternion rotation to be scaled.</param>
    /// <param name="scalar">The scale by which to multiply the rotation.</param>
    /// <returns>The scale adjusted Quaternion.</returns>
    public static Quaternion Multiply(Quaternion input, float scalar)
    {
        return new Quaternion(input.x * scalar, input.y * scalar, input.z * scalar, input.w * scalar);
    }

}

