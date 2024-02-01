using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(NewCutsceneManager))]
public class SpecialFunction : MonoBehaviour
{
    NewCutsceneManager manager;
    const float endStageTime = 1.0f;
    private void Start()
    {
        manager = GetComponent<NewCutsceneManager>();
    }
    public void Function(SpecialFunctionElements element)
    {
        if(element.function == SpecialFunctions.EndStage)
        {
            StartCoroutine(StageEnd());
        }
        else if (element.function == SpecialFunctions.ColliderOff)
        {
            GameManager.Instance.M_PlayerMovements.collider.enabled = false;
            manager.CutsceneProgress();
        }
        else if (element.function == SpecialFunctions.ColliderOn)
        {
            GameManager.Instance.M_PlayerMovements.collider.enabled = true;
            manager.CutsceneProgress();
        }
        else if (element.function == SpecialFunctions.RigidbodyOff)
        {
            GameManager.Instance.M_PlayerMovements.rb.isKinematic = true;
            manager.CutsceneProgress();
        }
        else if (element.function == SpecialFunctions.RigidbodyOn)
        {
            GameManager.Instance.M_PlayerMovements.rb.isKinematic = false;
            manager.CutsceneProgress();
        }
    }
    IEnumerator StageEnd()
    {
        GameManager.Instance.music.FadeOut();
        yield return new WaitForSeconds(endStageTime);
        GameManager.Instance.EndStage();
    }
}
[System.Serializable]
public struct SpecialFunctionElements
{
    public SpecialFunctions function;
}
[System.Serializable] public enum SpecialFunctions
{
    EndStage,
    ColliderOff,
    ColliderOn,
    RigidbodyOff,
    RigidbodyOn
}