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
    private Spider spiderPrefab;
    public Transform leftSpawn;
    public Transform rightSpawn;
    public BoxCollider2D homeArea;

    [SerializeField]
    private Flea fleaPrefab;
    public BoxCollider2D fleaSpawnArea;

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

    [SerializeField]
    private int extraLifeScore = 12000;
    private int nextMilestone;

    bool roundActive = true;

    private bool fleaActive = false;

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
        nextMilestone = extraLifeScore;

        InvokeRepeating("CheckMushroomsInHomeZone", 5.0f, 0.5f);

        NewGame();
    }

    private void Update()
    {
        if (lives <= 0 && Input.GetKeyDown(KeyCode.R))
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

        roundActive = true;
        SpawnSpider(4f);
        centipede.Respawn();
        blaster.Respawn();
        mushroomField.Clear();
        mushroomField.Generate();
        gameOver.SetActive(false);
    }

    public void ReadyNextFlea(float delay)
    {
        StartCoroutine(SetFleaNotActive(delay));
    }

    private IEnumerator SetFleaNotActive(float delay)
    {
        yield return new WaitForSeconds(delay);

        fleaActive = false;
    }

    private void CheckMushroomsInHomeZone()
    {
        int mushroomCount = 0;

        Mushroom[] mushrooms = FindObjectsOfType<Mushroom>();

        foreach (Mushroom mushroom in mushrooms) // check number of mushrooms in home zone
        {
            if (mushroom.transform.position.y <= homeArea.bounds.max.y)
            {
                mushroomCount++;
            }
        }

        if (mushroomCount < 5 && !fleaActive)
        {
            fleaActive = true;
            SpawnFlea();
        }
    }

    private void SpawnFlea()
    {
        Vector2 spawnPos = fleaSpawnArea.transform.position;
        spawnPos.x = Random.Range(fleaSpawnArea.bounds.min.x, fleaSpawnArea.bounds.max.x);
        spawnPos = GridPosition(spawnPos);

        Instantiate(fleaPrefab, spawnPos, Quaternion.identity);
    }

    private void GameOver()
    {
        blaster.gameObject.SetActive(false);

        roundActive = false;
        ClearSpiders();

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

        roundActive = false;
        ClearSpiders();

        blaster.gameObject.SetActive(false);
        centipede.PauseCentipede();
        mushroomField.Heal();
    }

    private void ClearSpiders()
    {
        Spider[] spiders = FindObjectsOfType<Spider>();

        foreach (Spider spider in spiders)
        {
            Destroy(spider.gameObject);
        }
    }

    public void RespawnPlayer()
    {
        centipede.Respawn();
        centipede.ResumeCentipede();
        blaster.Respawn();
        roundActive = true;
        SpawnSpider(4f);
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

        if (score >= nextMilestone)
        {
            nextMilestone += extraLifeScore;
            SetLives(lives + 1);
        }
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

    public void SpawnSpider(float delay)
    {
        StartCoroutine(SpawnSpiderAfterDelay(delay));
    }

    private IEnumerator SpawnSpiderAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        ClearSpiders();

        if (roundActive)
        {
            bool movingRight = (Random.value > 0.5);
            Vector2 position = (movingRight ? leftSpawn.position : rightSpawn.position);
            Spider spider = Instantiate(spiderPrefab, position, Quaternion.identity);
            spider.movingRight = movingRight;
        }
    }

    private Vector2 GridPosition(Vector2 position) // make sure postion is aligned to grid
    {
        position.x = Mathf.Round(position.x);
        position.y = Mathf.Round(position.y);

        return position;
    }
}
