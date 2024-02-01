using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class SceneMusic : MonoBehaviour
{
    [SerializeField] AudioSource source;
    private void Start()
    {
        GlobalManager.Instance.onSceneChange.AddListener(FadeOut);
    }
    public void FadeOut()
    {
        StartCoroutine(FadingOut());
    }
    IEnumerator FadingOut()
    {
        for(int i = 0; i < 10; i++)
        {
            source.volume -= 0.1f;
            yield return new WaitForSeconds(0.05f);
        }
    }
    public void MusicPlay(AudioClip changingClip)
    {
        source.clip = changingClip;
        source.volume = 1.0f;
        source.Play();
    }
}
