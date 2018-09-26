using System.Collections;
using UnityEngine;

[System.Serializable]
public class WeaponStats
{
    [SerializeField] private int ammoCapacity = 180;
    [SerializeField] private int magazineSize = 30;
    [SerializeField] private float range = 1000f;
    [SerializeField] private int ratio = 1;    // Shots per second.
    [SerializeField] private float reloadTime = 1.5f;   // In seconds.

    public int GetAmmoCapacity { get { return ammoCapacity; } }
    public int GetMagazineSize { get { return magazineSize; } }
    public float GetRange { get { return range; } }
    public int GetRatio { get { return ratio; } }
    public float GetReloadTime { get { return reloadTime; } }
}

public abstract class Weapon : EntityComponent
{
    [Header("Weapon settings")]
    private WeaponStats stats;

    [Header("Weapon components")]
    //public Text bulletsGUI;
    [SerializeField] private Bullet bulletToShoot;
    [SerializeField] private ParticleSystem shootingParticle;
    [SerializeField] private GameObject targetFeedbackImg;
    [SerializeField] private Transform weaponBarrel;
    [SerializeField] Transform playerCamera;

    [Header("Weapon editor debugging")]
    [SerializeField] private int actualAmmo;
    [SerializeField] private int magazineBullets;
    [SerializeField] private bool isReloading;
    [SerializeField] private bool canShoot;
    [SerializeField] private float fireInput;
    [SerializeField] private float reloadInput;
    [SerializeField] private float aimInput;

    public Vector3 GetFirePosition { get { return weaponBarrel.position; } }
    public Vector3 GetAimDirection { get { return weaponBarrel.forward; } }
    public float GetWeaponRange { get { return stats.GetRange; } }

    // Use this for initialization
    private void Start()
    {
        targetFeedbackImg.SetActive(false);
        actualAmmo = stats.GetAmmoCapacity;
        magazineBullets = stats.GetMagazineSize;
        playerCamera = Camera.main.GetComponent<Transform>();
        shootingParticle = GetComponentInChildren<ParticleSystem>();
        canShoot = true;
        //bulletsGUI.text = magazineBullets.ToString() + "/" + actualAmmo;
        return;
    }

    private void HandleInput()
    {
        if (Input.GetAxis("Fire1") != 0 && canShoot && magazineBullets > 0) {
            StartCoroutine(Shoot());
        }
        if (Input.GetAxis("Aim") != 0) {
            transform.forward = playerCamera.forward;
        }
        else if (Input.GetAxis("Aim") == 0) {
            transform.rotation = Quaternion.identity;
        }
        return;
    }

    private IEnumerator ShootFeedback()
    {
        targetFeedbackImg.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        targetFeedbackImg.SetActive(false);
        yield break;
    }

    private IEnumerator Reload()
    {
        Debug.Log("Reloading");
        canShoot = false;
        yield return new WaitForSeconds(stats.GetReloadTime);
        if (actualAmmo > stats.GetMagazineSize)
        {
            magazineBullets = stats.GetMagazineSize;
            actualAmmo -= stats.GetMagazineSize;
        }
        else
        {
            magazineBullets = actualAmmo;
            actualAmmo -= actualAmmo;
        }
        canShoot = true;
        yield break;
    }

    protected virtual IEnumerator Shoot()
    {
        //bulletsGUI.text = magazineBullets.ToString() + "/" + actualAmmo;
        shootingParticle.Play();
        bulletToShoot.Fire(this);
        magazineBullets--;
        canShoot = false;
        if (magazineBullets <= 0) {
            StartCoroutine(Reload());
        }
        yield return new WaitForSeconds(stats.GetRatio);
        canShoot = true;
        yield break;
    }

    public void GiveAmmo(int ammo)
    {
        if (actualAmmo < stats.GetAmmoCapacity)
        {
            actualAmmo += ammo;
            //actualAmmo -= (actualAmmo - stats.GetAmmoCapacity);
            //bulletsGUI.text = magazineBullets.ToString() + "/" + actualAmmo;
        }
        return;
    }
}
