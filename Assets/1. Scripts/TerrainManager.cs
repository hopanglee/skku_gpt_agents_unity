using System.Collections.Generic;
using Unity.Entities.UniversalDelegates;
using UnityEngine;

public class TerrainManager : MonoBehaviour
{
    public static TerrainManager Instance { get; set; }

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

    [SerializeField]
    private List<LocationPos> locationPosList = new();
    private Dictionary<Location, Transform> locationDic;

    private void Awake()
    {
        Instance = this;

        // 리스트를 딕셔너리로 변환
        ConvertListToDictionary();
    }

    private void ConvertListToDictionary()
    {
        locationDic = new Dictionary<Location, Transform>();

        foreach (var locationPos in locationPosList)
        {
            if (!locationDic.ContainsKey(locationPos.loc))
            {
                locationDic.Add(locationPos.loc, locationPos.transform);
            }
            else
            {
                Debug.LogWarning(
                    $"Duplicate Location Key Found: {locationPos.loc}. Ignoring this entry."
                );
            }
        }
    }

    public Vector3 GetLocationVector(Location location)
    {
        return locationDic[location].position;
    }
}
