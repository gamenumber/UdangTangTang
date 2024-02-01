using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class MovementController : MonoBehaviour
{
    public Movement current;
    [Header("Button Controlls")][SerializeField] HoldButton Left;
    [SerializeField] HoldButton Right;
    [Header("Statistics")] public bool canMove;
    public bool grounded = true;
    public bool canDisableJump = true;
    public bool antiAirMove = false;
    public bool pressingRight, pressingLeft;
    [Header("Common Stats")] public float riseGravity;
    public float fallGravity;
    [Header("Essential Components")] public Animator anim;
    public SpriteRenderer renderer;
    public BoxCollider2D collider;
    public Rigidbody2D rb;
    [Header("Map LayerMask")] public LayerMask whatToScan;
    private void Update()
    {
        if (canMove)
        {
            if (!antiAirMove)
            {
                if (Input.GetKey(KeyCode.D) || Right.buttonPressed)
                {
                    pressingRight = true;
                    current.MoveRight();
                }
                else pressingRight = false;
                if ((Input.GetKey(KeyCode.A) || Left.buttonPressed) && !pressingRight)
                {
                    pressingLeft = true;
                    current.MoveLeft();
                }
                else pressingLeft = false;
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                current.Jump();
            }
        }
        else
        {
            pressingLeft = false;
            pressingRight = false;
        }
    }
    private void FixedUpdate()
    {
        if (canMove && !antiAirMove)
        {
            if (Input.GetKey(KeyCode.D) || Right.buttonPressed)
            {
                pressingRight = true;
                current.FixedMoveRight();
            }
            else pressingRight = false;
            if ((Input.GetKey(KeyCode.A) || Left.buttonPressed) && !pressingRight)
            {
                pressingLeft = true;
                current.FixedMoveLeft();
            }
            else pressingLeft = false;
        }
        else
        {
            pressingLeft = false;
            pressingRight = false;
        }
    }
    public void Jump()
    {
        if(canMove) current.Jump();
    }
    public void Switch(Movement move)
    {
        current.OnExit();
        current.enabled = false;
        current = move;
        current.enabled = true;
    }
    public void Switch(Movement move, Animator changeAnim, SpriteRenderer changeRenderer)
    {
        current.OnExit();
        current.enabled = false;
        current = move;
        changeAnim.SetBool("Grounded", grounded);
        changeAnim.SetBool("Moving", pressingRight||pressingLeft);
        anim = changeAnim;
        renderer = changeRenderer;
        current.enabled = true;
    }
}
[System.Serializable] public enum moveType
{
    basic
}
