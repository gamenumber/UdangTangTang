using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurretTarget : MonoBehaviour
{
    public List<Turret> targetted;
    [SerializeField] Animator targetMark;
    [SerializeField] Text targetAmount;
    public void TargetUpdate()
    {
        if (targetted.Count > 1)
        {
            targetAmount.text = "" + targetted.Count;
            targetAmount.gameObject.SetActive(true);
        }
        else targetAmount.gameObject.SetActive(false);
        targetMark.SetBool("Targetted", targetted.Count>0);
    }
}
