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

    /* ｱｸｼｺ -> 0ｽｺﾅﾗﾀﾌﾁ・ﾆｩﾅ荳ｮｾ・ ｳ｡ｳｪｰ・ ｰﾔﾀﾓ ｽﾃﾀﾛ ｹｰﾀｻ ｴｭｷｶﾀｻ ｶｧ ﾀﾌ ｾﾀﾀﾌ ｶﾟｰﾔｲ・ 
     * -> 1ｽｺﾅﾗﾀﾌﾁ・ｳｻｿ｡ｼｭ 2ｽｺﾅﾗﾀﾌﾁｦ ｻ｡ｰ｣ ﾆﾄﾀﾌﾇﾁ ﾀﾌｿ・ﾘｼｭ ｳﾑｾ・ｰ･ ｼ・ﾀﾖｾ錡ｭ ｱｻﾀﾌ ｽｺﾅﾗﾀﾌﾁ・ｼｱﾅﾃﾃ｢ｿ｡ｼｭ ｼｱﾅﾃｾﾈﾇﾘｵｵ ｹﾙｷﾎ ｳﾑｾ銧･ ｼ・ﾀﾖﾀｽ.*/



    /* ｻ鄙・-> 0ｽｺﾅﾗﾀﾌﾁ・ｳ｡ｳｪｴﾂ ｺﾎｺﾐ ->  : SelectManager.Instance.SetBool(1, true);
               1ｽｺﾅﾗﾀﾌﾁ・ｳ｡ｳｪｴﾂ ｺﾎｺﾐ ->  : SelectManager.Instance.SetBool(2, true); */



}
