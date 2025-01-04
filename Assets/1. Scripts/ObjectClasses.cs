using UnityEngine;

public class PositionableObject : MonoBehaviour
{
    public TerrainManager.LocationTag Area;

    protected virtual void Awake()
    {
        SetAreaInit();
    }

    private void SetAreaInit()
    {
        Collider[] colliders = Physics.OverlapBox(transform.position, transform.localScale / 2);
        foreach (var other in colliders)
        {
            if (other.CompareTag("Area"))
            {
                Debug.Log($"Already in Area: {other.name}");
                var Area = other.GetComponent<LocationArea>();
                this.Area = Area.location;
                Area.Enter(this);
            }
        }
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Area"))
        {
            var Area = other.GetComponent<LocationArea>();
            this.Area = Area.location;
            Area.Enter(this);
            Debug.Log($"Area Enter : {Area.name}");
        }
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Area"))
        {
            var Area = other.GetComponent<LocationArea>();
            Area.Exit(this);
            Debug.Log($"Area Exit : {Area.name}");
        }
    }
}
