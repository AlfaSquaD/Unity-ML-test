using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Collider2D ownGoal;
    public Collider2D enemyGoal;
    public Sprite sprite;
    private SpriteRenderer sr;
    // Start is called before the first frame update
    void Start()
    {
        sr = gameObject.GetComponent<SpriteRenderer>();
        sr.sprite = sprite;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
