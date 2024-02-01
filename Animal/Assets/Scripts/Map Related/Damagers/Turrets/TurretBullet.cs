using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretBullet : DamageSource
{
    float counter = 0.0f;
    private void Update()
    {
        counter += Time.deltaTime;
        if (counter >= 5.0f) Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            GameManager.Instance.GetDamage(damage, immortalTime, ignoreImmortal, true);
        }
        if(collision.gameObject.tag != "Turret" && collision.gameObject.tag != "Bullet") Destroy(gameObject);
    }
}
