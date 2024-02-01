using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CutsceneManager : MonoBehaviour
{
    public Animator anim, playerAnim;
    public Text talkerText, talkText;
    public GameObject progressText, buttons;
    public GameObject[] cutsceneDisables;
    public float talkSpeed, talkWaitSpeed;
    public bool cutscene = false, talking = false, talkEnded = false, animating = false, allowProgress = false, cameraMoving = false, talkLeftOpen = false;
    public Text tutorialTitle, tutorialContent;
    public GameObject tutorialProgressText;
    public Image tutorialImage;
    public float tutorialProgressCooldown = 0.5f;
    public bool tutorial = false, allowTutorialProgress = false;
    public int currentTutorialPage = 0;
    public Image talkerImage;
    GameObject player { get { return GameManager.Instance.player; } }
    int currentCutscene = 0, currentContent = 0, currentTalk = 0;
    float tutorialProgressCD = 0.0f;
    bool talker = false; Sprite currentTalker = null;
    IEnumerator talk = null;
    [SerializeField] public List<Cutscene> contents;
    CutsceneElement current { get { return contents[currentCutscene].element[currentContent]; } }
    private void Start()
    {
        CutsceneStart(0);
    }
    private void Update()
    {
        if (cutscene)
        {
            if (talking && Input.GetMouseButtonDown(0))
            {
                if (talkEnded)
                {
                    progressText.SetActive(false);
                    talkEnded = false;
                    currentTalk++;
                    if(currentTalk == current.talker.Length - 1 && current.autoSkip)
                    {
                        talking = false;
                        Debug.Log("EEEEE");
                        talk = Talk(current.talker[currentTalk], current.talkContent[currentTalk], true);
                        StartCoroutine(talk);
                        talkLeftOpen = true;
                        CutsceneProgress();
                    }
                    else
                    {
                        if (currentTalk == current.talker.Length)
                        {
                            if (current.keepTalkBox)
                            {
                                CutsceneProgress();
                            }
                            else
                            {
                                talkerText.gameObject.SetActive(false);
                                talkText.gameObject.SetActive(false);
                                anim.SetBool("Talk", false);
                                talking = false;
                                CutsceneProgress();
                            }
                        }
                        else
                        {
                            talk = Talk(current.talker[currentTalk], current.talkContent[currentTalk], false);
                            StartCoroutine(talk);
                        }
                    }
                }
                else
                {
                    StopCoroutine(talk);
                    talkText.text = null;
                    for(int i = 0; i < current.talkContent[currentTalk].Length; i++)
                    {
                        if (current.talkContent[currentTalk][i] != '|')
                        {
                            talkText.text += current.talkContent[currentTalk][i];
                        }
                    }                 
                    talkEnded = true;
                    progressText.SetActive(true);
                }
            }
            if (animating && allowProgress && Input.GetMouseButtonDown(0))
            { 
                animating = false;
                allowProgress = false;
                progressText.SetActive(false);
                CutsceneProgress();
            }
            if(cameraMoving && allowProgress && Input.GetMouseButtonDown(0))
            {
                cameraMoving = false;
                progressText.SetActive(false);
                CutsceneProgress();
            }
            if (cameraMoving && allowProgress == false)
            {
                Camera.main.transform.position = Vector2.Lerp(Camera.main.transform.position, current.cameraMovePos, current.cameraMoveSpeed*Time.deltaTime);
                Vector2 tmp = Camera.main.transform.position;
                Camera.main.transform.position = new Vector3(tmp.x, tmp.y, -10.0f);
                if(Vector2.Distance(Camera.main.transform.position, current.cameraMovePos) <= 0.2f)
                {
                    Camera.main.transform.position = new Vector3(current.cameraMovePos.x, current.cameraMovePos.y, -10.0f);
                    if (current.autoSkip)
                    {
                        cameraMoving = false;
                        CutsceneProgress();
                    }
                    else
                    {
                        progressText.SetActive(true);
                        allowProgress = true;
                    }
                }
            }
        }
        if (tutorial)
        {
            if(allowTutorialProgress && Input.GetMouseButtonDown(0))
            {
                tutorialProgressText.SetActive(false);
                allowTutorialProgress = false;
                tutorialProgressCD = 0.0f;
                currentTutorialPage++;
                if(currentTutorialPage == contents[currentCutscene].tutorialPage)
                {
                    tutorial = false;
                    buttons.SetActive(true);
                    anim.SetBool("Tutorial", false);
                    GameManager.Instance.M_PlayerMovements.canMove = true;
                }
                else
                {
                    tutorialTitle.text = contents[currentCutscene].popup[currentTutorialPage].title;
                    tutorialContent.text = contents[currentCutscene].popup[currentTutorialPage].content;
                    tutorialImage.sprite = contents[currentCutscene].popup[currentTutorialPage].guideImage;
                }
            }
            else if (!allowTutorialProgress)
            {
                if (tutorialProgressCD < tutorialProgressCooldown) tutorialProgressCD += Time.deltaTime;
                else
                {
                    allowTutorialProgress = true;
                    tutorialProgressText.SetActive(true);
                }
            }
        }
    }
    public void CutsceneStart(int cutsceneNum)
    {
        for(int i = 0; i < cutsceneDisables.Length; i++)
        {
            cutsceneDisables[i].SetActive(false);
        }
        buttons.SetActive(false);
        GameManager.Instance.M_PlayerMovements.canMove = false;
        GameManager.Instance.freeCam = true;
        cutscene = true;
        anim.SetBool("Cutscene", true);
        currentCutscene = cutsceneNum;
        currentContent = -1;
        CutsceneProgress();
    }
    public void CutsceneProgress()
    {
        currentContent++;
        if (currentContent == contents[currentCutscene].element.Length)
        {
            GameManager.Instance.freeCam = false;
            anim.SetBool("Cutscene", false);
            cutscene = false;
            if (contents[currentCutscene].tutorialPopup)
            {
                currentTutorialPage = 0;
                tutorialTitle.text = contents[currentCutscene].popup[0].title;
                tutorialContent.text = contents[currentCutscene].popup[0].content;
                tutorialImage.sprite = contents[currentCutscene].popup[0].guideImage;
                tutorial = true;
                anim.SetBool("Tutorial", true);
            }
            else
            {
                buttons.SetActive(true);
                GameManager.Instance.M_PlayerMovements.canMove = true;
            }
            for (int i = 0; i < cutsceneDisables.Length; i++)
            {
                cutsceneDisables[i].SetActive(true);
            }
        }
        else if (current.talk == true)
        {
            anim.SetBool("Talk", true);
            currentTalker = null;
            if (talkLeftOpen)
            {
                talkLeftOpen = false;
                talkOpenFinish();
            }
        }
        else if(current.animationTrigger == true)
        {
            animating = true;
            if (!current.playerAnimationTrigger) anim.SetTrigger(current.triggerName);
            else playerAnim.SetTrigger(current.triggerName);
        }
        else if (current.cameraMovement)
        {
            if (current.blackScreenCameraMovement)
            {
                Debug.Log("AAAAA");
                anim.SetTrigger("Blackening");
            }
            else cameraMoving = true;
        }
    }
    public void talkOpenFinish()
    {
        currentTalk = 0;
        talkerText.gameObject.SetActive(true);
        talkText.gameObject.SetActive(true);
        talking = true;
        talk = Talk(current.talker[currentTalk], current.talkContent[currentTalk], false);
        StartCoroutine(talk);
    }
    public void AllowCutsceneProgress()
    {
        if (current.autoSkip)
        {
            CutsceneProgress();
        }
        else
        {
            allowProgress = true;
            progressText.SetActive(true);
        }
    }
    public void AllowTutorialProgress()
    {
        allowTutorialProgress = true;
    }
    IEnumerator Talk(string talker, string talkContent, bool skip)
    {
        talkText.text = "";
        talkerText.text = talker;
        for(int i = 0; i < talkContent.Length; i++)
        {
            if (talkContent[i] == '|')
            {
                yield return new WaitForSeconds(talkWaitSpeed);
            }
            else
            {
                yield return new WaitForSeconds(talkSpeed);
                talkText.text += talkContent[i];
            }
        }
        if (!skip)
        {
            talkEnded = true;
            progressText.SetActive(true);
        }
    }
    public void moveCam()
    {
        Camera.main.transform.position = new Vector3(current.cameraMovePos.x, current.cameraMovePos.y, -10.0f);
    }
    [System.Serializable]
    public class CutsceneElement
    {
        public bool animationTrigger;
        public bool playerAnimationTrigger;
        public bool autoSkip;
        public string triggerName;
        public bool talk;
        public string[] talker, talkContent;
        public bool keepTalkBox;
        public bool cameraMovement;
        public float cameraMoveSpeed;
        public bool blackScreenCameraMovement;
        public Vector2 cameraMovePos;
    }
    [System.Serializable]
    public class Cutscene
    {
        public bool tutorialPopup;
        public int tutorialPage;
        public tPopup[] popup;
        [SerializeField] public CutsceneElement[] element;
    }
    [System.Serializable]
    public struct tPopup
    {
        public string title, content;
        public Sprite guideImage;
    }

}
