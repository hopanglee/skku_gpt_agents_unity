using System;
using UnityEngine;

public class LocationArea : MonoBehaviour
{
    public TerrianManager.Location location;
    public event Action OnEventInvoked;
}
