using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    [HideInInspector] public bool requireType, remove;
    [HideInInspector] public AnimalType requiredType;
    protected PlayerInteraction interaction;
    public bool interactable = true;
    public int priority;
    public Sprite uniqueButton;
    public virtual void Start()
    {
        interaction = GameManager.Instance.M_PlayerInteraction;
    }
    public virtual void Interact()
    {

    } 
    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && interaction.interactions.Contains(this) == false)
        {
            interaction.Add(this);
        }
    }
    public virtual void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player" && interaction.interactions.Contains(this))
        {
            interaction.Remove(this);
        }
    }
}
[System.Serializable]
public enum Direction
{
    Right, Left, Up, Down
}
[System.Serializable]
public enum HorizontalDir
{
    Right, Left
}