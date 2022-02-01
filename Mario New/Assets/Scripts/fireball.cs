using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fireball : MonoBehaviour
{

    public static fireball instance;
    public int speed;
    public int vSpeed;
    public float upTime;
    public float timer = 0;
    public bool up = false;
    public LayerMask groundLayer;
    public bool isRight = true;
    public Transform isGroundedChecker; 
    public float checkGroundRadius;
    bool isGrounded = false;

    public void setFireballDir(bool right)
    {
        if (right)
        {
            isRight = true;
        }
        else
        {
            isRight = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        checkWalls();
        checkGround();
        Vector3 pos = transform.localPosition;
        
        if (isRight)
        {
          pos.x += speed * Time.deltaTime;
        }
        else
        {
            pos.x -= speed * Time.deltaTime;
        }

        if (up)
        {
         //timer for up then send it down
          if (timer < upTime)
          {
              pos.y += vSpeed * Time.deltaTime;
              timer += Time.deltaTime;
          }
          else
          {
              up = false;
          }
        }
        else
        {
            pos.y -= vSpeed * Time.deltaTime;
        }
        transform.localPosition = pos;   
    }

    void checkWalls()
    {
        if (isRight)
        {
             RaycastHit2D rightCast = Physics2D.Raycast(transform.position, Vector2.right, 0.25f, groundLayer);
             if (!isGrounded && rightCast.collider != null)
             {
                 Destroy(this.gameObject);
             }
        }
        
        if (!isRight)
        {
            RaycastHit2D leftCast = Physics2D.Raycast(transform.position, Vector2.left, 0.25f, groundLayer);
            if (!isGrounded && leftCast.collider != null)
             {
                 Destroy(this.gameObject);
             }
        }
    }

    void checkGround()
    {
                Collider2D col = Physics2D.OverlapCircle(isGroundedChecker.position, checkGroundRadius, groundLayer);

                 if (col != null) { 
                  isGrounded = true;
                  up = true;
                  timer = 0;
                  } else { 
                  isGrounded = false;
                  }
    }


}
