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
    [SerializeField] private AudioClip outOfBullet;
    [SerializeField] private AudioClip reloadClip;
    [SerializeField] private AudioClip moneyClip;
    [SerializeField] private AudioClip healClip;
    [SerializeField] private AudioClip playerDie;
    [SerializeField] private AudioClip playerHurt;
    [SerializeField] private AudioClip bossDie;
    [SerializeField] private GameManager gameManager;
    public void PlayShootSound()
    {
        effectAudioSource.PlayOneShot(shootClip);
    }

    public void PlayReloadSound()
    {
        effectAudioSource.PlayOneShot(reloadClip);
    }

    public void PlayOutOfBulletSound()
    {
        effectAudioSource.PlayOneShot(outOfBullet);
    }

    public void PlayHurtSound()
    {
        effectAudioSource.PlayOneShot(playerHurt);
    }

    public void PlayMoneySound()
    {
        effectAudioSource.PlayOneShot(moneyClip);
    }

    public void PlayPlayerDie()
    {
        effectAudioSource.PlayOneShot(playerDie);
    }
    public void PlayBossDie()
    {
        effectAudioSource.PlayOneShot(bossDie);
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
    public void StopAudioDefault()
    {
        defaultAudioSource.Pause();
    }

    public void StopAudioBoss()
    {
        bossAudioSource.Pause();
    }

    public void ContinueAudioDefault()
    {
        defaultAudioSource.Play();
    }
    public void ContinueAudioBoss()
    {
        bossAudioSource.Play();
    }

}

