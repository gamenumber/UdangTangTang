using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Timers;
using UnityEngine;
using UnityEngine.UI;

public class StageSelect : MonoBehaviour
{
    [SerializeField] GameObject[] _hidestage;
    [SerializeField] StageDesc[] stageDesc;
    [SerializeField] Animator anim;
    [SerializeField] Text stageNameText, stageDescText, recordText;
    [SerializeField] GameObject completion;
    int loadingStage = 0;
    private void Start()
    {
        for (int i = 0; i < _hidestage.Length; i++)
        {
            if (i>0&&!GlobalManager.Instance.stageComplete[i-1]) _hidestage[i].SetActive(true);
            else _hidestage[i].SetActive(false);
        }
    }
    public void OpenStageMenu(int stage)
    {
        if (stage>0 && !GlobalManager.Instance.stageComplete[stage - 1]) return;
        loadingStage = stage;
        if (GlobalManager.Instance.stageComplete[stage]) completion.SetActive(false);
        else completion.SetActive(true);
        int timer = GlobalManager.Instance.stageRecords[stage];
        recordText.text = ((timer / 60 < 10) ? "0" + timer / 60 : (timer / 60)) + ":" + ((timer % 60 < 10) ? "0" + timer % 60 : (timer % 60));
        stageNameText.text = stageDesc[stage].stageName;
        stageDescText.text = stageDesc[stage].stageDescription;
        anim.SetTrigger("Open");
    }
    public void CloseStageMenu()
    {
        anim.SetTrigger("Close");
    }
    public void StageLoad()
    {
        GlobalManager.Instance.LoadStage(loadingStage);
    }
    public void ReturnToMain()
    {
        GlobalManager.Instance.LoadScene("Main");
    }
}
[System.Serializable] struct StageDesc
{
    public string stageName;
    [TextArea] public string stageDescription;
}
