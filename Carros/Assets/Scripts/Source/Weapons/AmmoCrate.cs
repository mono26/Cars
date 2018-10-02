using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PickableType { Ammo }

public class PickEvent : GameEvent
{
    private Entity whoPicks;
    private PickableType pickableType;
    private int ammoPicked;

    public Entity GetWhoPicks { get { return whoPicks; } }
    public PickableType GetPickableType { get { return pickableType; } }
    public int GetAmmoPicked { get { return ammoPicked; } }

    public PickEvent(Entity _whoPicks, PickableType _pickableType)
    {
        whoPicks = _whoPicks;
        pickableType = _pickableType;
        return;
    }
}

public class AmmoCrate : MonoBehaviour {

    [SerializeField]
    private int bulletsAmount;

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Player")) 
        {
            Debug.Log("Ammo received");
            EventManager.TriggerEvent<PickEvent>(new PickEvent(col.GetComponent<Entity>(), PickableType.Ammo));
            Destroy(this.gameObject);
        }
        return;
    }
}
