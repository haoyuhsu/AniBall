using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip GearClip;
    public AudioClip refWhistleClip;
    public AudioClip gameStartWhistleClip;
    public AudioClip goalClip;
    float orig_volume;

    void Start()
    {
        orig_volume = audioSource.volume;
    }

    public void PlayGearClip()
    {
        audioSource.PlayOneShot(GearClip, 1.0f);
    }

    public void PlayRefWhistle()
    {
        audioSource.PlayOneShot(refWhistleClip, 0.8f);
    }

    public void PlayGameStartWhistle()
    {
        audioSource.PlayOneShot(gameStartWhistleClip, 0.7f);
    }

    public void PlayGoalClip()
    {
        audioSource.clip = goalClip;
        audioSource.PlayOneShot(goalClip);
        StartCoroutine(Decreasing());
    }

    IEnumerator Decreasing()
    {
        float decrement = 0.005f;
        float cur_volume = orig_volume;
        while (cur_volume >= 0)
        {
            cur_volume -= decrement;
            audioSource.volume = cur_volume;
            yield return 0;
        }
        audioSource.Stop();
        audioSource.volume = orig_volume;
    }
}
