using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicTurret : Turret
{
    public override void Update()
    {
        base.Update();
        if (lockedOn && counter >= fireRate)
        {
            counter = 0.0f;
            TurretBullet bul = Instantiate(bullet, firePoint.position, firePoint.rotation).GetComponent<TurretBullet>();
            bul.damage = damage;
            bul.immortalTime = immortalTime;
            bul.ignoreImmortal = ignoreImmortal;
            bul.transform.GetComponent<Rigidbody2D>().velocity = dir * bulletSpeed;
        }
    }
}
