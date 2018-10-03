// Copyright (c) What a Box Creative Studio. All rights reserved.

using UnityEngine;

public class HitScanBullet : Bullet
{
    public override void Fire(Weapon _shooterWeapon)
    {
        RaycastHit bulletHit = HitScanForGameObject(
            _shooterWeapon.GetFirePosition,
            _shooterWeapon.GetAimDirection,
            _shooterWeapon.GetWeaponRange
            );
        if(bulletHit.collider != null)
        {
            DealDamageTo(bulletHit.collider.gameObject);
            //PlayHitParticleInLocation(bulletHit.point);
        }
        return;
    }

    private RaycastHit HitScanForGameObject(Vector3 _firePosition, Vector3 _fireDirection, float _range)
    {
        // Bit shift the index of the layer (8) to get a bit mask
        int layerMask = 1 << 8;
        // This would cast rays only against colliders in layer 8.
        // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
        layerMask = ~layerMask;
        RayParameters rayInfo = new RayParameters(
            layerMask,
            _firePosition,
            _fireDirection,
            _range
            );
        RaycastHit bulletHitInfo = HelperMethods.GetFirstHitInformation(rayInfo);
        return bulletHitInfo;
    }
}
