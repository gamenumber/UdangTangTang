using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ArmadilloMode))]public class Armadillo : Ability
{
    [SerializeField] GameObject timingGUI;
    [SerializeField] float spinMoveSpeed;
    bool inArea = false, clicked = false, succeed = false;
    bool spinning = false;
    ArmadilloBreaker currentBreaking;
    ArmadilloMode mode;
    public override void OnChange()
    {
        base.OnChange();
        mode = GetComponent<ArmadilloMode>();
        GameManager.Instance.M_PlayerInteraction.Add(mode);
        bodyAnimator.SetBool("Spinning", false);
        spinning = false;
    }
    public void SpinToggle()
    {
        
    }
    public void Area(bool inOut)
    {
        inArea = inOut;
    }
    public void BreakTime()
    {
        if (succeed)
        {
            currentBreaking.Broken();
        }
    }
    public void StartBreak(HorizontalDir dir, Vector2 startPos, ArmadilloBreaker broken)
    {
        StartCoroutine(RunBreak(dir, startPos, broken));
    }
    public void Finish()
    {
        bodyAnimator.Play("idle");
        GameManager.Instance.M_PlayerInteraction.canInteract = true;
        GameManager.Instance.M_PlayerInteraction.canAddInteract = true;
        GameManager.Instance.M_PlayerMovements.canMove = true;
    }
    IEnumerator RunBreak(HorizontalDir dir, Vector2 startPos, ArmadilloBreaker broken)
    {
        currentBreaking = broken;
        GameManager.Instance.M_PlayerInteraction.canInteract = false;
        GameManager.Instance.M_PlayerInteraction.canAddInteract = false;
        GameManager.Instance.M_PlayerMovements.canMove = false;
        GameManager.Instance.player.transform.position = startPos;
        if (dir == HorizontalDir.Left)
        {
            bodyRenderer.flipX = true;
        }
        else bodyRenderer.flipX = false;
        clicked = false;
        inArea = false;
        timingGUI.SetActive(true);
        bodyAnimator.Play("spin");
        GameManager.Instance.anim.SetBool("ArmadilloTiming", true);
        while (true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                succeed = inArea;
                timingGUI.SetActive(false);
                bodyAnimator.Play("breakJump");
                GameManager.Instance.anim.SetBool("ArmadilloTiming", false);
                break;
            }
            yield return null;
        }
    }
}
