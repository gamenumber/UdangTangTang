using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(NewCutsceneManager))]
public class Delay : MonoBehaviour
{
    NewCutsceneManager manager;
    private void Start()
    {
        manager = GetComponent<NewCutsceneManager>();
    }
    public void DelayTime(DelayElements element)
    {
        StartCoroutine(RunDelay(element));
    }
    IEnumerator RunDelay(DelayElements element)
    {
        yield return new WaitForSeconds(element.delayTime);
        Debug.Log("Worksajewjfwjqn");
        manager.CutsceneProgress();
    }
}
[System.Serializable] public struct DelayElements
{
    public float delayTime;
}
