using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private Blaster blaster;
    private Centipede centipede;
    private MushroomField mushroomField;

    private int score;
    private int lives;

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
        centipede = FindObjectOfType<Centipede>();
        mushroomField = FindObjectOfType<MushroomField>();

        NewGame();
    }

    private void NewGame()
    {
        score = 0;
        lives = 3;

        centipede.Respawn();
        blaster.Respawn();
        mushroomField.Clear();
        mushroomField.Generate();
    }

    private void GameOver()
    {

    }

    private void ResetRound()
    {

    }

    private void NextLevel()
    {

    }
}
