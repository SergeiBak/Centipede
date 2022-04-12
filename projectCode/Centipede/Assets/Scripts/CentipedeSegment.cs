using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CentipedeSegment : MonoBehaviour
{
    public SpriteRenderer sr { get; private set; }

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }
}
