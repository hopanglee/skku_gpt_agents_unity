using System.Collections.Generic;
using Unity.Entities.UniversalDelegates;
using UnityEngine;

public static class TerrainManager
{
    public enum Location
    {
        AdamHome,
        Park,
        River,
        EveHouse,
    }

    [System.Serializable]
    public struct LocationPos
    {
        public Location loc;
        public Transform transform;
    }

    private static Dictionary<Location, Transform> locationDic = new();

    public static void AddLocation(Location location, Transform transform)
    {
        if (!locationDic.ContainsKey(location))
        {
            locationDic.Add(location, transform);
        }
    }

    public static Vector3 GetLocationVector(Location location)
    {
        if (locationDic.ContainsKey(location))
            return locationDic[location].position;
        else
        {
            Debug.LogError("Location Dictionary have not the key.");
            return Vector3.zero;
        }
    }

    public static Transform GetLocationTransform(Location location)
    {
        if (locationDic.ContainsKey(location))
            return locationDic[location];
        else
        {
            Debug.LogError("Location Dictionary have not the key.");
            return null;
        }
    }
}
