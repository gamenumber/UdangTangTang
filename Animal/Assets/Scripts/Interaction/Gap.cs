using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gap : Interaction
{
    Slime slime;
    [SerializeField] Vector2 enterOffset = new Vector2(1.0f, -0.75f), endOffset = new Vector2(1.0f, -0.75f);
    [SerializeField] float moveSpeed;
    [SerializeField] HorizontalDir enterTo, exitTo;
    [SerializeField] Transform[] movePoints;
    public override void Start()
    {
        base.Start();
        requiredType = AnimalType.slime;
        requireType = true;
        slime = (Slime)GameManager.Instance.animals[0];
        if(enterTo == HorizontalDir.Right)
        {
            enterOffset = new Vector2(-enterOffset.x, enterOffset.y);
        }
        if (exitTo == HorizontalDir.Left)
        {
            endOffset = new Vector2(-endOffset.x, endOffset.y);
        }
    }
    public override void Interact()
    {
        slime.GapEnter((Vector2)movePoints[0].position+enterOffset, (Vector2)movePoints[movePoints.Length-1].position+endOffset, enterTo, exitTo, movePoints, moveSpeed);

    }
}
