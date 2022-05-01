using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField]
    private Text highScoreText;
    [SerializeField]
    private Image[] lifeIcons;
    [SerializeField]
    private Sprite[] lifeSprites;

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

        SetupStats();
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    private void Start()
    {
        HideLifeIcons();
    }

    private void SetupStats()
    {
        if (!PlayerPrefs.HasKey("CentipedeHighScore"))
        {
            PlayerPrefs.SetInt("CentipedeHighScore", 0);
        }

        highScoreText.text = PlayerPrefs.GetInt("CentipedeHighScore").ToString();
    }

    public void UpdateLives(int lives)
    {
        HideLifeIcons();

        for (int i = 0; i < (lives - 1); i++)
        {
            if (i < lifeIcons.Length)
            {
                lifeIcons[i].enabled = true;
            }
        }
    }

    private void HideLifeIcons()
    {
        foreach (Image life in lifeIcons)
        {
            life.enabled = false;
        }
    }

    public void UpdateLiveColors()
    {
        foreach (Image life in lifeIcons)
        {
            life.sprite = lifeSprites[GameManager.Instance.currentIndex];
        }
    }
}
