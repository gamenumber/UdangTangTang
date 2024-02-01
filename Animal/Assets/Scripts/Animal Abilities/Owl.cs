using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(OwlFly))]public class Owl : Ability
{
    [SerializeField] OwlFly flyAbility;
    [SerializeField] GameObject darkArea, nightVision;
    [SerializeField] Basic basicMoveset;
    PlayerInteraction interaction;
    private void Start()
    {
        flyAbility = GetComponent<OwlFly>();
        interaction = GameManager.Instance.M_PlayerInteraction;
    }
    public override void OnChange()
    {
        defaultMove = basicMoveset;
        base.OnChange();
        darkArea.SetActive(false);
        nightVision.SetActive(true);
    }
    public override void OnExit()
    {
        base.OnExit();
        darkArea.SetActive(true);
        nightVision.SetActive(false);
    }
    private void LateUpdate()
    {
        if(GameManager.Instance.M_PlayerMovements.grounded == false)
        {
            interaction.Add(flyAbility);
        }
        else
        {
            interaction.Remove(flyAbility);
        }
    }
}
