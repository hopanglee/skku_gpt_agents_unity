using System.Collections.Generic;
using Unity.Entities.UniversalDelegates;
using Unity.Profiling;
using UnityEngine;

public static class TerrainManager
{
    public enum LocationTag
    {
        AdamHouse,
        Park,
        River,
        EveHouse,
    }

    public struct LocationInfo
    {
        public LocationArea area;
        public Transform transform;
    }

    private static Dictionary<LocationTag, LocationInfo> locationDic = new();

    public static void AddLocation(LocationTag location, LocationInfo locInfo)
    {
        if (!locationDic.ContainsKey(location))
        {
            locationDic.Add(location, locInfo);
        }
    }

    public static Vector3 LocationToVector(LocationTag location)
    {
        if (locationDic.ContainsKey(location))
            return locationDic[location].transform.position;
        else
        {
            Debug.LogError("Location Dictionary have not the key.");
            return Vector3.zero;
        }
    }

    public static Transform LocationToTransform(LocationTag location)
    {
        if (locationDic.ContainsKey(location))
            return locationDic[location].transform;
        else
        {
            Debug.LogError("Location Dictionary have not the key.");
            return null;
        }
    }

    public static LocationArea LocationToArea(LocationTag location)
    {
        if (locationDic.ContainsKey(location))
            return locationDic[location].area;
        else
        {
            Debug.LogError("Location Dictionary have not the key.");
            return null;
        }
    }
}
