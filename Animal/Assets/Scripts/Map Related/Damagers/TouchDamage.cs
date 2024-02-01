using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchDamage : DamageSource
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.tag == "Player")
        {
            GameManager.Instance.GetDamage(damage, immortalTime, ignoreImmortal, true);
        }
    }
}
