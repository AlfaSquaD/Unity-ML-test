using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuckScript : MonoBehaviour
{
    [SerializeField] GameObject P1;
    [SerializeField] GameObject P2;
    [SerializeField] Collider2D P1PlayArea;
    [SerializeField] Collider2D P2PlayArea;
    [SerializeField] Collider2D P1Goal;
    [SerializeField] Collider2D P2Goal;
    [SerializeField] Collider2D barrier;
    private PlayerAgent p1Agent;
    private PlayerAgent p2Agent;
    public Collider2D border;
    Vector2 pos;
    Rigidbody2D rb;
    // private ve readonly anahtar kelimeleri eklendi.
    private readonly List<Collision2D> collisions = new List<Collision2D>();
    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        p1Agent = P1.GetComponent<PlayerAgent>();
        p2Agent = P2.GetComponent<PlayerAgent>();
        Physics2D.IgnoreCollision(barrier, GetComponent<CircleCollider2D>());
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.Equals(border))
        {
            pos = transform.localPosition;
            pos.x = Mathf.Clamp(pos.x, border.bounds.min.x, border.bounds.max.x);
            pos.y = Mathf.Clamp(pos.y, border.bounds.min.y, border.bounds.max.y);
            rb.MovePosition(pos);
            if(collisions.Count != 0)
                rb.velocity = Vector2.Reflect(rb.velocity, collisions[0].GetContact(0).normal);
            else rb.velocity = Vector2.Reflect(rb.velocity, Vector2.up);        
        }
        if (collision.Equals(P1PlayArea))
        {
            p1Agent.AddReward(0.000005f);
        }
        else if (collision.Equals(P2PlayArea))
        {
            p2Agent.AddReward(0.000005f);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        collisions.Add(collision);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        collisions.Remove(collision);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.Equals(P1Goal))
        {
            p2Agent.goalReward();
        }
        else if (collision.Equals(P2Goal))
        {
            p1Agent.goalReward();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.Equals(P1PlayArea))
        {
            p1Agent.halfAreaPunisment();
        }
        else if (collision.Equals(P2PlayArea))
        {
            p2Agent.halfAreaPunisment();
        }
    }

    public void setRandomPos()
    {
        transform.position = RandomPointInBounds(border.bounds, 5);
    }

    public static Vector2 RandomPointInBounds(Bounds bounds, float offset)
    {
        return new Vector3(
            Random.Range(bounds.min.x + offset, bounds.max.x - offset),
            Random.Range(bounds.min.y + offset, bounds.max.y - offset)
        );
    }
}
