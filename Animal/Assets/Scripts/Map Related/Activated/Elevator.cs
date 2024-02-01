using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : Activated
{
    public Transform moveObject;
    public float moveSpeed, maxHeight, waitTime;
    bool activated = false, down = false;
    IEnumerator currentElevation;
    public override void OnActivation()
    {
        base.OnActivation();
        if (!activated)
        {
            activated = true;
            currentElevation = Elevating();
            StartCoroutine(currentElevation);
        }
        else
        {
            activated = false;
            StopCoroutine(currentElevation);
            currentElevation = null;
        }
    }
    IEnumerator Elevating()
    {
        while (true)
        {
            if (!down)
            {
                Vector3 value = Vector3.up * moveSpeed  * Time.deltaTime;
                moveObject.localPosition += value;
                if(moveObject.localPosition.y >= maxHeight)
                {
                    moveObject.localPosition = new Vector3(0.0f, maxHeight, 0.0f);
                    down = true;
                    yield return new WaitForSeconds(waitTime);
                }
            }
            else
            {
                Vector3 value = Vector3.down * moveSpeed * Time.deltaTime;
                moveObject.localPosition += value;
                if (moveObject.localPosition.y <= 0.0f)
                {
                    moveObject.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
                    down = false;
                    yield return new WaitForSeconds(waitTime);
                }
            }
            yield return null;
        }
    }
}
