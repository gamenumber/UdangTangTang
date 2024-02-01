using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glide : Movement
{
    [SerializeField] Basic basicMoveset;
    public float glideFallSpeed, glideMoveSpeed, glideJumpPower, glideJumpCooldown, maxGlideSpeed;
    public int glideJumpCount;

    int jumpCounter = 0;
    float timer = 0.0f;
    public override void OnEnable()
    {
        base.OnEnable();
        jumpCounter = 0;
        rb.gravityScale = controller.riseGravity;
    }
    private void Update()
    {
        #region GroundScan
        Vector3 offSet = new Vector3(0.0f, -0.01f, 0.0f);
        RaycastHit2D hit = Physics2D.BoxCast(collider.bounds.center + offSet, collider.bounds.size, 0.0f, Vector2.down, 0.02f, whatToScan);
        if (hit.collider != null)
        {
            UnGlide();
        }
        #endregion
        #region RendererFlip
        if (rb.velocityX >= 0.0f) renderer.flipX = false;
        else renderer.flipX = true;
        #endregion
        #region Timers
        if (timer < glideJumpCooldown) timer += Time.deltaTime;
        #endregion
        if (rb.velocityY < -glideFallSpeed) rb.velocityY = -glideFallSpeed;
    }
    public override void FixedMoveRight()
    {
        base.FixedMoveRight();
        rb.velocityX += glideMoveSpeed * Time.deltaTime;
        if (rb.velocityX > maxGlideSpeed) rb.velocityX = maxGlideSpeed;
    }
    public override void FixedMoveLeft()
    {
        base.FixedMoveLeft();
        rb.velocityX -= glideMoveSpeed * Time.deltaTime;
        if (rb.velocityX < -maxGlideSpeed) rb.velocityX = -maxGlideSpeed;
    }
    public override void Jump()
    {
        base.Jump();
        if (jumpCounter < glideJumpCount && timer >= glideJumpCooldown)
        {
            timer = 0.0f;
            jumpCounter++;
            rb.velocityY += glideJumpPower;
        }
    }
    public override void OnExit()
    {
        base.OnExit();
        controller.antiAirMove = true;
    }
    public void UnGlide()
    {
        controller.Switch(basicMoveset);
    }
}
