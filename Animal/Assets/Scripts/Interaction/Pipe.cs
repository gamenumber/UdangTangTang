using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Pipe : Interaction
{
    Slime slime;
    [SerializeField] Transform exitPipe;
    [SerializeField] Direction enterTo, exitTo;
    [SerializeField] float enterDelay, exitDelay;
    [SerializeField] CameraElements camera;
    [SerializeField] bool endCutscene;
    [SerializeField] int cutsceneIndex;
    Vector2 enterOffset = new Vector2(1.35f, 0.0f), exitOffset = new Vector2(1.35f, 0.0f);
    Vector2 enterOffsetUp = new Vector2(0.0f, -3.0f), exitOffsetUp = new Vector2(0.0f, 2.0f);
    Vector2 enterOffsetDown = new Vector2(0.0f, 2.0f), exitOffsetDown = new Vector2(0.0f, -3.0f);
    Vector2 enterPos, exitPos;
    bool cutscenePlayed = false;
    public override void Start()
    {
        base.Start();
        requiredType = AnimalType.slime;
        remove = true;
        requireType = true;
        slime = (Slime)GameManager.Instance.animals[0];
        if (enterTo == Direction.Right)
        {
            enterPos = new Vector2(transform.position.x + -enterOffset.x, transform.position.y + enterOffset.y);
        }
        else if (enterTo == Direction.Left)
        {
            enterPos = new Vector2(transform.position.x + enterOffset.x, transform.position.y + enterOffset.y);
        }
        else if(enterTo == Direction.Up)
        {
            enterPos = new Vector2(transform.position.x + enterOffsetUp.x, transform.position.y + enterOffsetUp.y);
        }
        else if(enterTo == Direction.Down)
        {
            enterPos = new Vector2(transform.position.x + enterOffsetDown.x, transform.position.y + enterOffsetDown.y);
        }
        if(exitTo == Direction.Right)
        {
            exitPos = new Vector2(exitPipe.transform.position.x + exitOffset.x, exitPipe.transform.position.y + exitOffset.y);
        }
        else if (exitTo == Direction.Left)
        {
            exitPos = new Vector2(exitPipe.transform.position.x + -exitOffset.x, exitPipe.transform.position.y + exitOffset.y);
        }
        else if (exitTo == Direction.Up)
        {
            exitPos = new Vector2(exitPipe.transform.position.x + exitOffsetUp.x, exitPipe.transform.position.y + exitOffsetUp.y);
        }
        else if (exitTo == Direction.Down)
        {
            exitPos = new Vector2(exitPipe.transform.position.x + exitOffsetDown.x, exitPipe.transform.position.y + exitOffsetDown.y);
        }
    }
    public override void Interact()
    {
        if (endCutscene && !cutscenePlayed)
        {
            slime.PipeEnter(enterPos, exitPos, enterTo, exitTo, enterDelay, exitDelay, camera, cutsceneIndex);
            cutscenePlayed = true;
        }
        else
        {
            slime.PipeEnter(enterPos, exitPos, enterTo, exitTo, enterDelay, exitDelay, camera);
        }
    }
}