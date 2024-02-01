using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Armadillo))]public class ArmadilloMode : Interaction
{
    Armadillo armadillo;
    public override void Start()
    {
        base.Start();
        armadillo = GetComponent<Armadillo>();
        remove = false;
        requireType = true;
        requiredType = AnimalType.armadillo;
    }
    public override void Interact()
    {
        base.Interact();
        armadillo.SpinToggle();
    }
}
