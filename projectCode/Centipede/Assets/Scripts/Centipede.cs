using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Centipede : MonoBehaviour
{
    [SerializeField]
    private int size = 12;
    [SerializeField]
    private CentipedeSegment segmentPrefab;
    [SerializeField]
    private Sprite headSprite;
    [SerializeField]
    private Sprite bodySprite;

    private List<CentipedeSegment> segments = new List<CentipedeSegment>();

    private void Start()
    {
        Respawn();
    }

    private void Respawn()
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
            segments.Add(segment);
        }
    }

    private Vector2 GridPosition(Vector2 position) // make sure postion is aligned to grid
    {
        position.x = Mathf.Round(position.x);
        position.y = Mathf.Round(position.y);

        return position;
    }
}
