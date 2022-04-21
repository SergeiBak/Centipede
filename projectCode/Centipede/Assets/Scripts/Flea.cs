using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flea : MonoBehaviour
{
    private BoxCollider2D homeArea;
    [SerializeField]
    private float speed = 20f;
    [SerializeField]
    private float nextFleaDelay = 1f;
    [SerializeField]
    private Mushroom mushroomPrefab;
    private float minY;

    private int health = 2;

    private Vector2 targetPosition;

    private float nextMushroomTimeStamp;

    private void Awake()
    {
        homeArea = GameManager.Instance.homeArea;
    }

    private void Start()
    {
        minY = homeArea.bounds.min.y - 1f;
        targetPosition = transform.position;
        targetPosition.y = minY - 5f;

        nextMushroomTimeStamp = Time.time + Random.Range(0.1f, 0.3f);
    }

    private void Update()
    {
        Vector2 currentPosition = transform.position;
        float fleaSpeed = speed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(currentPosition, targetPosition, fleaSpeed);

        if (Time.time >= nextMushroomTimeStamp)
        {
            SpawnMushroom();
            nextMushroomTimeStamp = Time.time + Random.Range(0.1f, 0.3f);
        }

        CheckIfReachedBottom();
    }

    private void CheckIfReachedBottom()
    {
        if (transform.position.y <= minY)
        {
            GameManager.Instance.ReadyNextFlea(nextFleaDelay);
            Destroy(gameObject);
        }
    }

    private void IncreaseSpeed()
    {
        speed *= 1.5f;
    }

    private void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            GameManager.Instance.ReadyNextFlea(0);
            Destroy(gameObject);
        }
        else
        {
            IncreaseSpeed();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.enabled && collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            collision.collider.enabled = false;
            GameManager.Instance.ResetRound();
            GameManager.Instance.ReadyNextFlea(5f);
            Destroy(gameObject);
            return;
        }

        if (collision.collider.enabled && collision.gameObject.layer == LayerMask.NameToLayer("Dart"))
        {
            collision.collider.enabled = false;
            TakeDamage(1);
        }
    }

    private void SpawnMushroom()
    {
        Vector2 spawnPos = transform.position;
        spawnPos = GridPosition(spawnPos);

        if (!(spawnPos.y < (homeArea.bounds.min.y + 1f)))
        {
            Instantiate(mushroomPrefab, spawnPos, Quaternion.identity);
        }
    }

    private Vector2 GridPosition(Vector2 position) // make sure postion is aligned to grid
    {
        position.x = Mathf.Round(position.x);
        position.y = Mathf.Round(position.y);

        return position;
    }
}
