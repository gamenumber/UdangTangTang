using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monkey : Ability
{
    [SerializeField] Basic basicMoveset;
    public override void OnChange()
    {
        defaultMove = basicMoveset;
        base.OnChange();
    }
}
