using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundController : MonoBehaviour
{
    public AudioClip shootClip;       // 彈射出去音效
    public AudioClip inflateClip;     // 充氣增大的音效
    public AudioClip burstClip;       // 爆炸音效
    public AudioClip freezeClip;      // 凍結音效
    AudioSource audioSource;

    void Start()
    {
        audioSource = this.gameObject.GetComponent<AudioSource> ();
    }

    public void PlayShootClip()
    {
        audioSource.PlayOneShot(shootClip, 0.3f);
    }

    public void PlayBurstClip()
    {
        audioSource.PlayOneShot(burstClip, 0.5f);
    }

    public void PlayFreezeClip()
    {
        audioSource.PlayOneShot(freezeClip, 0.9f);
    }

    public void PlayInflateClip()
    {
        audioSource.clip = inflateClip;
        audioSource.Play();
    }

    public void StopPlaying()
    {
        audioSource.Stop();
    }
}
