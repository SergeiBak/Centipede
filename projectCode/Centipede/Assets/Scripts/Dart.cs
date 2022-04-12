using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dart : MonoBehaviour
{
    private Rigidbody2D rb;
    private new Collider2D collider;
    private Transform parent;

    [SerializeField]
    private float speed = 50f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;

        collider = GetComponent<Collider2D>();
        collider.enabled = false;

        parent = transform.parent;
    }

    private void Update()
    {
        if (rb.isKinematic && (Input.GetButton("Fire1") || Input.GetKey(KeyCode.Space)))
        {
            transform.SetParent(null);
            rb.bodyType = RigidbodyType2D.Dynamic;
            collider.enabled = true;
        }
    }

    private void FixedUpdate()
    {
        if (!rb.isKinematic)
        {
            Vector2 position = rb.position;
            position += Vector2.up * speed * Time.fixedDeltaTime;
            rb.MovePosition(position);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        transform.SetParent(parent);
        transform.localPosition = new Vector3(0f, 0.5f, 0f);
        rb.bodyType = RigidbodyType2D.Kinematic;
        collider.enabled = false;
    }
}
