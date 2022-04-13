using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private Blaster blaster;
    private Dart dart;
    private Centipede centipede;
    private MushroomField mushroomField;

    [SerializeField]
    private Text scoreText;
    [SerializeField]
    private Text livesText;
    [SerializeField]
    private GameObject gameOver;

    private int score;
    private int lives;

    [SerializeField]
    private int maxIndex = 13;
    public int currentIndex { get; private set; }

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

    private void Start()
    {
        blaster = FindObjectOfType<Blaster>();
        dart = FindObjectOfType<Dart>();
        centipede = FindObjectOfType<Centipede>();
        mushroomField = FindObjectOfType<MushroomField>();

        NewGame();
    }

    private void Update()
    {
        if (lives <= 0 && Input.anyKeyDown)
        {
            NewGame();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            SetColorIndex(currentIndex + 1);
        }
    }

    private void NewGame()
    {
        SetScore(0);
        SetLives(3);
        SetColorIndex(0);

        centipede.Respawn();
        blaster.Respawn();
        mushroomField.Clear();
        mushroomField.Generate();
        gameOver.SetActive(false);
    }

    private void GameOver()
    {
        blaster.gameObject.SetActive(false);
        gameOver.SetActive(true);
    }

    public void ResetRound()
    {
        SetLives(lives - 1);

        if (lives <= 0)
        {
            GameOver();
            return;
        }

        blaster.gameObject.SetActive(false);
        centipede.PauseCentipede();
        mushroomField.Heal();
    }

    public void RespawnPlayer()
    {
        centipede.Respawn();
        centipede.ResumeCentipede();
        blaster.Respawn();
    }

    public void NextLevel()
    {
        centipede.speed *= 1.1f;
        centipede.Respawn();

        SetColorIndex(currentIndex + 1);
    }

    public void IncreaseScore(int amount)
    {
        SetScore(score + amount);
    }

    private void SetScore(int value)
    {
        score = value;
        scoreText.text = score.ToString();
    }

    private void SetLives(int value)
    {
        lives = value;
        livesText.text = lives.ToString();
    }

    private void SetColorIndex(int index)
    {
        if (index > maxIndex)
        {
            index = 0;
        }

        currentIndex = index;
        blaster.UpdateColor();
        dart.UpdateColor();
        mushroomField.UpdateMushroomColors();
    }
}
