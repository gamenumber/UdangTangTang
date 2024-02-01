using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneInvoke : Interaction
{
    [SerializeField] int cutsceneNum;
    public override void Start()
    {
        base.Start();
        requireType = false;
        remove = true;
    }
    public override void Interact()
    {
        GameManager.Instance.M_NewCutsceneManager.StartCutscene(cutsceneNum);
        Destroy(this);
    }
}
