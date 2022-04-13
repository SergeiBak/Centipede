using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom : MonoBehaviour
{
    [SerializeField]
    private Sprite[] states;

    private int health;

    private SpriteRenderer sr;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        health = states.Length;
    }

    private void Damage(int amount)
    {
        health -= amount;

        if (health > 0)
        {
            sr.sprite = states[states.Length - health]; // Encapsulate into separate method later
        } 
        else
        {
            Destroy(gameObject);
        }
    }

    public void Heal()
    {
        health = states.Length;
        sr.sprite = states[0];
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Dart"))
        {
            Damage(1);
        }
    }
}
