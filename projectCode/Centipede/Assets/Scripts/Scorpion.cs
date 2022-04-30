using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scorpion : MonoBehaviour
{
    private BoxCollider2D spawnArea;
    private float maxY;
    private float minY;
    private Vector2 targetPosition;

    [SerializeField]
    private float scorpionSpeed = 10f;
    [SerializeField]
    private int points = 1000;

    public bool movingRight { private get; set; }

    private SpriteRenderer sr;

    private void Awake()
    {
        spawnArea = GameManager.Instance.scorpionSpawnArea;
        sr = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        maxY = spawnArea.bounds.max.y;
        minY = spawnArea.bounds.min.y;

        targetPosition = transform.position;
        targetPosition.x = (movingRight ? (spawnArea.bounds.max.x + 2f) : (spawnArea.bounds.min.x - 2f));

        if (movingRight)
        {
            sr.flipX = true;
        }
    }

    private void Update()
    {
        Vector2 currentPosition = transform.position;
        float speed = scorpionSpeed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(currentPosition, targetPosition, speed);

        CheckIfReachedOtherSide();
    }

    private void CheckIfReachedOtherSide()
    {
        if (movingRight && transform.position.x > spawnArea.bounds.max.x)
        {
            GameManager.Instance.ReadyNextScorpion(0);
            Destroy(gameObject);
        }
        else if (!movingRight && transform.position.x < spawnArea.bounds.min.x)
        {
            GameManager.Instance.ReadyNextScorpion(0);
            Destroy(gameObject);
        }
    }

    private void ScorpionShot()
    {
        GameManager.Instance.IncreaseScore(points);
        GameManager.Instance.ReadyNextScorpion(0);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.enabled && collision.gameObject.layer == LayerMask.NameToLayer("Dart"))
        {
            collision.enabled = false;
            ScorpionShot();
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Mushroom")) // infect mushroom
        {
            Mushroom mushroom = collision.gameObject.GetComponent<Mushroom>();
            mushroom.Infect();
        }
    }
}
