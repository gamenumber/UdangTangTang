using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ActivatorButton : Interaction
{
    public Activated activated;
    public bool oneTime = true;
    public float recallTime = 1.0f;
    public Animator anim;

    BoxCollider2D collider;
    bool canPress = true;
    float counter = 0.0f;
    public override void Start()
    {
        base.Start();
        requireType = false;
        remove = true;
        if(anim==null && GetComponent<Animator>()!=null) anim = GetComponent<Animator>();
        if(!oneTime) collider = GetComponent<BoxCollider2D>();
    }
    public override void OnTriggerEnter2D(Collider2D other)
    {
        if (!canPress) return;
        base.OnTriggerEnter2D(other);
    }
    public override void Interact()
    {
        base.Interact();
        activated.OnActivation();
        anim.SetBool("Pressed", true);
        if (oneTime) interactable = false;
        else
        {
            canPress = false;
            StartCoroutine(ButtonWait());
        }
    }
    IEnumerator ButtonWait()
    {
        yield return new WaitForSeconds(recallTime);
        anim.SetBool("Pressed", false);
        canPress = true;
        collider.enabled = false; collider.enabled = true;
    }
}
