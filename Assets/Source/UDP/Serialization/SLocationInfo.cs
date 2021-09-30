using System;
using UnityEngine;

[Serializable]
public struct SLocationInfo
{
    public float altitude;
    public float horizontalAccuracy;
    public float latitude;
    public float longitude;
    public double timestamp;
    public float verticalAccuracy;

    public static implicit operator SLocationInfo(LocationInfo rValue)
    {
        SLocationInfo locationInfo = new SLocationInfo();
        locationInfo.altitude = rValue.altitude;
        locationInfo.horizontalAccuracy = rValue.horizontalAccuracy;
        locationInfo.latitude = rValue.latitude;
        locationInfo.longitude = rValue.longitude;
        locationInfo.timestamp = rValue.timestamp;
        locationInfo.verticalAccuracy = rValue.verticalAccuracy;

        return locationInfo;
    }
}
