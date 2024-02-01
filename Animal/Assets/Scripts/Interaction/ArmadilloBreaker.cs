using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmadilloBreaker : Interaction
{
    Armadillo armadillo;
    [SerializeField] HorizontalDir breakDir;
    [SerializeField] Transform startPos;
    [SerializeField] Animator breakAnimation;
    public override void Start()
    {
        base.Start();
        armadillo = GameManager.Instance.animals[3] as Armadillo;
        requireType = true;
        requiredType = AnimalType.armadillo;
        if (breakAnimation == null && GetComponent<Animator>() != null) breakAnimation = GetComponent<Animator>();
    }
    public override void Interact()
    {
        base.Interact();
        armadillo.StartBreak(breakDir, startPos.position, this);
    }
    public void Broken()
    {
        breakAnimation.Play("break");
        interaction.Remove(this);
        interactable = false;
    }
}
