using System;
using UnityEngine;
/// <summary>
/// Since unity doesn't flag the Vector2 as serializable, we
/// need to create our own version. This one will automatically convert
/// between Vector2 and SerializableVector2
/// </summary>
[Serializable]
public struct SVector2
{
    /// <summary>
    /// x component
    /// </summary>
    public float x;

    /// <summary>
    /// y component
    /// </summary>
    public float y;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="rX"></param>
    /// <param name="rY"></param>
    public SVector2(float rX, float rY)
    {
        x = rX;
        y = rY;
    }

    /// <summary>
    /// Returns a string representation of the object
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return string.Format("[{0}, {1}]", x, y);
    }

    /// <summary>
    /// Automatic conversion from SerializableVector2 to Vector2
    /// </summary>
    /// <param name="rValue"></param>
    /// <returns></returns>
    public static implicit operator Vector2(SVector2 rValue)
    {
        return new Vector2(rValue.x, rValue.y);
    }

    /// <summary>
    /// Automatic conversion from Vector2 to SerializableVector2
    /// </summary>
    /// <param name="rValue"></param>
    /// <returns></returns>
    public static implicit operator SVector2(Vector2 rValue)
    {
        return new SVector2(rValue.x, rValue.y);
    }
}
