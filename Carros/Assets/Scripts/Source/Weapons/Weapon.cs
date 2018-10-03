// Copyright (c) What a Box Creative Studio. All rights reserved.

using System.Collections;
using UnityEngine;

public class WeaponInput
{
    private float fireInput;
    private float reloadInput;
    private float aimInput;

    public float GetFireInput { get { return fireInput; } }
    public float GetReloadInput { get { return reloadInput; } }
    public float GetAimInput { get { return aimInput; } }

    public WeaponInput(float _fireInput, float _reloadInput, float _aimInput)
    {
        fireInput = _fireInput;
        reloadInput = _reloadInput;
        aimInput = _aimInput;
        return;
    }
}

[System.Serializable]
public class WeaponStats
{
    [SerializeField] private int ammoCapacity = 180;
    [SerializeField] private int magazineSize = 30;
    [SerializeField] private float range = 1000f;
    [SerializeField] private int shotsPerSecond = 1;    // Shots per second.
    [SerializeField] private float reloadTime = 1.5f;   // In seconds.

    public int GetAmmoCapacity { get { return ammoCapacity; } }
    public int GetMagazineSize { get { return magazineSize; } }
    public float GetRange { get { return range; } }
    public int GetShotsPerSecond { get { return shotsPerSecond; } }
    public float GetReloadTime { get { return reloadTime; } }

    public WeaponStats()
    {

    }
}

public class Weapon : EntityComponent, EventHandler<PickEvent>
{
    [Header("Weapon settings")]
    [SerializeField] private WeaponStats stats = new WeaponStats();

    [Header("Weapon components")]
    //public Text bulletsGUI;
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private ParticleSystem shootParticle;
    [SerializeField] private GameObject targetFeedbackImg;
    [SerializeField] private Transform weaponBarrel;
    [SerializeField] Transform playerCamera;

    [Header("Weapon editor debugging")]
    [SerializeField] private int actualAmmo;
    [SerializeField] private int magazineBullets;
    [SerializeField] private bool isReloading;
    [SerializeField] private bool canShoot;
    [SerializeField] private WeaponInput currentInput;

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
        shootParticle = GetComponentInChildren<ParticleSystem>();
        canShoot = true;
        //bulletsGUI.text = magazineBullets.ToString() + "/" + actualAmmo;
        return;
    }

    public override void FixedFrame()
    {
        HandleInput();
        return;
    }

    private void HandleInput()
    {
        if (Input.GetAxis("Fire1") != 0 && canShoot && magazineBullets > 0) {
            StartCoroutine(Shoot());
        }
        // TODO refactorization, weapon should't rotate itself. Other component must do it, weapon targetter for ex.
        if (Input.GetAxis("Aim") != 0) {
            HelperMethods.DebugMessageWithTimeStamp("Weapon is aiming");
            transform.forward = playerCamera.forward;
        }
        else if (Input.GetAxis("Aim") == 0) {
            transform.localRotation = Quaternion.identity;
        }
        return;
    }

    private void OnEnable()
    {
        EventManager.AddListener<PickEvent>(this);
        return;
    }

    private void OnDisable()
    {
        EventManager.RemoveListener<PickEvent>(this);
        return;
    }

    private IEnumerator ShootFeedback()
    {
        targetFeedbackImg.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        targetFeedbackImg.SetActive(false);
        yield break;
    }

    protected virtual IEnumerator Shoot()
    {
        //bulletsGUI.text = magazineBullets.ToString() + "/" + actualAmmo;
        PlayShootParticle();
        FireBullet();
        canShoot = false;
        if (magazineBullets <= 0)
        {
            StartCoroutine(Reload());
        }
        yield return new WaitForSeconds(1 / stats.GetShotsPerSecond);
        canShoot = true;
        yield break;
    }

    private void PlayShootParticle()
    {
        if (HasShootParticleComponent())
        {
            shootParticle.transform.forward = GetAimDirection;
            this.shootParticle.Play();
        }
        return;
    }

    private bool HasShootParticleComponent()
    {
        bool hasParticle = true;
        try
        {
            if (shootParticle == null)
            {
                hasParticle = false;
                throw new MissingComponentException(gameObject, typeof(ParticleSystem));
            }
        }
        catch (MissingComponentException missingComponentException) {
            missingComponentException.DisplayException();
        }
        return hasParticle;
    }

    private void FireBullet()
    {
        if(HasBulletPrefab())
        {
            Bullet bulletToShoot = Instantiate(bulletPrefab);
            bulletToShoot.Fire(this);
            magazineBullets--;
        }
        return;
    }

    private bool HasBulletPrefab()
    {
        bool hasBullet = true;
        try
        {
            if(bulletPrefab == null)
            {
                hasBullet = false;
                throw new MissingComponentException(gameObject, typeof(Bullet));
            }
        }
        catch (MissingComponentException missingComponentException) {
            missingComponentException.DisplayException();
        }
        return hasBullet;
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

    public void OnEvent(PickEvent _pickEvent)
    {
        if (_pickEvent.GetWhoPicks.Equals(this))
        {
            switch (_pickEvent.GetPickableType)
            {
                case PickableType.Ammo:
                    GiveAmmo(_pickEvent.GetAmmoPicked);
                    break;
                default:
                    break;
            }
        }
        return;
    }
}
