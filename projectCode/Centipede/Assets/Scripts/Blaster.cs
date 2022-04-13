using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blaster : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 direction;
    private Vector2 spawnPosition;
    [SerializeField]
    private float speed = 20f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spawnPosition = transform.position;
    }

    private void Update()
    {
        direction.x = Input.GetAxis("Horizontal");
        direction.y = Input.GetAxis("Vertical");
    }

    private void FixedUpdate()
    {
        Vector2 position = rb.position;
        position += direction.normalized * speed * Time.fixedDeltaTime;
        rb.MovePosition(position);
    }

    public void Respawn()
    {
        transform.position = spawnPosition;
        gameObject.SetActive(true);
    }
}
