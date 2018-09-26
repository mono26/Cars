using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : Bullet
{
    [Header("Rocket settings")]
    [SerializeField] private float explosionRadius;
    [SerializeField] private float launchForce;

    [Header("Rocket components")]
    [SerializeField] private Rigidbody rocketBody;
    [SerializeField] private ParticleSystem explosionVfx;

    private void Awake()
    {
        if(rocketBody == null) {
            rocketBody = GetComponent<Rigidbody>();
        }
        return;
    }

    private void LaucnhInDirection(Vector3 _launchDirection)
    {
        if (HasBodyComponent())
        {
            _launchDirection = _launchDirection.normalized;
            rocketBody.AddForce(_launchDirection * launchForce, ForceMode.Impulse);
        }
        return;
    }

    private bool HasBodyComponent()
    {
        bool hasBody = true;
        try
        {
            if (rocketBody == null)
            {
                hasBody = false;
                throw new MissingComponentException("The rocket has no body component:");
            }
        }
        catch (MissingComponentException missingComponentException) {
            missingComponentException.DisplayException();
        }
        return hasBody;
    }

    // Update is called once per frame
    private void OnCollisionEnter(Collision col)
    {
        ExplosionDamage();
        RocketPool.Instance.ReleaseRocket(rocketBody);
        return;
    }

    private void ExplosionDamage()
    {
        //GameObject explosion = VfxPool.Instance.GetVFX();
        //explosion.transform.position = this.transform.position;
        Collider[] colliders = Physics.OverlapSphere(this.transform.position, explosionRadius);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].CompareTag("Enemy"))
                colliders[i].SendMessage("ReceiveDamage", damage);
        }
    }
}
