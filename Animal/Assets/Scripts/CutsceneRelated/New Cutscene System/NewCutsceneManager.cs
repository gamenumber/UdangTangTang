using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NewCutsceneManager : MonoBehaviour
{
    public Cutscene[] cutscene;
    public Element current { get { return cutscene[cutsceneIndex].element[elementIndex]; } }
    [SerializeField] Tutorial tutorialScript;
    [SerializeField] Talk talkScript;
    [SerializeField] AnimationTrigger animationTriggerScript;
    [SerializeField] CameraMovement cameraMovementScript;
    [SerializeField] Delay delayScript;
    [SerializeField] SpecialFunction specialFunctionScript;
    [SerializeField] GameObject[] CutsceneHide;
    int cutsceneIndex, elementIndex = 0;
    [HideInInspector] public UnityEvent atCutsceneStart, atCutsceneEnd;
    public void CutsceneProgress()
    {
        elementIndex++;
        if(elementIndex == cutscene[cutsceneIndex].element.Length)
        {
            Debug.Log("Works");
            GameManager.Instance.M_PlayerMovements.canMove = true;
            GameManager.Instance.immortal = false;
            GameManager.Instance.freeCam = false;
            GameManager.Instance.timerStopped = false;
            GameManager.Instance.anim.SetBool("Cutscene", false);
            for (int i = 0; i < CutsceneHide.Length; i++)
            {
                CutsceneHide[i].SetActive(true);
            }
            atCutsceneEnd.Invoke();
            return;
        }
        if (current.elementType == ElementType.Tutorial)
        {
            tutorialScript.StartTutorial(current.tutorial);
        }
        if(current.elementType == ElementType.Talk)
        {
            talkScript.StartTalk(current.talk);
        }
        if(current.elementType == ElementType.AnimationTrigger)
        {
            animationTriggerScript.Trigger(current.animationTrigger);
        }
        if(current.elementType == ElementType.CameraMovement)
        {
            cutscene[cutsceneIndex].element[elementIndex].cameraMovement.progressCutscene = true;
            cameraMovementScript.MoveCamera(current.cameraMovement);
        }
        if(current.elementType == ElementType.Delay)
        {
            delayScript.DelayTime(current.delay);
        }
        if (current.elementType == ElementType.SpecialFunction)
        {
            specialFunctionScript.Function(current.specialFunction);
        }
    }
    public void StartCutscene(int num)
    {
        atCutsceneStart.Invoke();
        GameManager.Instance.timerStopped = true;
        GameManager.Instance.M_PlayerMovements.canMove = false;
        GameManager.Instance.immortal = true;
        GameManager.Instance.M_PlayerMovements.rb.velocity = new Vector2(0.0f, GameManager.Instance.M_PlayerMovements.rb.velocity.y);
        GameManager.Instance.freeCam = true;
        GameManager.Instance.anim.SetBool("Cutscene", true);
        for(int i = 0; i < CutsceneHide.Length; i++)
        {
            CutsceneHide[i].SetActive(false);
        }
        cutsceneIndex = num;
        elementIndex = -1;
        CutsceneProgress();
    }
}
[System.Serializable] 
public struct Cutscene
{
    public Element[] element;
}
[System.Serializable] 
public struct Element
{
    [Tooltip("이 컷씬 요소가 어떤 종류인지 결정")][Space(10)] public ElementType elementType;
    public TutorialElements[] tutorial;
    public TalkElements[] talk;
    public AnimationElements[] animationTrigger;
    public CameraElements cameraMovement;
    public DelayElements delay;
    public SpecialFunctionElements specialFunction;
}
public enum ElementType
{
    Tutorial,
    Talk,
    AnimationTrigger,
    CameraMovement,
    Delay,
    SpecialFunction
}
