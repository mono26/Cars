using UnityEngine;

public class DamageOnHit : MonoBehaviour
{
    [Header("Damage On Hit settings")]
    [SerializeField] private int damageToApplyWhenHit = 1;
    [SerializeField] private LayerMask[] layersToHit;
    [SerializeField] private string[] tagsToHit = new string[] { "Enemy" };

    private void OnCollisionEnter(Collision collision)
    {
        DealDamageIfPosible(collision.gameObject);
        return;
    }

    private bool CanDealDamageTo(GameObject _gameObjectToCheck)
    {
        bool canDealDamage = false;
        if(_gameObjectToCheck != null && HasProperLayerMask(_gameObjectToCheck) && HasProperTag(_gameObjectToCheck)) {
            canDealDamage = true;
        }
        return canDealDamage;
    }

    private bool HasProperLayerMask(GameObject _gameObjectToCheck)
    {
        bool hasProperLayer = false;
        if(_gameObjectToCheck != null && HasAssignedLayersToDamage())
        {
            foreach(LayerMask layer in layersToHit)
            {
                if(layer == _gameObjectToCheck.layer)
                {
                    hasProperLayer = true;
                    break;
                }
            }
        }
        return hasProperLayer;
    }

    private bool HasAssignedLayersToDamage()
    {
        bool hasLayerToDamage = true;
        if(layersToHit == null || layersToHit.Length == 0) {
            hasLayerToDamage = false;
        }
        return hasLayerToDamage;
    }

    private bool HasProperTag(GameObject _gameObjectToCheck)
    {
        bool hasProperTags = false;
        if (_gameObjectToCheck != null && HasAssignedLayersToDamage())
        {
            foreach (LayerMask layer in layersToHit)
            {
                if (layer == _gameObjectToCheck.layer)
                {
                    hasProperTags = true;
                    break;
                }
            }
        }
        return hasProperTags;
    }

    private bool HasAssignedTagstoDamage()
    {
        bool hasTagsToDamage = true;
        if(tagsToHit == null || tagsToHit.Length == 0) {
            hasTagsToDamage = false;
        }
        return hasTagsToDamage;
    }

    public void DealDamageIfPosible(GameObject _gameObjectToDamage)
    {
        if (_gameObjectToDamage != null && CanDealDamageTo(_gameObjectToDamage))
        {
            HealthComponent health = _gameObjectToDamage.GetComponent<HealthComponent>();
            if (health != null)
            {
                health.ReceiveDamage(damageToApplyWhenHit);
            }
        }
        return;
    }
}
