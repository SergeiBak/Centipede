using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom : MonoBehaviour
{
    public int points = 1;
    [SerializeField]
    private int healPoints = 5;
    private int health;
    private SpriteRenderer sr;
    [SerializeField]
    MushroomRepairAnim repairAnimation;

    private Sprite[][] mushroomSprites;
    [Header("Mushroom Sprites")]
    [SerializeField]
    private Sprite[] color1Sprites;
    [SerializeField]
    private Sprite[] color2Sprites;
    [SerializeField]
    private Sprite[] color3Sprites;
    [SerializeField]
    private Sprite[] color4Sprites;
    [SerializeField]
    private Sprite[] color5Sprites;
    [SerializeField]
    private Sprite[] color6Sprites;
    [SerializeField]
    private Sprite[] color7Sprites;
    [SerializeField]
    private Sprite[] color8Sprites;
    [SerializeField]
    private Sprite[] color9Sprites;
    [SerializeField]
    private Sprite[] color10Sprites;
    [SerializeField]
    private Sprite[] color11Sprites;
    [SerializeField]
    private Sprite[] color12Sprites;
    [SerializeField]
    private Sprite[] color13Sprites;
    [SerializeField]
    private Sprite[] color14Sprites;

    private Sprite[][] infectedMushroomSprites;
    [Header("Infected Mushroom Sprites")]
    [SerializeField]
    private Sprite[] infectedColor1Sprites;
    [SerializeField]
    private Sprite[] infectedColor2Sprites;
    [SerializeField]
    private Sprite[] infectedColor3Sprites;
    [SerializeField]
    private Sprite[] infectedColor4Sprites;
    [SerializeField]
    private Sprite[] infectedColor5Sprites;
    [SerializeField]
    private Sprite[] infectedColor6Sprites;
    [SerializeField]
    private Sprite[] infectedColor7Sprites;
    [SerializeField]
    private Sprite[] infectedColor8Sprites;
    [SerializeField]
    private Sprite[] infectedColor9Sprites;
    [SerializeField]
    private Sprite[] infectedColor10Sprites;
    [SerializeField]
    private Sprite[] infectedColor11Sprites;
    [SerializeField]
    private Sprite[] infectedColor12Sprites;
    [SerializeField]
    private Sprite[] infectedColor13Sprites;
    [SerializeField]
    private Sprite[] infectedColor14Sprites;

    [HideInInspector]
    public bool infected = false;

private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();

        mushroomSprites = new Sprite[14][];
        mushroomSprites[0] = color1Sprites;
        mushroomSprites[1] = color2Sprites;
        mushroomSprites[2] = color3Sprites;
        mushroomSprites[3] = color4Sprites;
        mushroomSprites[4] = color5Sprites;
        mushroomSprites[5] = color6Sprites;
        mushroomSprites[6] = color7Sprites;
        mushroomSprites[7] = color8Sprites;
        mushroomSprites[8] = color9Sprites;
        mushroomSprites[9] = color10Sprites;
        mushroomSprites[10] = color11Sprites;
        mushroomSprites[11] = color12Sprites;
        mushroomSprites[12] = color13Sprites;
        mushroomSprites[13] = color14Sprites;

        infectedMushroomSprites = new Sprite[14][];
        infectedMushroomSprites[0] = infectedColor1Sprites;
        infectedMushroomSprites[1] = infectedColor2Sprites;
        infectedMushroomSprites[2] = infectedColor3Sprites;
        infectedMushroomSprites[3] = infectedColor4Sprites;
        infectedMushroomSprites[4] = infectedColor5Sprites;
        infectedMushroomSprites[5] = infectedColor6Sprites;
        infectedMushroomSprites[6] = infectedColor7Sprites;
        infectedMushroomSprites[7] = infectedColor8Sprites;
        infectedMushroomSprites[8] = infectedColor9Sprites;
        infectedMushroomSprites[9] = infectedColor10Sprites;
        infectedMushroomSprites[10] = infectedColor11Sprites;
        infectedMushroomSprites[11] = infectedColor12Sprites;
        infectedMushroomSprites[12] = infectedColor13Sprites;
        infectedMushroomSprites[13] = infectedColor14Sprites;

        health = mushroomSprites[0].Length;
    }

    private void Start()
    {
        RenderMushroom();
    }

    private void Damage(int amount)
    {
        health -= amount;

        if (health > 0)
        {
            RenderMushroom();
        } 
        else
        {
            Destroy(gameObject);
            GameManager.Instance.IncreaseScore(points);
        }
    }

    public void RenderMushroom() // renders mushroom based on state & color index
    {
        int colorIndex = GameManager.Instance.currentIndex;
        int healthState = mushroomSprites[0].Length - health;

        if (!sr || healthState >= mushroomSprites[0].Length || healthState < 0)
        {
            return;
        }

        if (!infected)
        {
            sr.sprite = mushroomSprites[colorIndex][healthState];
        }
        else
        {
            sr.sprite = infectedMushroomSprites[colorIndex][healthState];
        }
    }

    public void Infect()
    {
        if (infected)
        {
            return;
        }

        infected = true;
        RenderMushroom();
    }

    public void Heal()
    {
        infected = false;
        health = mushroomSprites[0].Length;
        RenderMushroom();
        repairAnimation.PlayRepairAnimation(0.1f);
        GameManager.Instance.IncreaseScore(healPoints);
    }

    public bool IsFullHealth()
    {
        return (health == mushroomSprites[0].Length);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Dart"))
        {
            Damage(1);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Spider"))
        {
            Damage(health);
        }
    }
}
