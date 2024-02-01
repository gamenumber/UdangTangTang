using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(NewCutsceneManager), typeof(AnimationTrigger))] public class Talk : MonoBehaviour
{
    [Tooltip("대화 내용이 뜨는 속도")][SerializeField] float talkDelay;
    [Tooltip("대화 내용에 |가 들어있을 경우 기다리는 시간")][SerializeField] float delayTime;
    [Tooltip("대화가 끝나고 다음으로 넘길 수 있게 될 때까지 기다리는 시간")][SerializeField] float endTime;
    [SerializeField] AnimationTrigger animationTrigger;
    [SerializeField] CameraMovement cameraMovement;
    [SerializeField] Text talkerText, talkText;
    [SerializeField] GameObject talkEndText;
    [SerializeField] Image talkerImage;
    bool talkEnded = false;
    Animator anim { get { return GameManager.Instance.anim; } }
    NewCutsceneManager manager;
    bool talkStarted = false, animTalkProgress = false;
    private void Start()
    {
        manager = GetComponent<NewCutsceneManager>();
    }
    public void StartTalk(TalkElements[] elements)
    {
        anim.SetBool("Talk", true);
        StartCoroutine(RunTalk(elements));
    }
    IEnumerator RunTalk(TalkElements[] elements)
    {
        while (true)
        {
            if (talkStarted)
            {
                talkStarted = false;
                break;
            }
            yield return null;
        }
        talkText.gameObject.SetActive(true);
        talkerText.gameObject.SetActive(true);
        for(int i = 0; i < elements.Length; i++)
        {
            talkEnded = false;
            talkText.text = "";
            talkerText.text = elements[i].talker;
            if (elements[i].useImage)
            {
                talkerImage.sprite = elements[i].talkerImage;
                anim.SetBool("Talker", true);
            }
            else
            {
                anim.SetBool("Talker", false);
            }
            IEnumerator a = Talking(elements[i]);
            StartCoroutine(a);
            while (true)
            {
                if (!elements[i].animEnd)
                {
                    if (Input.GetMouseButtonDown(0) && elements[i].canSkip && !elements[i].useFunctions)
                    {
                        StopCoroutine(a);
                        talkText.text = "";
                        for (int k = 0; k < elements[i].talk.Length; k++)
                        {
                            if (elements[i].talk[k] != '|') talkText.text += elements[i].talk[k];
                        }
                        break;
                    }
                    else if (talkEnded)
                    {
                        break;
                    }
                }
                else{
                    if (animTalkProgress)
                    {
                        animTalkProgress = false;
                        break;
                    }
                }
                yield return null;
            }
            yield return new WaitForSeconds(endTime);
            talkEndText.SetActive(true);
            while (true)
            {
                if (Input.GetMouseButtonDown(0) || elements[i].autoSkip)
                {
                    break;
                }
                yield return null;
            }
            talkEndText.SetActive(false);
            yield return null;
        }
        talkText.gameObject.SetActive(false);
        talkerText.gameObject.SetActive(false);
        anim.SetBool("Talk", false); anim.SetBool("Talker", false);
        manager.CutsceneProgress();
    }
    IEnumerator Talking(TalkElements element)
    {
        for (int k = 0; k < element.talk.Length; k++)
        {
            if (element.talk[k] == '|') yield return new WaitForSeconds(delayTime);
            else if (k+2 < element.talk.Length && element.talk[k] == '{' && element.talk[k+2] == '}')
            {
                animationTrigger.SingleTrigger(element.animation[(int)element.talk[k + 1]-48]);
                k += 2;
            }
            else if(k + 2 < element.talk.Length && element.talk[k] == '[' && element.talk[k + 2] == ']')
            {
                element.camera[(int)element.talk[k + 1] - 48].progressCutscene = false;
                cameraMovement.MoveCamera(element.camera[(int)element.talk[k + 1] - 48]);
                k += 2;
            }
            else
            {
                talkText.text += element.talk[k];
                yield return new WaitForSeconds(talkDelay);
            }
        }
        talkEnded = true;
        yield break;
    }
    public void TalkStart()
    {
        talkStarted = true;
    }
    public void TalkProgress()
    {
        animTalkProgress = true;
    }
}
[System.Serializable] public struct TalkElements
{
    [Tooltip("대화창에서 대화하는 인물 이름")] public string talker;
    [Tooltip("대화 내용")][TextArea] public string talk;
    [Space(3)][Tooltip("대화창에서 이미지를 표시할지 결정. false일 경우 이미지는 뜨지 않음.")] public bool useImage;
    [Tooltip("useImage가 true일 경우 사용할 이미지.")] public Sprite talkerImage;
    [Space(5)][Tooltip("대화문 뜨는 도중 클릭으로 스킵 가능 여부. useFunctions가 켜져 있으면 자동으로 꺼짐.")] public bool canSkip;
    [Tooltip("마지막에 한번 클릭 안해도 자동으로 넘어가는지 여부")] public bool autoSkip;
    [Space(5)][Tooltip("대화문에 특수기호를 넣어서 발동시키는 것들을 사용할지 여부.\n{0}: 0번 애니메이션 실행\n[0]: 0번 카메라 이동 실행")] public bool useFunctions;
    [Tooltip("에니메이션이 끝나야 대화 진행을 가능하게 할지 결정. true라면 에니메이션이 마지막에 TalkProgress를 실행시켜야 함.")] public bool animEnd;
    [Tooltip("useFunction이 true면 사용되는 에니메이션 요소 설정")] public AnimationElements[] animation;
    [Tooltip("useFunction이 true면 사용되는 카메라 이동 요소 설정")] public CameraElements[] camera;
}
