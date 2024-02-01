using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : MonoBehaviour
{
    [SerializeField] protected float moveSpeed, jumpPower;
    [SerializeField] protected Animator bodyAnimator;
    [SerializeField] protected SpriteRenderer bodyRenderer;
    protected Movement defaultMove;
    public virtual void OnChange()
    {
        GameManager.Instance.M_PlayerMovements.collider.enabled = false;
        GameManager.Instance.M_PlayerMovements.collider.enabled = true;
        GameManager.Instance.M_PlayerMovements.Switch(defaultMove, bodyAnimator, bodyRenderer);
        defaultMove.moveSpeed = moveSpeed; defaultMove.jumpPower = jumpPower;
    }
    public virtual void OnInteract()
    {

    }
    public virtual void OnExit()
    {

    }
    public void CanDisableJump()
    {
        GameManager.Instance.M_PlayerMovements.canDisableJump = true;
    }
}
