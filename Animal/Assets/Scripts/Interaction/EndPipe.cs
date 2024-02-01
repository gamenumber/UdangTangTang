using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPipe : Interaction
{
    Slime slime;
    [SerializeField] Direction enterTo;
    [SerializeField] bool endCutscene;
    [SerializeField] int cutsceneIndex;
    float enterDelay = 1.0f;
    Vector2 enterOffset = new Vector2(1.35f, 0.0f), exitOffset = new Vector2(1.35f, 0.0f);
    Vector2 enterOffsetUp = new Vector2(0.0f, -3.0f), exitOffsetUp = new Vector2(0.0f, 2.0f);
    Vector2 enterOffsetDown = new Vector2(0.0f, 2.0f), exitOffsetDown = new Vector2(0.0f, -3.0f);
    Vector2 enterPos;
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
        else if (enterTo == Direction.Up)
        {
            enterPos = new Vector2(transform.position.x + enterOffsetUp.x, transform.position.y + enterOffsetUp.y);
        }
        else if (enterTo == Direction.Down)
        {
            enterPos = new Vector2(transform.position.x + enterOffsetDown.x, transform.position.y + enterOffsetDown.y);
        }
    }
    public override void Interact()
    {
        slime.EndPipeEnter(enterPos, enterTo, enterDelay, endCutscene, cutsceneIndex);
    }
}
