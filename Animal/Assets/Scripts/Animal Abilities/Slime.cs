using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Slime : Ability
{
    [SerializeField] float doubleJumpPower, dashPower, termDJ, termD;
    [SerializeField] Agile agileMoveset;
    bool pipeStarted = false, gapStarted = false;
    bool playCutscene = false; int cutsceneIndex = 0;
    
    public override void OnChange()
    {
        defaultMove = agileMoveset;
        base.OnChange();
        agileMoveset.doubleJumpPower = doubleJumpPower;
        agileMoveset.dashPower = dashPower;
        agileMoveset.term1 = termDJ;
        agileMoveset.term2 = termD;
    }
    public void PipeEnter(Vector2 enterPos, Vector2 exitPos, Direction enterTo, Direction exitTo, float enterDelay, float exitDelay, CameraElements camera)
    {
        playCutscene = false;
        StartCoroutine(PipeMoving(enterPos, exitPos, enterTo, exitTo, enterDelay, exitDelay, camera, false));
    }
    public void PipeEnter(Vector2 enterPos, Vector2 exitPos, Direction enterTo, Direction exitTo, float enterDelay, float exitDelay, CameraElements camera, int cutsceneNum)
    {
        playCutscene = true;
        cutsceneIndex = cutsceneNum;
        StartCoroutine(PipeMoving(enterPos, exitPos, enterTo, exitTo, enterDelay, exitDelay, camera, false));
    }
    public void EndPipeEnter(Vector2 enterPos, Direction enterTo, float enterDelay, bool cutscene, int cutsceneNum)
    {
        if (!cutscene) GameManager.Instance.music.FadeOut();
        playCutscene = cutscene;
        cutsceneIndex = cutsceneNum;
        StartCoroutine(PipeMoving(enterPos, Vector2.zero, enterTo, Direction.Right, enterDelay, 0.0f, new CameraElements(), true));
    }
    public void PipeMoveStart()
    {
        pipeStarted = true;
    }
    IEnumerator PipeMoving(Vector2 enterPos, Vector2 exitPos, Direction enterTo, Direction exitTo, float enterDelay, float exitDelay, CameraElements camera, bool end)
    {
        pipeStarted = false;
        camera.movePos = new Vector3(exitPos.x, exitPos.y, -10.0f);
        GameManager.Instance.M_PlayerCharacter.canChange = false;
        GameManager.Instance.M_PlayerInteraction.canInteract = false;
        GameManager.Instance.M_PlayerInteraction.canAddInteract = false;
        GameManager.Instance.M_PlayerMovements.rb.isKinematic = true;
        GameManager.Instance.M_PlayerMovements.rb.velocity = Vector2.zero;
        GameManager.Instance.player.transform.position = enterPos;
        GameManager.Instance.M_PlayerMovements.canMove = false;
        GameManager.Instance.freeCam = true;
        bodyAnimator.Play("idle");
        if (enterTo == Direction.Left)
        {
            bodyRenderer.flipX = true;
            bodyAnimator.Play("pipeEnter");
        }
        else if(enterTo == Direction.Right)
        {
            bodyRenderer.flipX = false;
            bodyAnimator.Play("pipeEnter");
        }
        while (true)
        {
            if (pipeStarted) break;
            yield return null;
        }
        yield return new WaitForSeconds(enterDelay);
        if (end)
        {
            if (playCutscene)
            {
                GameManager.Instance.M_NewCutsceneManager.StartCutscene(cutsceneIndex);
            }
            else
            {
                GameManager.Instance.EndStage();
            }
            yield break;
        }
        GameManager.Instance.M_CameraMovement.MoveCamera(camera);
        yield return new WaitForSeconds(exitDelay);
        if (exitTo == Direction.Left)
        {
            bodyRenderer.flipX = false;
        }
        else
        {
            bodyRenderer.flipX = true;
        }
        GameManager.Instance.player.transform.position = exitPos;
        bodyAnimator.Play("pipeExit");
    }
    public void PipeMoveEnd()
    {
        GameManager.Instance.M_PlayerCharacter.canChange = true;
        GameManager.Instance.M_PlayerInteraction.canAddInteract = true;
        GameManager.Instance.M_PlayerInteraction.canInteract = true;
        GameManager.Instance.M_PlayerMovements.rb.isKinematic = false;
        GameManager.Instance.M_PlayerMovements.canMove = true;
        GameManager.Instance.freeCam = false;
        if (playCutscene)
        {
            GameManager.Instance.M_NewCutsceneManager.StartCutscene(cutsceneIndex);
        }
    }
    public void GapEnter(Vector2 enterPos, Vector2 endPos, HorizontalDir enterTo, HorizontalDir exitTo, Transform[] movePoints, float speed)
    {
        StartCoroutine(GapMove(enterPos, endPos, enterTo, exitTo, movePoints, speed));
    }
    public void GapMoveStart()
    {
        gapStarted = true;
    }
    IEnumerator GapMove(Vector2 enterPos, Vector2 endPos, HorizontalDir enterTo, HorizontalDir exitTo, Transform[] movePoints, float speed)
    {
        gapStarted = false;
        GameManager.Instance.M_PlayerInteraction.canInteract = false;
        GameManager.Instance.M_PlayerInteraction.canAddInteract = false;
        GameManager.Instance.M_PlayerMovements.rb.isKinematic = true;
        GameManager.Instance.M_PlayerMovements.rb.velocity = Vector2.zero;
        GameManager.Instance.M_PlayerMovements.collider.enabled = false;
        GameManager.Instance.M_PlayerMovements.canMove = false;
        GameManager.Instance.player.transform.position = enterPos;
        if(enterTo==HorizontalDir.Left)
        {
            bodyRenderer.flipX = true;
        }
        else
        {
            bodyRenderer.flipX = false;
        }
        bodyAnimator.SetBool("Grounded", true);
        bodyAnimator.SetTrigger("ForcedIdle");
        bodyAnimator.SetTrigger("GapEnter");
        while (true)
        {
            if (gapStarted) break;
            yield return null;
        }
        GameManager.Instance.player.transform.position = movePoints[0].position;
        bodyAnimator.SetTrigger("GapEntered");
        for (int i = 1; i < movePoints.Length; i++)
        {
            while (true)
            {
                if(Vector2.Distance(GameManager.Instance.player.transform.position, movePoints[i].position) <= 0.15f)
                {
                    GameManager.Instance.player.transform.position = movePoints[i].position;
                    break;
                }
                GameManager.Instance.player.transform.position = Vector2.Lerp(GameManager.Instance.player.transform.position, movePoints[i].position, speed * Time.deltaTime);
                yield return null;
            }
        }
        GameManager.Instance.player.transform.position = endPos;
        if (exitTo == HorizontalDir.Left)
        {
            bodyRenderer.flipX = false;
        }
        else
        {
            bodyRenderer.flipX = true;
        }
        bodyAnimator.SetTrigger("GapExit");
    }
    public void GapMoveEnd()
    {
        GameManager.Instance.M_PlayerInteraction.canAddInteract = true;
        GameManager.Instance.M_PlayerInteraction.canInteract = true;
        GameManager.Instance.M_PlayerMovements.canMove = true;
        GameManager.Instance.M_PlayerMovements.collider.enabled = true;
        GameManager.Instance.M_PlayerMovements.rb.isKinematic = false;
    }
    public void PipeStageStart(Vector2 endPos, Direction exitTo, bool cutsceneStart)
    {
        playCutscene = cutsceneStart;
        cutsceneIndex = 0;
        GameManager.Instance.M_PlayerInteraction.canInteract = false;
        GameManager.Instance.M_PlayerInteraction.canAddInteract = false;
        GameManager.Instance.M_PlayerMovements.rb.isKinematic = true;
        GameManager.Instance.M_PlayerMovements.canMove = false;
        GameManager.Instance.player.transform.position = endPos;
        if (exitTo == Direction.Left)
        {
            bodyRenderer.flipX = false;
        }
        else
        {
            bodyRenderer.flipX = true;
        }
        bodyAnimator.Play("pipeExit");
    }
}
