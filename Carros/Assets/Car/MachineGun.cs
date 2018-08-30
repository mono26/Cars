using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MachineGun : MonoBehaviour {

    public float bulletDamage;
    public int ammoCapacity = 180;
    public int actualAmmo;
    public int magazineBullets;
    public int magazineSize;
    public float ratio;
    public float range;
    public bool isReloading;
    public float reloadTime;
    public bool canShoot;

    private Transform pTransform; 
    private Quaternion originalPosition;
    private ParticleSystem shootingParticle;
    private Transform pCamera;

    public Transform weaponBarrel;

    
    public GameObject targetFeedbackImg;

    public Text bulletsGUI;

    // Use this for initialization
    void Start () {
        
        
        targetFeedbackImg.SetActive(false);
        actualAmmo = ammoCapacity;
        magazineBullets = magazineSize;
        pTransform = this.GetComponent<Transform>();
        originalPosition = new Quaternion(0,0,0,0);
        pCamera = Camera.main.GetComponent <Transform>();
        shootingParticle = GetComponentInChildren<ParticleSystem>();
        canShoot = true;
        bulletsGUI.text = magazineBullets.ToString() + "/" + actualAmmo;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (Input.GetAxis("Fire1") != 0 && canShoot && magazineBullets > 0)
        {
            StartCoroutine(Shoot());
        }
        if (Input.GetAxis("Aim") != 0)
        {
            //pTransform.RotateAround(pCamera.forwardthis.transform.position );
            pTransform.forward = pCamera.forward;
            
        }
      /*  else if (Input.GetAxis("Aim") == 0)
        {
            
            pTransform.rotation = originalPosition;
        }*/
        

    }

    public void ReceiveAmmo(int ammo)
    {
        if (actualAmmo < ammoCapacity)
        {
            actualAmmo += ammo; 
            actualAmmo -= (actualAmmo - ammoCapacity);
            bulletsGUI.text = magazineBullets.ToString() + "/" + actualAmmo;
        }
    }

    IEnumerator ShootFeedback()
    {
        targetFeedbackImg.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        targetFeedbackImg.SetActive(false);
    }

    IEnumerator Shoot()
    {
        bulletsGUI.text = magazineBullets.ToString() + "/" + actualAmmo;
        shootingParticle.Play();
        // Bit shift the index of the layer (8) to get a bit mask
        int layerMask = 1 << 8;

        // This would cast rays only against colliders in layer 8.
        // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
        layerMask = ~layerMask;

        RaycastHit hit;

        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(weaponBarrel.position, weaponBarrel.TransformDirection(Vector3.forward), out hit, range, layerMask))
        {
            Debug.DrawRay(weaponBarrel.position, weaponBarrel.TransformDirection(Vector3.forward) * range, Color.yellow);
            Debug.Log("Did Hit");
            StartCoroutine("ShootFeedback");
            canShoot = false;
            Debug.Log(hit.transform.position);

            if (hit.collider.gameObject.GetComponent<HealthComponent>() != null)
            {
                Debug.Log("Damage received");
                hit.collider.gameObject.GetComponent<HealthComponent>().ReceiveDamage(bulletDamage);
            }
        }
        else
        {
            Debug.DrawRay(weaponBarrel.position, weaponBarrel.TransformDirection(Vector3.forward) * range, Color.white);
            Debug.Log("Did not Hit");
            canShoot = false;
        }

        magazineBullets--;
        if(magazineBullets <= 0)
        {
            StartCoroutine("Reload");
        }
        yield return new WaitForSeconds(ratio);
        canShoot = true;


    }

    IEnumerator Reload()
    {
        Debug.Log("Reloading");
        canShoot = false;
        yield return new WaitForSeconds(reloadTime);
        if(actualAmmo > magazineSize)
        {
            magazineBullets = magazineSize;
            actualAmmo -= magazineSize;
        } else
        {
            magazineBullets = actualAmmo;
            actualAmmo -= actualAmmo;
        }
        
        canShoot = true;
    }
}
