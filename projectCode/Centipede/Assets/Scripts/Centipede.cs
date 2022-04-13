using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Centipede : MonoBehaviour
{
    [SerializeField]
    private int size = 12;
    public float speed = 20f;

    public LayerMask collisionMask;
    public BoxCollider2D homeArea;

    [SerializeField]
    private Sprite headSprite;
    [SerializeField]
    private Sprite bodySprite;

    [SerializeField]
    private CentipedeSegment segmentPrefab;
    [SerializeField]
    private Mushroom mushroomPrefab;

    private List<CentipedeSegment> segments = new List<CentipedeSegment>();

    public void Respawn()
    {
        foreach (CentipedeSegment segment in segments)
        {
            Destroy(segment.gameObject);
        }

        segments.Clear();

        for (int i = 0; i < size; i++)
        {
            Vector2 position = GridPosition(transform.position) + (Vector2.right * i);
            CentipedeSegment segment = Instantiate(segmentPrefab, position, Quaternion.identity);
            segment.sr.sprite = i == 0 ? headSprite : bodySprite;
            segment.centipede = this;
            segments.Add(segment);
        }

        for (int i = 0; i < segments.Count; i++)
        {
            CentipedeSegment segment = segments[i];
            segment.ahead = GetSegmentAt(i - 1);
            segment.behind = GetSegmentAt(i + 1);
        }
    }

    public void Remove(CentipedeSegment segment)
    {
        Vector3 position = GridPosition(segment.transform.position);
        Instantiate(mushroomPrefab, position, Quaternion.identity);

        if (segment.ahead != null)
        {
            segment.ahead.behind = null;
        }

        if (segment.behind != null)
        {
            segment.behind.ahead = null;
            segment.behind.sr.sprite = headSprite;
            segment.behind.UpdateHeadSegment();
        }

        segments.Remove(segment);
        Destroy(segment.gameObject);
    }

    private CentipedeSegment GetSegmentAt(int index)
    {
        if (index >= 0 && index < segments.Count) // check if in bounds
        {
            return segments[index];
        }
        return null;
    }

    private Vector2 GridPosition(Vector2 position) // make sure postion is aligned to grid
    {
        position.x = Mathf.Round(position.x);
        position.y = Mathf.Round(position.y);

        return position;
    }
}
