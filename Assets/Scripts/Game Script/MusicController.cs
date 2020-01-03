using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioSource WhistleAudioSource;
    public AudioClip GearClip;
    public AudioClip refWhistleClip;
    public AudioClip gameStartWhistleClip;
    public AudioClip goalClip;
    public AudioClip WaterSplashClip;
    public AudioClip RankClip;
    float orig_volume;

    void Start()
    {
        orig_volume = WhistleAudioSource.volume;
    }

    public void PlayRank(){
        audioSource.PlayOneShot(RankClip, 1.0f);
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
        if (WhistleAudioSource != null)
        {
            WhistleAudioSource.clip = goalClip;
            WhistleAudioSource.Play();
            StartCoroutine(Decreasing());
        }
    }

    IEnumerator Decreasing()
    {
        float decrement = 0.003f;
        float cur_volume = orig_volume;
        while (cur_volume >= 0)
        {
            cur_volume -= decrement;
            WhistleAudioSource.volume = cur_volume;
            yield return 0;
        }
        WhistleAudioSource.Stop();
        WhistleAudioSource.volume = orig_volume;
    }

    public void PlayWaterDrop()
    {
        audioSource.PlayOneShot(WaterSplashClip, 1.0f);
    }
}
