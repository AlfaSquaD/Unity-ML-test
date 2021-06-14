using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    bool wasJustClicked = true;
    bool canMove;
    Vector2 playerSize;
    public Collider2D border;
    public float maxSpeed;
    Rigidbody2D rb;
    // Use this for initialization
    void Start()
    {
        playerSize = GetComponent<SpriteRenderer>().bounds.extents;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.x = Mathf.Clamp(mousePos.x, border.bounds.min.x, border.bounds.max.x);
        mousePos.y = Mathf.Clamp(mousePos.y, border.bounds.min.y, border.bounds.max.y);
        if (Input.GetMouseButton(0))
        {

            if (wasJustClicked)
            {
                wasJustClicked = false;

                if ((mousePos.x >= transform.position.x && mousePos.x < transform.position.x + playerSize.x ||
                mousePos.x <= transform.position.x && mousePos.x > transform.position.x - playerSize.x) &&
                (mousePos.y >= transform.position.y && mousePos.y < transform.position.y + playerSize.y ||
                mousePos.y <= transform.position.y && mousePos.y > transform.position.y - playerSize.y))
                {
                    canMove = true;
                }
                else
                {
                    canMove = false;
                }
            }

            if (canMove)
            {
                Vector2 pos = transform.position;
                rb.MovePosition(pos + ((mousePos - pos) * Time.deltaTime * maxSpeed));
                pos = transform.position;
                if(pos.x > border.bounds.max.x || pos.x < border.bounds.min.x || pos.y > border.bounds.max.y || pos.y < border.bounds.min.y)
                {
                    pos.x = Mathf.Clamp(pos.x, border.bounds.min.x, border.bounds.max.x);
                    pos.y = Mathf.Clamp(pos.y, border.bounds.min.y, border.bounds.max.y);
                    rb.transform.position = pos;
                }
                
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
