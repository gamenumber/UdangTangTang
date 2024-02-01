using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneTrigger : MonoBehaviour
{
    [SerializeField] int triggerCutscene;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            GameManager.Instance.M_NewCutsceneManager.StartCutscene(triggerCutscene);
            gameObject.SetActive(false);
        }
    }
}
