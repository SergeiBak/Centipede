using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderAnimation : MonoBehaviour
{
    private SpriteRenderer sr;
    [SerializeField]
    private float animationTime = 0.25f;
    public int animationFrame { get; private set; }
    [SerializeField]
    private bool loop = true;

    private Sprite[][] sprites;
    [Header("Spider Sprites")]
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

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();

        sprites = new Sprite[14][];
        sprites[0] = color1Sprites;
        sprites[1] = color2Sprites;
        sprites[2] = color3Sprites;
        sprites[3] = color4Sprites;
        sprites[4] = color5Sprites;
        sprites[5] = color6Sprites;
        sprites[6] = color7Sprites;
        sprites[7] = color8Sprites;
        sprites[8] = color9Sprites;
        sprites[9] = color10Sprites;
        sprites[10] = color11Sprites;
        sprites[11] = color12Sprites;
        sprites[12] = color13Sprites;
        sprites[13] = color14Sprites;
    }

    private void Start()
    {
        InvokeRepeating(nameof(Advance), animationTime, animationTime);
    }

    private void Advance() // increment frame
    {
        if (!sr.enabled) // if spriterenderer not enabled, return
        {
            return;
        }

        animationFrame++; // increment frame
        int colorIndex = GameManager.Instance.currentIndex;

        if (animationFrame >= sprites[colorIndex].Length && loop) // if frame out of range go back to 0
        {
            animationFrame = 0;
        }

        if (animationFrame >= 0 && animationFrame < sprites[colorIndex].Length) // if frame valid, update sprite
        {
            sr.sprite = sprites[colorIndex][animationFrame];
        }
    }

    public void Restart(int frame = 0) // starts animation over from certain frame frame
    {
        animationFrame = (frame - 1);

        Advance();
    }
}
