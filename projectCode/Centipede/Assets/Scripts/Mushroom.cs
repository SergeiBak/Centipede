using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom : MonoBehaviour
{
    [SerializeField]
    private Sprite[] states;
    public int points = 1;
    private int health;
    private SpriteRenderer sr;
    [SerializeField]
    MushroomRepairAnim repairAnimation;

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
            GameManager.Instance.IncreaseScore(points);
        }
    }

    public void Heal()
    {
        health = states.Length;
        sr.sprite = states[0];
        repairAnimation.PlayRepairAnimation(0.1f);
    }

    public bool IsFullHealth()
    {
        return (health == states.Length);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Dart"))
        {
            Damage(1);
        }
    }
}
