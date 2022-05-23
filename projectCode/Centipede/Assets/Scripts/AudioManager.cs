using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    private AudioSource backgroundSfx;

    [SerializeField]
    private AudioSource shootSource;
    [SerializeField]
    private AudioClip shootSound;

    [SerializeField]
    private AudioSource deathSource;
    [SerializeField]
    private AudioClip deathSound;

    [SerializeField]
    private AudioSource enemyDeathSource;
    [SerializeField]
    private AudioClip enemyDeathSound;


    private void Awake()
    {
        if (Instance == null) // establish singleton pattern
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        backgroundSfx = GetComponent<AudioSource>();
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    public void PauseBackgroundSFX()
    {
        backgroundSfx.Stop();
    }

    public void ResumeBackgroundSFX()
    {
        backgroundSfx.Play();
    }

    public void PlayShootSound()
    {
        shootSource.PlayOneShot(shootSound, 1f);
    }

    public void PlayDeathSound()
    {
        deathSource.PlayOneShot(deathSound, 1f);
    }

    public void PlayEnemyDeathSound()
    {
        enemyDeathSource.PlayOneShot(enemyDeathSound, 1f);
    }
}
