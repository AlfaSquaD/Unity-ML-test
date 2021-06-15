using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    bool wasJustClicked = true;
    bool canMove;
    public Collider2D playArea;
    public float maxSpeed;
    Rigidbody2D rb;
    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
        if (Input.GetMouseButton(0))
        {
            if (wasJustClicked)
            {
                wasJustClicked = false;
                canMove = playArea.Equals(hit.collider);
            }

            if (canMove)
            {
                Vector2 pos = transform.position;
                Vector2 nPos = pos + ((mousePos - pos) * Time.deltaTime * maxSpeed);
                if (nPos.x > playArea.bounds.max.x || nPos.x < playArea.bounds.min.x || nPos.y > playArea.bounds.max.y || nPos.y < playArea.bounds.min.y)
                {
                    nPos.x = Mathf.Clamp(nPos.x, playArea.bounds.min.x, playArea.bounds.max.x);
                    nPos.y = Mathf.Clamp(nPos.y, playArea.bounds.min.y, playArea.bounds.max.y);
                }

                rb.MovePosition(nPos);

            }
        }
        else
        {
            wasJustClicked = true;
        }

    }
    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    Vector2 pos = transform.position;
    //    if (collision.Equals(border))
    //    {
    //        pos = transform.position;
    //        if (pos.x > border.bounds.max.x || pos.x < border.bounds.min.x || pos.y > border.bounds.max.y || pos.y < border.bounds.min.y)
    //        {
    //            pos.x = Mathf.Clamp(pos.x, border.bounds.min.x, border.bounds.max.x);
    //            pos.y = Mathf.Clamp(pos.y, border.bounds.min.y, border.bounds.max.y);
    //            rb.transform.position = pos;
    //        }
    //    }
    //}
}
