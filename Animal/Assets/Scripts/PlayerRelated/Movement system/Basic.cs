using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basic : Movement
{
    private void Update()
    {
        #region GroundScan
        Vector3 offSet = new Vector3(0.0f, -0.01f, 0.0f);
        RaycastHit2D hit = Physics2D.BoxCast(collider.bounds.center + offSet, collider.bounds.size, 0.0f, Vector2.down, 0.02f, whatToScan);
        if (hit.collider != null)
        {
            controller.grounded = true;
            if (controller.antiAirMove)
            {
                controller.antiAirMove = false;
                rb.velocityX = 0.0f;
            }
        }
        else controller.grounded = false;
        anim.SetBool("Grounded", controller.canDisableJump && controller.grounded);
        anim.SetBool("Moving", controller.pressingRight||controller.pressingLeft);
        #endregion
        #region GravityScales
        if (rb.velocityY < 0) rb.gravityScale = controller.fallGravity;
        else rb.gravityScale = controller.riseGravity;
        #endregion
    }
    public override void MoveRight()
    {
        base.MoveRight();
        RaycastHit2D hit = Physics2D.BoxCast(collider.bounds.center, collider.bounds.size, 0.0f, Vector2.right, 0.1f, whatToScan);
        if (hit.collider == null) transform.position += Vector3.right * Time.deltaTime * moveSpeed;
        renderer.flipX = false;
    }
    public override void MoveLeft()
    {
        base.MoveLeft();
        RaycastHit2D hit = Physics2D.BoxCast(collider.bounds.center, collider.bounds.size, 0.0f, Vector2.left, 0.1f, whatToScan);
        if (hit.collider == null) transform.position += Vector3.left * Time.deltaTime * moveSpeed;
        renderer.flipX = true;
    }
    public override void Jump()
    {
        base.Jump();
        if (controller.grounded)
        {
            controller.grounded = false;
            controller.canDisableJump = false;
            anim.SetBool("Grounded", false);
            anim.Play("jump");
            rb.velocityY = jumpPower;
        }
    }
}
