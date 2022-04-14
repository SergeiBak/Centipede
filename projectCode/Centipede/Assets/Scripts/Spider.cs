using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider : MonoBehaviour
{
    private BoxCollider2D homeArea;
    private Transform leftSpawn;
    private Transform rightSpawn;
    [SerializeField]
    private float spiderSpeed = 10f;

    [SerializeField]
    private float reachedSideDelay = 1.9f;
    [SerializeField]
    private float killedDelay = 4f;

    private Vector2 targetPosition;
    public bool movingRight { private get; set; }
    private float maxY;
    private float minY;

    private void Awake()
    {
        leftSpawn = GameManager.Instance.leftSpawn;
        rightSpawn = GameManager.Instance.rightSpawn;
        homeArea = GameManager.Instance.homeArea;
    }

    private void Start()
    {
        maxY = homeArea.bounds.max.y;
        minY = homeArea.bounds.min.y;

        targetPosition = transform.position;
        targetPosition.x += (movingRight ? 1f : -1f);
        targetPosition = GridPosition(targetPosition);
    }

    private void Update()
    {
        if (Vector2.Distance(transform.position, targetPosition) < 0.1f) // spider has basically reached its target position
        {
            CalculateNextTargetPosition();
        }

        Vector2 currentPosition = transform.position;
        float speed = spiderSpeed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(currentPosition, targetPosition, speed);

        CheckIfReachedOtherSide();
    }

    private void CheckIfReachedOtherSide()
    {
        if (movingRight && transform.position.x > rightSpawn.position.x)
        {
            GameManager.Instance.SpawnSpider(reachedSideDelay);
            Destroy(gameObject);
        }
        else if (!movingRight && transform.position.x < leftSpawn.position.x)
        {
            GameManager.Instance.SpawnSpider(reachedSideDelay);
            Destroy(gameObject);
        }
    }

    private void CalculateNextTargetPosition()
    {
        int directionType = Random.Range(1, 5); // 1 = up, 2 = down, 3 = diagonal up, 4 = diagonal down
        targetPosition = transform.position;
        float positionOffset;

        switch (directionType)
        {
            case 1: // up
                targetPosition.y = Random.Range(transform.position.y, maxY);
                break;
            case 2: // down
                targetPosition.y = Random.Range(minY, transform.position.y);
                break;
            case 3: // diagonal up
                positionOffset = Random.Range(transform.position.y, maxY);
                targetPosition.x += (movingRight ? (positionOffset - targetPosition.y) : -(positionOffset - targetPosition.y));
                targetPosition.y = positionOffset;
                break;
            case 4: // diagonal down
                positionOffset = Random.Range(minY, transform.position.y);
                targetPosition.x += (movingRight ? (targetPosition.y - positionOffset) : -(targetPosition.y - positionOffset));
                targetPosition.y = positionOffset;
                break;
            default:
                Debug.LogError("Spider -> CalculateNextTargetPosition() = Invalid switch case!");
                break;
        }

        targetPosition = GridPosition(targetPosition);
    }

    private Vector2 GridPosition(Vector2 position) // make sure postion is aligned to grid
    {
        position.x = Mathf.Round(position.x);
        position.y = Mathf.Round(position.y);

        return position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.enabled && collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            collision.enabled = false;
            GameManager.Instance.ResetRound();
            return;
        }

        if (collision.enabled && collision.gameObject.layer == LayerMask.NameToLayer("Dart"))
        {
            collision.enabled = false;
            SpiderShot();
        }
    }

    private void SpiderShot()
    {
        GameManager.Instance.SpawnSpider(killedDelay);
        Destroy(gameObject);
    }
}
