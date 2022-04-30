using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CentipedeSegment : MonoBehaviour
{
    public SpriteRenderer sr { get; private set; }
    public Centipede centipede { get; set; }
    public CentipedeSegment ahead { get; set; }
    public CentipedeSegment behind { get; set; }
    public bool isHead => ahead == null;

    private Vector2 direction = Vector2.left + Vector2.down;
    private Vector2 targetPosition;

    [SerializeField]
    private float animationTime = 0.25f;
    public int animationFrame { get; private set; }
    [SerializeField]
    private bool loop = true;
    [HideInInspector]
    public bool isInfected = false;

    private Sprite[][] headSprites;
    [Header("Head Sprites")]
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

    private Sprite[][] bodySprites;
    [Header("Body Sprites")]
    [SerializeField]
    private Sprite[] bodyColor1Sprites;
    [SerializeField]
    private Sprite[] bodyColor2Sprites;
    [SerializeField]
    private Sprite[] bodyColor3Sprites;
    [SerializeField]
    private Sprite[] bodyColor4Sprites;
    [SerializeField]
    private Sprite[] bodyColor5Sprites;
    [SerializeField]
    private Sprite[] bodyColor6Sprites;
    [SerializeField]
    private Sprite[] bodyColor7Sprites;
    [SerializeField]
    private Sprite[] bodyColor8Sprites;
    [SerializeField]
    private Sprite[] bodyColor9Sprites;
    [SerializeField]
    private Sprite[] bodyColor10Sprites;
    [SerializeField]
    private Sprite[] bodyColor11Sprites;
    [SerializeField]
    private Sprite[] bodyColor12Sprites;
    [SerializeField]
    private Sprite[] bodyColor13Sprites;
    [SerializeField]
    private Sprite[] bodyColor14Sprites;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();

        headSprites = new Sprite[14][];
        headSprites[0] = color1Sprites;
        headSprites[1] = color2Sprites;
        headSprites[2] = color3Sprites;
        headSprites[3] = color4Sprites;
        headSprites[4] = color5Sprites;
        headSprites[5] = color6Sprites;
        headSprites[6] = color7Sprites;
        headSprites[7] = color8Sprites;
        headSprites[8] = color9Sprites;
        headSprites[9] = color10Sprites;
        headSprites[10] = color11Sprites;
        headSprites[11] = color12Sprites;
        headSprites[12] = color13Sprites;
        headSprites[13] = color14Sprites;

        bodySprites = new Sprite[14][];
        bodySprites[0] = bodyColor1Sprites;
        bodySprites[1] = bodyColor2Sprites;
        bodySprites[2] = bodyColor3Sprites;
        bodySprites[3] = bodyColor4Sprites;
        bodySprites[4] = bodyColor5Sprites;
        bodySprites[5] = bodyColor6Sprites;
        bodySprites[6] = bodyColor7Sprites;
        bodySprites[7] = bodyColor8Sprites;
        bodySprites[8] = bodyColor9Sprites;
        bodySprites[9] = bodyColor10Sprites;
        bodySprites[10] = bodyColor11Sprites;
        bodySprites[11] = bodyColor12Sprites;
        bodySprites[12] = bodyColor13Sprites;
        bodySprites[13] = bodyColor14Sprites;

        targetPosition = transform.position;
    }

    private void Start()
    {
        InvokeRepeating(nameof(Advance), animationTime, animationTime);
    }

    private void Update()
    {
        if (isHead && Vector2.Distance(transform.position, targetPosition) < 0.1f) // if head very close to target position, calculate next target position
        {
            UpdateHeadSegment();
        }

        Vector2 currentPosition = transform.position;
        float speed = centipede.speed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(currentPosition, targetPosition, speed);

        Vector2 movementDirection = (targetPosition - currentPosition).normalized;
        float angle = Mathf.Atan2(-movementDirection.y, -movementDirection.x);
        transform.rotation = Quaternion.AngleAxis(angle * Mathf.Rad2Deg, Vector3.forward);
    }

    public void UpdateHeadSegment()
    {
        Vector2 gridPosition = GridPosition(transform.position);

        targetPosition = gridPosition;
        targetPosition.x += direction.x;

        if (isInfected)
        {
            direction.x = -direction.x;

            targetPosition.x = gridPosition.x;
            if (direction.x == 1f)
            {
                targetPosition.x += 1f;
            }
            else
            {
                targetPosition.x -= 1f;
            }

            targetPosition.y = gridPosition.y + direction.y;

            Bounds homeBounds = centipede.homeArea.bounds;
            if ((direction.y == 1f && targetPosition.y > homeBounds.max.y) || (direction.y == -1f && targetPosition.y < homeBounds.min.y)) // reverse direction
            {
                isInfected = false;
                direction.y = -direction.y;
                targetPosition.y = gridPosition.y + direction.y;
            }
        }
        else if (Physics2D.OverlapBox(targetPosition, Vector2.zero, 0f, centipede.collisionMask))
        {
            Collider2D col = Physics2D.OverlapBox(targetPosition, Vector2.zero, 0f, centipede.collisionMask);
            Mushroom mushroom = col.gameObject.GetComponent<Mushroom>();

            if (mushroom != null)
            {
                if (mushroom.infected)
                {
                    isInfected = true;
                    direction.y = -1f;
                }
            }

            direction.x = -direction.x;

            targetPosition.x = gridPosition.x;
            targetPosition.y = gridPosition.y + direction.y;

            Bounds homeBounds = centipede.homeArea.bounds;
            if ((direction.y == 1f && targetPosition.y > homeBounds.max.y) || (direction.y == -1f && targetPosition.y < homeBounds.min.y)) // reverse direction
            {
                direction.y = -direction.y;
                targetPosition.y = gridPosition.y + direction.y;
            }
        }

        if (behind != null)
        {
            if (isInfected)
            {
                behind.isInfected = true;
            }
            behind.UpdateBodySegment();
        }
    }

    private void UpdateBodySegment()
    {
        targetPosition = GridPosition(ahead.transform.position);
        direction = ahead.direction;

        if (behind != null)
        {
            behind.UpdateBodySegment();
        }
    }

    private Vector2 GridPosition(Vector2 position) // make sure postion is aligned to grid
    {
        position.x = Mathf.Round(position.x);
        position.y = Mathf.Round(position.y);

        return position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.enabled && collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            collision.collider.enabled = false;
            GameManager.Instance.ResetRound();
            return;
        }

        if (collision.collider.enabled && collision.gameObject.layer == LayerMask.NameToLayer("Dart"))
        {
            collision.collider.enabled = false;
            centipede.Remove(this);
        }
    }

    private void Advance() // increment frame
    {
        if (!sr.enabled) // if spriterenderer not enabled, return
        {
            return;
        }

        animationFrame++; // increment frame
        int colorIndex = GameManager.Instance.currentIndex;

        if (isHead)
        {
            if (animationFrame >= headSprites[colorIndex].Length && loop) // if frame out of range go back to 0
            {
                animationFrame = 0;
            }

            if (animationFrame >= 0 && animationFrame < headSprites[colorIndex].Length) // if frame valid, update sprite
            {
                sr.sprite = headSprites[colorIndex][animationFrame];
            }
        }
        else
        {
            if (animationFrame >= bodySprites[colorIndex].Length && loop) // if frame out of range go back to 0
            {
                animationFrame = 0;
            }

            if (animationFrame >= 0 && animationFrame < bodySprites[colorIndex].Length) // if frame valid, update sprite
            {
                sr.sprite = bodySprites[colorIndex][animationFrame];
            }
        }  
    }

    public void Restart(int frame = 0) // starts animation over from certain frame frame
    {
        animationFrame = (frame - 1);

        Advance();
    }

    public int SegmentAnimationLength()
    {
        return bodySprites[0].Length;
    }
}
