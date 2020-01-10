using UnityEngine;

/// <summary>
/// Extensions for rotations
/// </summary>
public static class RotationExtensions
{
    #region SetRotation2D
    /// <summary>
    /// Sets the 2D rotation of the transform to look towards a target
    /// </summary>
    /// <param name="transform">The transform</param>
    /// <param name="target">The target</param>
    public static void SetRotation2D(this Transform transform, Vector3 target)
        => SetRotation2D(transform, transform.position, target);
    #endregion

    #region SetRotation2D
    /// <summary>
    /// Sets the 2D rotation of the transform from a "look towards"
    /// </summary>
    /// <param name="transform">The transform</param>
    /// <param name="from">The starting point</param>
    /// <param name="to">The ending point</param>
    public static void SetRotation2D(this Transform transform, Vector3 from, Vector3 to)
        => transform.rotation = Quaternion.Euler(0, 0, GetRotationDegrees(from.Direction(to)));
    #endregion

    #region SetVelocity
    /// <summary>
    /// Sets the velocity of the rigidbody2D from a target and speed
    /// </summary>
    /// <param name="rb">The rigidbody2D</param>
    /// <param name="target">The target</param>
    /// <param name="speed">The speed</param>
    public static void SetVelocity(this Rigidbody2D rb, Vector3 target, float speed)
        => rb.velocity = rb.transform.position.Direction(target) * speed;
    #endregion

    #region SetRotation
    /// <summary>
    /// Sets the rotation of the rigidbody2D from its current velocity
    /// </summary>
    /// <param name="rb">The rigidbody2D</param>
    public static void SetRotation(this Rigidbody2D rb)
        => rb.rotation = GetRotationDegrees(rb.velocity) - 90;
    #endregion

    #region SetVelocityAndRotation
    /// <summary>
    /// Sets the velocity and then the rotation of the rigidbody2D from a target and speed
    /// </summary>
    /// <param name="rb">The rigidbody2D</param>
    /// <param name="target">The target</param>
    /// <param name="speed">The speed</param>
    public static void SetVelocityAndRotation(this Rigidbody2D rb, Vector3 target, float speed)
    {
        SetVelocity(rb, target, speed);
        SetRotation(rb);
    }
    #endregion

    #region Direction
    /// <summary>
    /// Gets the direction between the two points [normalized vector]
    /// </summary>
    /// <param name="from">The starting point</param>
    /// <param name="to">The ending point</param>
    /// <returns>The direction between the two points</returns>
    private static Vector3 Direction(this Vector3 from, Vector3 to)
        => (to - from).normalized;
    #endregion

    #region GetRotationDegrees
    /// <summary>
    /// Gets the [2D] rotation in degrees of the direction
    /// </summary>
    /// <param name="direction">The direction</param>
    /// <returns>The [2D] rotation in degrees of the direction</returns>
    private static float GetRotationDegrees(Vector3 direction)
        => Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    #endregion
}
