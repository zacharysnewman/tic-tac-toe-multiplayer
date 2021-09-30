using System;
using UnityEngine;

[Serializable]
public struct STouch
{
    public float altitudeAngle;
    public float azimuthAngle;
    public SVector2 deltaPosition;
    public float deltaTime;
    public int fingerID;
    public float maximumPossiblePressure;
    public TouchPhase phase;
    public SVector2 position;
    public float pressure;
    public float radius;
    public float radiusVariance;
    public SVector2 rawPosition;
    public int tapCount;
    public TouchType type;

    public static implicit operator STouch(Touch rValue)
    {
        STouch touch = new STouch();
        touch.altitudeAngle = rValue.altitudeAngle;
        touch.azimuthAngle = rValue.azimuthAngle;
        touch.deltaPosition = rValue.deltaPosition;
        touch.deltaTime = rValue.deltaTime;
        touch.fingerID = rValue.fingerId;
        touch.maximumPossiblePressure = rValue.maximumPossiblePressure;
        touch.phase = rValue.phase;
        touch.position = rValue.position;
        touch.pressure = rValue.pressure;
        touch.radius = rValue.radius;
        touch.radiusVariance = rValue.radiusVariance;
        touch.rawPosition = rValue.rawPosition;
        touch.tapCount = rValue.tapCount;
        touch.type = rValue.type;

        return touch;
    }

    public static implicit operator Touch(STouch rValue)
    {
        Touch touch = new Touch();
        touch.altitudeAngle = rValue.altitudeAngle;
        touch.azimuthAngle = rValue.azimuthAngle;
        touch.deltaPosition = rValue.deltaPosition;
        touch.deltaTime = rValue.deltaTime;
        touch.fingerId = rValue.fingerID;
        touch.maximumPossiblePressure = rValue.maximumPossiblePressure;
        touch.phase = rValue.phase;
        touch.position = rValue.position;
        touch.pressure = rValue.pressure;
        touch.radius = rValue.radius;
        touch.radiusVariance = rValue.radiusVariance;
        touch.rawPosition = rValue.rawPosition;
        touch.tapCount = rValue.tapCount;
        touch.type = rValue.type;

        return touch;
    }
}
