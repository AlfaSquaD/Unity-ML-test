using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    bool wasJustClicked = true;
    bool canMove;
    public Collider2D playArea;
    public float maxSpeed;
    public Rigidbody2D rb;
    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetMouseButton(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            movePlayer(mousePos);
        }
        else wasJustClicked = true;
    }

    public void movePlayer(Vector2 mousePos)
    {
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
        if (wasJustClicked)
        {
            wasJustClicked = false;
            canMove = playArea.Equals(hit.collider);
        }

        if (canMove)
        {
            Vector2 pos = transform.localPosition;
            Vector2 nPos = pos + ((mousePos - pos) * Time.deltaTime * maxSpeed);
            if (nPos.x > playArea.bounds.max.x || nPos.x < playArea.bounds.min.x || nPos.y > playArea.bounds.max.y || nPos.y < playArea.bounds.min.y)
            {
                nPos.x = Mathf.Clamp(nPos.x, playArea.bounds.min.x, playArea.bounds.max.x);
                nPos.y = Mathf.Clamp(nPos.y, playArea.bounds.min.y, playArea.bounds.max.y);
            }

            rb.MovePosition(nPos);
        }   
    }
}
