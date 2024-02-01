using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : DamageSource
{
    [SerializeField] protected float fireRate, bulletSpeed;
    [SerializeField] protected Transform rotatePart, firePoint;
    [SerializeField] protected GameObject bullet;
    protected Transform player;
    protected TurretTarget target;
    protected bool lockedOn = false;
    protected float counter = 0.0f;
    protected Vector2 dir;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (player == null)
            {
                player = collision.transform;
                target = player.GetComponent<TurretTarget>();
            }
            if (target.targetted.Contains(this) == false)
            {
                target.targetted.Add(this);
                target.TargetUpdate();
            }
            lockedOn = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (target.targetted.Contains(this))
            {
                target.targetted.Remove(this);
                target.TargetUpdate();
            }
            lockedOn = false;
        }
    }
    public virtual void Update()
    {
        if (counter < fireRate) counter += Time.deltaTime;
        if(lockedOn)
        {
            dir = player.position - rotatePart.position;
            dir.Normalize();
            rotatePart.rotation = Quaternion.FromToRotation(Vector3.up, dir);
        }
    }
}
