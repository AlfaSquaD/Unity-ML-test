using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuckScript : MonoBehaviour
{
    public Collider2D border;
    Vector2 pos;
    Rigidbody2D rb;
    // private ve readonly anahtar kelimeleri eklendi.
    private readonly List<Collision2D> collisions = new List<Collision2D>();
    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.Equals(border))
        {
            // Burada bir zamanlar if vardi.
            pos = transform.position;
            pos.x = Mathf.Clamp(pos.x, border.bounds.min.x, border.bounds.max.x);
            pos.y = Mathf.Clamp(pos.y, border.bounds.min.y, border.bounds.max.y);
            rb.MovePosition(pos);
            if(collisions.Count != 0)
                rb.velocity = Vector2.Reflect(rb.velocity, collisions[0].GetContact(0).normal);
            else rb.velocity = Vector2.Reflect(rb.velocity, Vector2.up);
            
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
}
