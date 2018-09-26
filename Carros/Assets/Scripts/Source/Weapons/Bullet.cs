using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Bullet : MonoBehaviour
{
    [Header("Bullet settings")]
    protected int damage;

    [Header("Bullet components")]
    protected ParticleSystem hitParticle;

    [Header("Bullet editor debugging")]
    private Weapon shooter;

    private void WhoShot(Weapon _shooter)
    {
        shooter = _shooter;
        return;
    }

    protected void DealDamageIfPosible(GameObject _gameObjectToDamage)
    {
        if(_gameObjectToDamage != null)
        {
            HealthComponent health = _gameObjectToDamage.GetComponent<HealthComponent>();
            if (health != null) {
                health.ReceiveDamage(damage);
            }
        }
        return;
    }

    protected void PlayHitParticleInLocation(Vector3 _particleLocation)
    {
        if(HasHitParticle()) {
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
            if (hitParticle == null)
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

    public virtual void Fire(Weapon _shootFrom)
    {
        WhoShot(_shootFrom);
        return;
    }
}
