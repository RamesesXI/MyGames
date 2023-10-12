using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RewindController : MonoBehaviour
{
    [HideInInspector]
    public bool isRewinding;

    private List<Vector2> Movements = new List<Vector2>();
    private int MovementIndex = 0;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRend;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRend = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (!isRewinding)
        {
            Movements.Add(transform.position);
            MovementIndex++;
        }    
    }

    public IEnumerator Rewind()
    {
        isRewinding = true;

        rb.gravityScale = 0;
        rb.velocity = Vector2.zero;
        spriteRend.color = Color.red;

        int backPoint;
        int iterator = 0;

        if (MovementIndex > 300)
            backPoint = MovementIndex - 300;
        else
            backPoint = 1;

        while (MovementIndex >= backPoint)
        {
            MovementIndex--;
            transform.position = Movements[MovementIndex];
            Movements.RemoveAt(MovementIndex);
            iterator++;
            if (iterator >= 3)
            {
                yield return new WaitForFixedUpdate();
                iterator = 0;
            }
        }

        MovementIndex = 0;
        Movements.Clear();
        isRewinding = false;

        rb.gravityScale = 1;
        spriteRend.color = Color.white;     
    }
}
