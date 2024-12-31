using System;
using UnityEngine;

public class LocationArea : MonoBehaviour
{
    public TerrainManager.Location location;
    public event Action OnEventInvoked;
}
