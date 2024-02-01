using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(NewCutsceneManager))] public class AnimationTrigger : MonoBehaviour
{
    NewCutsceneManager manager;
    private void Start()
    {
        manager = GetComponent<NewCutsceneManager>();
    }
    public void Trigger(AnimationElements[] elements)
    {
        for(int i = 0; i < elements.Length; i++)
        {
            StartCoroutine(RunAnimationTrigger(elements[i]));
        }
    }
    public void SingleTrigger(AnimationElements element)
    {
        StartCoroutine(RunAnimationTrigger(element));
    }
    IEnumerator RunAnimationTrigger(AnimationElements elements)
    {
        yield return new WaitForSeconds(elements.delay);
        if(elements.triggerType == TriggerType.Bool)
        {
            elements.anim.SetBool(elements.triggerName, elements.content>=1);
        }
        else if(elements.triggerType == TriggerType.Trigger)
        {
            elements.anim.SetTrigger(elements.triggerName);
        }
        else if (elements.triggerType == TriggerType.Integer)
        {
            elements.anim.SetInteger(elements.triggerName, elements.content);
        }
        if(elements.instant)
        {
            Debug.Log("qejkfjqewnkkhfjwqekhfje");
            manager.CutsceneProgress();
        }
    }
}
[System.Serializable] public struct AnimationElements
{
    [Tooltip("변수의 변동을 줄 에니메이터")] public Animator anim;
    [Space(3)][Tooltip("변동을 줄 변수의 타입")] public TriggerType triggerType;
    [Tooltip("변동을 줄 변수의 이름")] public string triggerName;
    [Tooltip("변동 내용. bool일 경우 1이상 아니면 0, integer일 경우 숫자, trigger일 경우 참조하지 않음.")] public int content;
    [Space(3)][Tooltip("발동시킨 후 변수의 변동이 일어나기까지 딜레이 시간")] public float delay;
    [Space(3)][Tooltip("변동 후 바로 다음 요소로 넘어갈지 결정. false라면 클립에서 CutsceneCommand로 CutsceneProgress를 끝에 실행시켜야함.")] public bool instant;
}
[System.Serializable] public enum TriggerType
{
    Bool,
    Trigger,
    Integer
}
