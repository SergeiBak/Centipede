using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomField : MonoBehaviour
{
    private BoxCollider2D area;
    [SerializeField]
    private Mushroom prefab;
    [SerializeField]
    private int amount = 50;

    private void Awake()
    {
        area = GetComponent<BoxCollider2D>();
    }

    public void Generate()
    {
        Bounds bounds = area.bounds;
        
        for (int i = 0; i < amount; i++)
        {
            Vector2 position = Vector2.zero;

            position.x = Mathf.Round(Random.Range(bounds.min.x, bounds.max.x));
            position.y = Mathf.Round(Random.Range(bounds.min.y, bounds.max.y));

            Instantiate(prefab, position, Quaternion.identity, transform);
        }
    }

    public void Clear()
    {
        Mushroom[] mushrooms = FindObjectsOfType<Mushroom>();

        foreach (Mushroom mushroom in mushrooms)
        {
            Destroy(mushroom.gameObject);
        }
    }

    public void Heal()
    {
        StartCoroutine(HealAnimation());
    }

    private IEnumerator HealAnimation()
    {
        Mushroom[] mushrooms = FindObjectsOfType<Mushroom>();
        System.Array.Sort(mushrooms, ComparePos);

        foreach (Mushroom mushroom in mushrooms)
        {
            if (!mushroom.IsFullHealth() || mushroom.infected)
            {
                mushroom.Heal();
                yield return new WaitForSeconds(0.25f);
            }
        }

        if (!GameManager.Instance.NoLivesLeft())
        {
            GameManager.Instance.RespawnPlayer();
        }
    }

    private static int ComparePos(Mushroom room1, Mushroom room2) // sort mushrooms based on screen position from left to right
    {
        if(room1.gameObject.transform.position.x != room2.gameObject.transform.position.x)
        {
            return room1.gameObject.transform.position.x.CompareTo(room2.gameObject.transform.position.x);
        }
        return room1.gameObject.transform.position.y.CompareTo(room2.gameObject.transform.position.y);
    }

    public void UpdateMushroomColors()
    {
        Mushroom[] mushrooms = FindObjectsOfType<Mushroom>();

        foreach (Mushroom mushroom in mushrooms)
        {
            mushroom.RenderMushroom();
        }
    }
}
