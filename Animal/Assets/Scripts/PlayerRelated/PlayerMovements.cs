#pragma warning disable CS0108
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovements : MonoBehaviour
{
    [SerializeField] Animator animatingBody;
    public Animator setAnimator
    {
        get
        {
            return animatingBody;
        }
        set
        {
            if (animatingBody==null)
            {
                Debug.Log("rea");
                animatingBody = value;
            }
            else
            {
                value.SetBool("Grounded", animatingBody.GetBool("Grounded"));
                value.SetBool("Moving", animatingBody.GetBool("Moving"));
                animatingBody = value;
            }
        }
    }
    public SpriteRenderer renderingBody;
    public bool grounded = true, 
        canMove = true, jump = true, 
        doubleJump = false, dash = false, 
        glide = false;
    public float jumpPower, moveSpeed, 
        doubleJumpPower, dashPower,
        glideJumpPower, glideSpeed, glideSpeedLimit;
    public float gravity = 1.5f, fallGravity = 3.0f, 
        doubleJumpCooldown = 0.2f, dashCooldown = 0.2f,
        glideJumpCooldown = 1.0f;
    [SerializeField] LayerMask whatToScan;
    public BoxCollider2D collider;
    public Rigidbody2D rb;
    [SerializeField] float sideScanDist = 0.2f;
    [Header("Button Controlls")] public HoldButton Left;
    public HoldButton Right;
    [Space(5)] bool movingLeft = false, movingRight = false, jumpHolding = false;
    bool doubleJumped = false, dashed = false, dashScanned = false;
    public bool midairAntiMove = false;
    float DJcounter = 0.0f, Dcounter = 0.0f, GJcounter = 0.0f;
    public bool canDisableJump = true;
    public int maxAirGlideJump;
    int airJumpCounter = 0;

    #region BaseValues
    public float baseGravity = 1.5f, baseFallGravity = 3.0f;
    #endregion
    private void Awake()
    {
        collider = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        grounded = GroundCheck();
        bool finalGrounded = grounded;
        if (grounded)
        {
            doubleJumped = false;
            if (dashed)
            {
                dashed = false;
                dashScanned = false;
                rb.velocity = new Vector2(0.0f, 0.0f);
            }
            if (!canDisableJump) finalGrounded = false;
            if (midairAntiMove)
            {
                rb.velocityX = 0.0f;
                midairAntiMove = false;
            }
            airJumpCounter = 0;
        }
        else if (dashed && !dashScanned)
        {
            RaycastHit2D hit = Physics2D.BoxCast(collider.bounds.center, collider.bounds.size, 0.0f, Vector2.right, 0.1f, whatToScan);
            if(hit.collider != null)
            {
                rb.velocity = new Vector2(0.0f, rb.velocity.y);
                dashScanned = true;
            }
            RaycastHit2D hit2 = Physics2D.BoxCast(collider.bounds.center, collider.bounds.size, 0.0f, Vector2.left, 0.1f, whatToScan);
            if (hit2.collider != null)
            {
                rb.velocity = new Vector2(0.0f, rb.velocity.y);
                dashScanned = true;
            }
        }
        if (rb.isKinematic) finalGrounded = true;
        animatingBody.SetBool("Grounded", finalGrounded);
        if (glide && rb.velocity.y < -fallGravity) rb.velocity = new Vector2(rb.velocity.x, -fallGravity);
        animatingBody.SetBool("Moving", movingLeft||movingRight);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
        if (doubleJump && DJcounter < doubleJumpCooldown) DJcounter += Time.deltaTime;
        if (dash && Dcounter < dashCooldown) Dcounter += Time.deltaTime;
        if (glide && GJcounter < glideJumpCooldown) GJcounter += Time.deltaTime;
        if (rb.velocity.y >= 0)
        {
            rb.gravityScale = gravity;
        }
        else rb.gravityScale = fallGravity;
        if (glide)
        {
            if (rb.velocity.x >= 0)
            {
                renderingBody.flipX = false;
            }
            else renderingBody.flipX = true;
        }
        
    }
    private void FixedUpdate()
    {
        movingLeft = Left.buttonPressed; movingRight = Right.buttonPressed;
        if ((Input.GetKey(KeyCode.D) || Right.buttonPressed) && !midairAntiMove && canMove)
        {
            if (!glide)
            {
                RaycastHit2D hit = Physics2D.BoxCast(collider.bounds.center, collider.bounds.size, 0.0f, Vector2.right, 0.1f, whatToScan);
                if (hit.collider == null) transform.position += Vector3.right * Time.deltaTime * moveSpeed;
                renderingBody.flipX = false;
                movingRight = true;
            }
            else
            {
                Debug.Log("EEE");
                rb.velocityX = Mathf.Min(rb.velocityX + glideSpeed, glideSpeedLimit);
            }
        }
        else movingRight = false;
        if ((Input.GetKey(KeyCode.A) || Left.buttonPressed) && !midairAntiMove && canMove && !movingRight)
        {
            if (!glide)
            {
                RaycastHit2D hit = Physics2D.BoxCast(collider.bounds.center, collider.bounds.size, 0.0f, Vector2.left, 0.1f, whatToScan);
                if (hit.collider == null) transform.position += Vector3.left * Time.deltaTime * moveSpeed;
                renderingBody.flipX = true;
                movingLeft = true;
            }
            else
            {
                rb.velocityX = Mathf.Max(rb.velocityX - glideSpeed, -glideSpeedLimit);
            }
        }
        else movingLeft = false;
    }
    private bool GroundCheck()
    {
        Vector3 offSet = new Vector3(0.0f, -0.01f, 0.0f);
        RaycastHit2D hit = Physics2D.BoxCast(collider.bounds.center + offSet, collider.bounds.size, 0.0f, Vector2.down, 0.02f, whatToScan);
        if (hit.collider != null) return true;
        else return false;
    }
    public void FixPositionX(float x)
    {
        rb.velocity = new Vector2(0.0f, rb.velocity.y);
        transform.position = new Vector2(x, transform.position.y);
    }
    public void FixPositionY(float y)
    {
        rb.velocity = new Vector2(rb.velocity.x, 0.0f);
        transform.position = new Vector2(transform.position.x, y);
    }
    public void Jump()
    {
        if (canMove)
        {
            if (!glide)
            {
                if (grounded)
                {
                    grounded = false;
                    canDisableJump = false;
                    animatingBody.Play("jump");
                    animatingBody.SetBool("Grounded", false);
                    rb.velocity = new Vector2(rb.velocity.x, jumpPower);
                    if(doubleJump) DJcounter = 0.0f;
                }
                else if (doubleJump && !doubleJumped && DJcounter >= doubleJumpCooldown)
                {
                    animatingBody.Play("doubleJump");
                    rb.velocity = new Vector2(rb.velocity.x, doubleJumpPower);
                    doubleJumped = true;
                    if(dash) Dcounter = 0.0f;
                }
                else if (dash && !dashed && doubleJumped && Dcounter >= dashCooldown)
                {
                    animatingBody.Play("dash");
                    if (renderingBody.flipX == true)
                    {
                        rb.velocity = new Vector2(-dashPower, 0.0f);
                    }
                    else
                    {
                        rb.velocity = new Vector2(dashPower, 0.0f);
                    }
                    dashed = true;
                    midairAntiMove = true;
                }
            }
            else if (glide)
            {
                if(GJcounter >= glideJumpCooldown && airJumpCounter < maxAirGlideJump)
                {
                    rb.velocity = new Vector2(rb.velocity.x, glideJumpPower);
                    GJcounter = 0.0f;
                    airJumpCounter++;
                }
            }
        }
    }
    public void SpecialsReset()
    {
        jump = true;
        dash = false;
        doubleJump = false;
        glide = false;
        gravity = baseGravity;
        fallGravity = baseFallGravity;
    }
}
