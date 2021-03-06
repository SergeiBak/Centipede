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
    private Scorpion scorpionPrefab;
    public BoxCollider2D scorpionSpawnArea;

    [SerializeField]
    private Text scoreText;
    [SerializeField]
    private GameObject gameOver;

    private int score;
    private int lives;
    private int wave;

    [SerializeField]
    private int maxIndex = 13;
    public int currentIndex { get; private set; }

    [SerializeField]
    private int extraLifeScore = 12000;
    private int nextMilestone;

    bool roundActive = true;
    bool canRestart = false;

    private bool fleaActive = false;
    private bool scorpionActive = false;

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
        InvokeRepeating("CheckIfTimeToSpawnScorpion", 12f, 9f);

        NewGame();
    }

    private void Update()
    {
        if (NoLivesLeft() && canRestart && Input.GetKeyDown(KeyCode.R))
        {
            canRestart = false;
            NewGame();
        }
    }

    public bool NoLivesLeft()
    {
        return lives <= 0;
    }

    private void NewGame()
    {
        SetScore(0);
        SetLives(3);
        SetColorIndex(0);
        wave = 1;
        UpdateCentipedeSpeed();

        AudioManager.Instance.ResumeBackgroundSFX();

        roundActive = true;
        SpawnSpider(4f);
        centipede.ResumeCentipede();
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

        if (mushroomCount < 5 && !fleaActive && blaster.isActiveAndEnabled && !scorpionActive)
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

    private void ClearFleas()
    {
        Flea[] fleas = FindObjectsOfType<Flea>();

        foreach (Flea flea in fleas)
        {
            Destroy(flea.gameObject);
        }

        fleaActive = false;
    }

    public void ReadyNextScorpion(float delay)
    {
        StartCoroutine(SetScorpionNotActive(delay));
    }

    private IEnumerator SetScorpionNotActive(float delay)
    {
        yield return new WaitForSeconds(delay);

        scorpionActive = false;
    }

    private void CheckIfTimeToSpawnScorpion()
    {
        if (!scorpionActive && !fleaActive && blaster.isActiveAndEnabled && (wave >= 3))
        {
            scorpionActive = true;

            SpawnScorpion();
        }
    }

    private void SpawnScorpion()
    {
        Vector2 spawnPos;
        bool movingRight = (Random.value > 0.5);
        spawnPos.x = (movingRight ? scorpionSpawnArea.bounds.min.x : scorpionSpawnArea.bounds.max.x);
        spawnPos.y = Random.Range(scorpionSpawnArea.bounds.min.y, scorpionSpawnArea.bounds.max.y);
        spawnPos = GridPosition(spawnPos);

        Scorpion scorpion = Instantiate(scorpionPrefab, spawnPos, Quaternion.identity);
        scorpion.movingRight = movingRight;
    }

    private void ClearScorpions()
    {
        Scorpion[] scorpions = FindObjectsOfType<Scorpion>();

        foreach (Scorpion scorpion in scorpions)
        {
            Destroy(scorpion.gameObject);
        }

        scorpionActive = false;
    }

    private void GameOver()
    {
        blaster.PlayDeathAnimation();
        blaster.gameObject.SetActive(false);

        roundActive = false;
        ClearSpiders();
        ClearFleas();
        ClearScorpions();
        centipede.PauseCentipede();

        AudioManager.Instance.PauseBackgroundSFX();

        StartCoroutine(StartEndSequence());
    }

    private IEnumerator StartEndSequence(float delay = 1.5f)
    {
        yield return new WaitForSeconds(delay);

        mushroomField.Heal();
    }

    public void CanRestart()
    {
        UIManager.Instance.CheckHighScore(score);

        gameOver.SetActive(true);
        canRestart = true;
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
        ClearFleas();
        ClearScorpions();

        blaster.PlayDeathAnimation();

        AudioManager.Instance.PauseBackgroundSFX();

        blaster.gameObject.SetActive(false);
        centipede.PauseCentipede();
        StartCoroutine(StartRepairSequence());
    }

    private IEnumerator StartRepairSequence(float delay = 1.5f)
    {
        yield return new WaitForSeconds(delay);

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
        AudioManager.Instance.ResumeBackgroundSFX();
        roundActive = true;
        SpawnSpider(4f);
    }

    public void NextLevel()
    {
        centipede.Respawn();

        SetColorIndex(currentIndex + 1);

        wave++;
        if (wave > 12)
        {
            wave = 1;
        }
        UpdateCentipedeSpeed();
    }

    private void UpdateCentipedeSpeed()
    {
        switch (wave)
        {
            case 1:
                SetCentipedeSpeed(centipede.fastSpeed);
                break;
            case 2:
                SetCentipedeSpeed(centipede.slowSpeed);
                break;
            case 3:
                SetCentipedeSpeed(centipede.fastSpeed);
                break;
            case 4:
                SetCentipedeSpeed(centipede.slowSpeed);
                break;
            case 5:
                SetCentipedeSpeed(centipede.fastSpeed);
                break;
            case 6:
                SetCentipedeSpeed(centipede.slowSpeed);
                break;
            case 7:
                SetCentipedeSpeed(centipede.fastSpeed);
                break;
            case 8:
                SetCentipedeSpeed(centipede.slowSpeed);
                break;
            default:
                SetCentipedeSpeed(centipede.fastSpeed);
                break;
        }
    }

    private void SetCentipedeSpeed(float speed)
    {
        centipede.SetOriginalSpeed(speed);
        if (!(centipede.speed == 0))
        {
            centipede.speed = speed;
        }
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

            AudioManager.Instance.PlayExtraLifeSound();
        }
    }

    private void SetLives(int value)
    {
        lives = value;
        UIManager.Instance.UpdateLives(lives);
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
        UIManager.Instance.UpdateLiveColors();
        UIManager.Instance.UpdateTextColors();
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
