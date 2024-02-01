using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(NewCutsceneManager))]public class Tutorial : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] Image tImage;
    [SerializeField] Text tTitle, tText;
    [SerializeField] GameObject tPassText;
    [SerializeField] float tutorialPassTime = 1.0f;
    int index = 0; bool tutorialRunning = false, canProgress = false;
    NewCutsceneManager manager;
    IEnumerator tutorial;
    private void Start()
    {
        manager = GetComponent<NewCutsceneManager>();
    }
    public void StartTutorial(TutorialElements[] elements)
    {
        tutorial = RunTutorial(elements);
        StartCoroutine(tutorial);
    }
    IEnumerator RunTutorial(TutorialElements[] elements)
    {
        anim.SetBool("Tutorial", true);
        tutorialRunning = true;
        for(int i = 0; i < elements.Length ; i++)
        {
            tTitle.text = elements[i].tutorialName;
            tText.text = elements[i].tutorialContent;
            if (elements[i].useImage)
            {
                tImage.gameObject.SetActive(true);
                tImage.sprite = elements[i].tutorialImage;
            }
            else tImage.gameObject.SetActive(false);
            yield return new WaitForSeconds(tutorialPassTime);
            tPassText.SetActive(true);
            while (true)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    break;
                }
                yield return null;
            }
            tPassText.SetActive(false);
        }
        tutorialRunning = false;
        anim.SetBool("Tutorial", false);
    }
    public void SkipTutorial()
    {
        if (tutorial == null) return;
        else
        {
            StopCoroutine(tutorial);
            tPassText.SetActive(false);
            tutorialRunning = false;
            anim.SetBool("Tutorial", false);
            tutorial = null;
        }
    }
    public void EndTutorial()
    {
        manager.CutsceneProgress();
    }
}
[System.Serializable] public struct TutorialElements
{
    [Tooltip("튜토리얼의 제목")]public string tutorialName;
    [Tooltip("튜토리얼의 내용")][TextArea(3, 5)] public string tutorialContent;
    [Space(3)][Tooltip("체크하지 않으면 이미지를 사용하지 않음. 이미지가 표시되는 부분에 아무것도 안 뜸.")] public bool useImage;
    [Tooltip("useImage를 체크할 경우 사용할 이미지")] public Sprite tutorialImage;
}