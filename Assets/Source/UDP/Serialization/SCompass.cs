using System;
using UnityEngine;

[Serializable]
public struct SCompass
{
    public bool enabled;
    public float headingAccuracy;
    public float magneticHeading;
    public SVector3 rawVector;
    public double timestamp;
    public float trueHeading;

    public static implicit operator SCompass(Compass rValue)
    {
        SCompass compass = new SCompass();
        compass.enabled = rValue.enabled;
        compass.headingAccuracy = rValue.headingAccuracy;
        compass.magneticHeading = rValue.magneticHeading;
        compass.rawVector = rValue.rawVector;
        compass.timestamp = rValue.timestamp;
        compass.trueHeading = rValue.trueHeading;

        return compass;
    }
}
