using System;
using UnityEngine;

[Serializable]
public struct SAccelerationEvent
{
    public SVector3 acceleration;
    public float deltaTime;

    public SAccelerationEvent(SVector3 newAcceleration, float newDeltaTime)
    {
        acceleration = newAcceleration;
        deltaTime = newDeltaTime;
    }

    public override string ToString()
    {        
        return string.Format("Acceleration: {0}, Delta Time: {1}", acceleration, deltaTime);
    }

    public static implicit operator SAccelerationEvent(AccelerationEvent rValue)
    {
        return new SAccelerationEvent(rValue.acceleration, rValue.deltaTime);
    }
}
