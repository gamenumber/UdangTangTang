using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pit : DamageSource
{
    public Transform returnLocation;
    public float waitTime;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            GameManager.Instance.GetDamage(damage, 0.0f, true, false);
            GameManager.Instance.freeCam = true;
            StartCoroutine(WaitThenReturn());
        }
    }
    IEnumerator WaitThenReturn()
    {
        yield return new WaitForSeconds(waitTime);
        GameManager.Instance.M_PlayerMovements.rb.velocity = Vector2.zero;
        GameManager.Instance.player.transform.position = returnLocation.position;
        GameManager.Instance.freeCam = false;
    }
}
