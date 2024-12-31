using System;
using UnityEngine;

public class LocationArea : MonoBehaviour
{
    public TerrainManager.Location location;
    public event Action OnEventInvoked;

    [SerializeField]
    private Transform position;

    private void Awake() {
        TerrainManager.AddLocation(location, position);
    }
}
