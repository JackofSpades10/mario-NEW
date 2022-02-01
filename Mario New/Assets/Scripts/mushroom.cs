using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class mushroom : MonoBehaviour
{
    public float speed;
    public LayerMask maskey;
    private bool movingRight = true;
    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

        RaycastHit2D rightCast = Physics2D.Raycast(transform.position, Vector2.right, 1.0f, maskey);
        RaycastHit2D leftCast = Physics2D.Raycast(transform.position, Vector2.left, 1.0f, maskey);
        if (movingRight && rightCast.collider != null)
        {
            movingRight = !movingRight;
        }
        if (!movingRight && leftCast.collider != null)
        {
            movingRight = !movingRight;
        }


        if (movingRight)
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2((-1*speed), rb.velocity.y);
        }
    }
}
