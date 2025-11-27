using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource defaultAudioSource;
    [SerializeField] private AudioSource bossAudioSource;
    [SerializeField] private AudioSource effectAudioSource;
    [SerializeField] private AudioClip shootClip;
    [SerializeField] private AudioClip reloadClip;
    [SerializeField] private AudioClip moneyClip;
    [SerializeField] private AudioClip healClip;

    public void PlayShootSound()
    {
        effectAudioSource.PlayOneShot(shootClip);
    }

    public void PlayReloadSound()
    {
        effectAudioSource.PlayOneShot(reloadClip);
    }

    public void PlayMoneySound()
    {
        effectAudioSource.PlayOneShot(moneyClip);
    }

    public void PlayHealSound()
    {
        effectAudioSource.PlayOneShot(healClip);
    }

    public void PlayDefaultAudio()
    {
        bossAudioSource.Stop();
        defaultAudioSource.Play();
    }
    public void PlayBossAudio()
    {
        bossAudioSource.Play();
        defaultAudioSource.Stop();
    }

    public void StopAudioGame()
    {
        effectAudioSource.Stop();
        defaultAudioSource.Stop();
        bossAudioSource.Stop();
    }
}

