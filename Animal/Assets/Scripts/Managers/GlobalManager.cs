using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class GlobalManager : MonoBehaviour
{
    private static GlobalManager instance;

    [SerializeField] private GameObject[] _hidestage;

    public bool[] stageComplete = new bool[3];
    public int[] stageRecords = new int[3];
    public bool sceneChanging = false;

    public UnityEvent onSceneChange = new UnityEvent();

    GameObject globalObject = null;
    Animator globalAnim = null;
    string changingScene;
    public static GlobalManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GlobalManager>();
                if (instance == null)
                {
                    GameObject singletonObject = new GameObject();
                    instance = singletonObject.AddComponent<GlobalManager>();
                    singletonObject.name = "SelectManager (Singleton)";
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            globalObject = Instantiate(Resources.Load("Prefabs/DontDestroyObject") as GameObject);
            globalAnim = globalObject.transform.GetChild(0).GetComponent<Animator>();
            DontDestroyOnLoad(this.gameObject);
            DontDestroyOnLoad(globalObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void LoadStage(int stageNum)
    {
        if (stageNum > 0 && !stageComplete[stageNum-1]) return;
        sceneChanging = true;
        changingScene = "" + stageNum;
        onSceneChange.Invoke();
        globalAnim.SetTrigger("SceneChange");
    }
    public void LoadScene(string sceneName)
    {
        sceneChanging = true;
        changingScene = sceneName;
        onSceneChange.Invoke();
        globalAnim.SetTrigger("SceneChange");
    }
    public void ExecuteChange()
    {
        SceneManager.LoadScene(changingScene);
        globalAnim.SetTrigger("SceneChanged");
    }

    public void SetStageComplete(int stageNumber)
    {
        stageComplete[stageNumber] = true;
    }
    public void SetRecord(int stage, int record)
    {
        stageRecords[stage] = record;
    }

    /* 구성 -> 0스테이햨E튜토리푳E 끝나컖E 게임 시작 버튼을 눌렀을 때 이 씬이 뜨게쾪E 
     * -> 1스테이햨E내에서 2스테이지를 빨간 파이프 이퓖E漫� 넘푳E갈 펯E있엉幕 굳이 스테이햨E선택창에서 선택안해도 바로 넘엉怯 펯E있음.*/



    /* 사퓖E-> 0스테이햨E끝나는 부분 ->  : SelectManager.Instance.SetBool(1, true);
               1스테이햨E끝나는 부분 ->  : SelectManager.Instance.SetBool(2, true); */



}
