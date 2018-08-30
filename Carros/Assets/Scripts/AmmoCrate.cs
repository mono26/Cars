using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoCrate : MonoBehaviour {

    [SerializeField]
    private int bulletsAmount;

	void Start () {
		
	}

    private void OnTriggerEnter(Collider col)
    {
        Debug.Log("Ammo received");
        col.SendMessageUpwards("ReceiveAmmo", bulletsAmount);
        Destroy(this.gameObject);
        if (col.gameObject.CompareTag("Player")) 
        {
            Debug.Log("Ammo received");
            col.SendMessageUpwards("ReceiveAmmo", bulletsAmount);
        }
    }
}
