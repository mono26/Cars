using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MachineGun : Weapon
{
	// Update is called once per frame
	void FixedUpdate () {
        if (Input.GetAxis("Fire1") != 0 && canShoot && magazineBullets > 0)
        {
            //StartCoroutine(Shoot());
        }
        if (Input.GetAxis("Aim") != 0)
        {
            //pTransform.RotateAround(pCamera.forwardthis.transform.position );
            transform.forward = camera.forward;
            
        }
      /*  else if (Input.GetAxis("Aim") == 0)
        {
            
            pTransform.rotation = originalPosition;
        }*/
        

    }
}
