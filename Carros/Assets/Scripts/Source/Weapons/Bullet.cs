using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Bullet : MonoBehaviour
{
    [Header("Bullet components")]
    [SerializeField] protected DamageOnHit damageComponent;
    [SerializeField] protected ParticleSystem hitParticlePrefab;

    private void Awake()
    {
        if(damageComponent == null) {
            damageComponent = GetComponent<DamageOnHit>();
        }
        if(hitParticlePrefab == null) {
            hitParticlePrefab = GetComponent<ParticleSystem>();
        }
        return;
    }

    protected void PlayHitParticleInLocation(Vector3 _particleLocation)
    {
        if(HasHitParticle())
        {
            ParticleSystem hitParticle = Instantiate(hitParticlePrefab).GetComponent<ParticleSystem>();
            hitParticle.transform.position = _particleLocation;
            hitParticle.Play();
        }
        return;
    }

    private bool HasHitParticle()
    {
        bool hasHitParticle = true;
        try
        {
            if (hitParticlePrefab == null)
            {
                hasHitParticle = false;
                throw new MissingComponentException(gameObject, typeof(ParticleSystem));
            }
        }
        catch (MissingComponentException missingComponentException) {
            missingComponentException.DisplayException();
        }
        return hasHitParticle;
    }

    public abstract void Fire(Weapon _shootFrom);

    public void DealDamageTo(GameObject _gameObjectToDamage)
    {
        damageComponent.DealDamageIfPosible(_gameObjectToDamage);
        return;
    }
}
