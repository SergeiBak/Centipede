using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomRepairAnim : MonoBehaviour
{
    private SpriteRenderer sr;
    private Sprite[][] sprites;
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

    public void PlayRepairAnimation(float frameTime)
    {
        StartCoroutine(RepairAnimation(frameTime));
    }

    private IEnumerator RepairAnimation(float frameTime)
    {
        int colorIndex = GameManager.Instance.currentIndex;

        sr.sprite = sprites[colorIndex][0];
        sr.enabled = true;

        for (int i = 0; i < sprites[colorIndex].Length; i++)
        {
            sr.sprite = sprites[colorIndex][i];

            yield return new WaitForSeconds(frameTime);
        }

        sr.enabled = false;
    }
}
