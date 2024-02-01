using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPipe : MonoBehaviour
{
    [SerializeField] Slime slime;
    [SerializeField] Direction exitTo;
    [SerializeField] bool cutsceneStart = true;
    Vector2 exitPos;
    Vector2 exitOffset = new Vector2(1.35f, 0.0f), exitOffsetUp = new Vector2(0.0f, 2.0f), exitOffsetDown = new Vector2(0.0f, -3.0f);
    public void StartGame()
    {
        slime = (Slime)GameManager.Instance.animals[0];
        if (exitTo == Direction.Right)
        {
            exitPos = new Vector2(transform.position.x + exitOffset.x, transform.position.y + exitOffset.y);
        }
        else if (exitTo == Direction.Left)
        {
            exitPos = new Vector2(transform.position.x + -exitOffset.x, transform.position.y + exitOffset.y);
        }
        else if (exitTo == Direction.Up)
        {
            exitPos = new Vector2(transform.position.x + exitOffsetUp.x, transform.position.y + exitOffsetUp.y);
        }
        else if (exitTo == Direction.Down)
        {
            exitPos = new Vector2(transform.position.x + exitOffsetDown.x, transform.position.y + exitOffsetDown.y);
        }
        slime.PipeStageStart(exitPos, exitTo, cutsceneStart);
    }
}
