using System;
using UnityEngine;

[Serializable]
public struct SLocationService
{
    public bool isEnabledByUser;
    public SLocationInfo lastData;
    public LocationServiceStatus status;

    public static implicit operator SLocationService(LocationService rValue)
    {
        SLocationService locationService = new SLocationService();
        locationService.isEnabledByUser = rValue.isEnabledByUser;
        locationService.status = rValue.status;
        locationService.lastData = locationService.status == LocationServiceStatus.Running ? rValue.lastData : locationService.lastData;

        return locationService;
    }
}
