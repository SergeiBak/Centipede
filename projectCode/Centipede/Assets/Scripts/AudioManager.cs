using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField]
    private AudioSource shootSource;
    [SerializeField]
    private AudioClip shootSound;

    [SerializeField]
    private AudioSource deathSource;
    [SerializeField]
    private AudioClip deathSound;


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
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    public void PlayShootSound()
    {
        shootSource.PlayOneShot(shootSound, 1f);
    }

    public void PlayDeathSound()
    {
        deathSource.PlayOneShot(deathSound, 1f);
    }
}
