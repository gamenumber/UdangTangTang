using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    public AnimalType animalType;
    public GameObject[] animalBody;
    public bool canChange = true;
    PlayerInteraction interaction;
    PlayerMovements movements;
    private void Start()
    {
        interaction = GetComponent<PlayerInteraction>();
        movements = GetComponent<PlayerMovements>();
        for(int i = 0; i < GameManager.Instance.animals.Length; i++)
        {
            animalBody[i] = GameManager.Instance.animals[i].gameObject;
        }
        animalBody[(int)animalType].GetComponent<Ability>().OnChange();
        animalBody[(int)animalType].SetActive(true);
    }
    public void ChangeAnimal(int animalNum)
    {
        if (animalNum == (int)animalType) return;
        interaction.Refresh();
        animalBody[(int)animalType].GetComponent<Ability>().OnExit();
        animalBody[animalNum].GetComponent<Ability>().OnChange();
        animalBody[animalNum].SetActive(true);
        animalBody[(int)animalType].SetActive(false);
        animalType = (AnimalType)animalNum;
    }
}
public enum AnimalType
{
    slime,
    monkey,
    owl,
    armadillo
}
