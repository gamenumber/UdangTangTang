using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Climb : Interaction
{
    [SerializeField] float upspeed;
    public Rigidbody2D Player;
    [SerializeField] bool climbing = false;
    public override void Start()
    {
        base.Start();
        requireType = true;
        requiredType = AnimalType.monkey;
        Player = GameManager.Instance.M_PlayerMovements.rb;
    }
    private void Update()
    {
        if (climbing)
        {
            Player.velocityY = upspeed;
        }
    }


    // 1스테이지 중간에 옆으로 걸쳐진 나무 타는걸 실패하면 옆에 서 있는 큰나무 탈 수 있게끔 해놓음.
    public override void Interact()
    {

        base.Interact();
        if (!climbing) climbing = true;
        else climbing = false;
    }
    public override void OnTriggerExit2D(Collider2D other)
    {
        climbing = false;
        base.OnTriggerExit2D(other);
    }
}
