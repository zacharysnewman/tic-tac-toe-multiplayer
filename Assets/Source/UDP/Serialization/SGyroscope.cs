using System;
using UnityEngine;

[Serializable]
public struct SGyroscope
{
    public SQuaternion attitude;
    public bool enabled;
    public SVector3 gravity;
    public SVector3 rotationRate;
    public SVector3 rotationRateUnbiased;
    public float updateInterval;
    public SVector3 userAcceleration;

    public static implicit operator SGyroscope(Gyroscope rValue)
    {
        SGyroscope gyro = new SGyroscope();
        gyro.attitude = rValue.attitude;
        gyro.enabled = rValue.enabled;
        gyro.gravity = rValue.gravity;
        gyro.rotationRate = rValue.rotationRate;
        gyro.rotationRateUnbiased = rValue.rotationRateUnbiased;
        gyro.updateInterval = rValue.updateInterval;
        gyro.userAcceleration = rValue.userAcceleration;

        return gyro;
    }
}
