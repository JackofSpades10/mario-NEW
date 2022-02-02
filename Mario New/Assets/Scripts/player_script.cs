using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_script : MonoBehaviour
{
    Rigidbody2D rb;
    public float speed;
    public float jumpForce;
    public float bounceVelocity;
    public float gravity;
    public Vector2 velocity;
    bool isGrounded = false; 
    public Transform isGroundedChecker; 
    public float checkGroundRadius; 
    public LayerMask groundLayer;
    public LayerMask floorMask;
    public LayerMask enemyMask;
    public float fallMultiplier = 2.5f; 
    public float lowJumpMultiplier = 2f;
    public float bounceTime = 0f;
    public float bounceTimer = 1f;
    private bool bounce = false;
    public Sprite bigPlayer;
    public Sprite smallPlayer;
    public int health = 1;
    public float iTimer = 3;
    public float iTime = 0;

    public bool invincible = false;
    private bool canInput = true;
    private bool gotPoints = false;
    float castleTimer = 1f;
    float castleTime = 0f;
    float blinkTimer = 0.2f;
    float blinkTime = 0;
    bool dieUp = true;
    bool dieDown = false;
    bool ogPosSet = false;
    public Vector2 ogLoc;

    public enum PlayerState {
        jumping,
        idle,
        walking,
        bouncing,
        sliding
    }

    private PlayerState playerState = PlayerState.idle;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); 
    }

    // Update is called once per frame
    void Update()
    {      
        if (Time.timeScale != 0)
        {
            ogLoc = transform.localPosition;
        }
        Vector3 pos = transform.localPosition;
        Vector3 scale = transform.localScale;
        move();
        Jump();
        BetterJump();
        CheckIfGrounded();
        CheckFloorRays(pos);
        CheckCielingRays(pos);
        if (invincible)
        {
            if (iTime < iTimer)
            {
                if (blinkTime < blinkTimer)
                {
                    blinkTime += Time.deltaTime;
                }
                else
                {
                    if (gameObject.GetComponent<SpriteRenderer>().enabled)
                    {
                        gameObject.GetComponent<SpriteRenderer> ().enabled = false;
                        blinkTime = 0;
                    }
                    else
                    {   
                        gameObject.GetComponent<SpriteRenderer> ().enabled = true;
                        blinkTime = 0;
                    }
                }


                iTime += Time.deltaTime;
            }
            else
            {
                gameObject.GetComponent<SpriteRenderer> ().enabled = true;
                invincible = false;
            }
        }

        if (playerState == PlayerState.sliding)
        {
            Vector2 place = transform.localPosition;
                 //set timer to sit, then slide down, walk into castle
            if (castleTime <= castleTimer)
            {   
                castleTime += Time.deltaTime;
                
            }
            else // slide down and walk into castle
            {
                if (health == 1)
                {
                if (transform.position.y > 1)
                {
                    place.y -= 2f * Time.deltaTime;
                    
                }// 50 is the castle location
                else if (transform.position.x < 204)
                {
                    place.x += 2f * Time.deltaTime;
                }
                }

                if (health >= 2)
                {
                    if (transform.position.y > 3)
                {
                    place.y -= 2f * Time.deltaTime;
                    
                }// 50 is the castle location
                else if (transform.position.x < 204)
                {
                    place.x += 2f * Time.deltaTime;
                }
                }

            }
            transform.localPosition = place;
        }

        if (health == 3) {
            // change the sprite here
            gameObject.GetComponent<SpriteRenderer>().color = Color.blue;
            gameObject.GetComponent<SpriteRenderer>().sprite = bigPlayer;
            gameObject.GetComponent<BoxCollider2D>().size = new Vector2(1, 2);
        }
        if (health == 2) {
            gameObject.GetComponent<SpriteRenderer>().sprite = bigPlayer;
            gameObject.GetComponent<BoxCollider2D>().size = new Vector2(1, 2);
        }
        if (health == 1)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = smallPlayer;
            gameObject.GetComponent<BoxCollider2D>().size = new Vector2(1, 1);
        }
        if (health == 0)
        {
            Time.timeScale = 0;

            gameObject.GetComponent<SpriteRenderer>().color = Color.black;
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            Vector2 loc = transform.localPosition;
            if(dieUp)
            {
                if (transform.localPosition.y < ogLoc.y+3f)
                {
                    loc.y += 7f * Time.unscaledDeltaTime;
                }
                if (transform.localPosition.y < ogLoc.y+4.5f && transform.localPosition.y > ogLoc.y+3f)
                {
                    loc.y += 3.75f * Time.unscaledDeltaTime;
                }
                  if (transform.localPosition.y < ogLoc.y+5f && transform.localPosition.y > ogLoc.y+4.5f)
                {
                    loc.y += 2.3f * Time.unscaledDeltaTime;
                }
                if (transform.localPosition.y >= ogLoc.y+5f)
                {
                    dieDown = true;
                    dieUp = false;
                }              
            }
            if (dieDown)
            {
                if (transform.localPosition.y > -1f)
                {
                    loc.y -= 8 * Time.unscaledDeltaTime;
                }
                if (transform.localPosition.y <= -1f)
                {
                    dieDown = false;
                    Time.timeScale = 1;
                    score_manager.instance.changeLives(-1);
                }              
            }
            transform.localPosition = loc;
        }

        if (health == 3 && (Input.GetKeyDown(KeyCode.LeftShift)) && canInput)
        {
            if (GameObject.FindGameObjectsWithTag("fireball").Length < 2)
            {
                GameObject newFireball = (GameObject)Instantiate (Resources.Load("Prefabs/fireball", typeof(GameObject)));
                newFireball.transform.localPosition = new Vector2(transform.position.x + 0.25f, transform.position.y);
                if (transform.localScale.x == 1)
                {
                    newFireball.GetComponent<fireball>().isRight = true;
                }
                else
                {
                    newFireball.GetComponent<fireball>().isRight = false;
                }
                
            }
            
        }

    //bouncing
    if (bounce && playerState != PlayerState.bouncing)
    {
        playerState = PlayerState.bouncing;

        velocity = new Vector2(rb.velocity.x, bounceVelocity);
    }

    if (playerState == PlayerState.bouncing)
    {
        pos.y += velocity.y * Time.deltaTime;
        velocity.y -= gravity * Time.deltaTime;
       
        rb.velocity = new Vector2(rb.velocity.x, velocity.y);
    }

    Vector3 CheckFloorRays (Vector3 pos)
    {
        Vector2 originLeft = new Vector2(pos.x - 0.5f + 0.2f, pos.y - 1f);
        Vector2 originMiddle = new Vector2(pos.x, pos.y-1f);
        Vector2 originRight = new Vector2(pos.x + 0.5f - 0.2f, pos.y - 1f);

        RaycastHit2D floorLeft = Physics2D.Raycast(originLeft, Vector2.down, rb.velocity.y * Time.deltaTime, enemyMask);
        RaycastHit2D floorMiddle = Physics2D.Raycast(originMiddle, Vector2.down, rb.velocity.y * Time.deltaTime, enemyMask);
        RaycastHit2D floorRight = Physics2D.Raycast(originRight, Vector2.down, rb.velocity.y * Time.deltaTime, enemyMask);

        if (floorLeft.collider != null || floorMiddle.collider != null || floorRight.collider != null)
        {
            RaycastHit2D hitRay = floorRight;

            if (floorLeft) {
                hitRay = floorLeft;
            } else if (floorMiddle) {
                hitRay = floorMiddle;
            } else if (floorRight) {
                hitRay = floorRight;
            }

            if (hitRay.collider.tag == "Enemy")
            {
                bounce = true;
                hitRay.collider.GetComponent<enemyAI>().Crush();
            }
            if (hitRay.collider.tag == "koopa")
            {
                bounce = true;
                hitRay.collider.GetComponent<koopaAI>().Crush();
            }
        }
        return pos;
    }

    Vector3 CheckCielingRays (Vector3 pos)
    {
        Vector2 originLeft = new Vector2(pos.x - 0.5f + 0.2f, pos.y + 1f);
        Vector2 originMiddle = new Vector2(pos.x, pos.y + 1f);
        Vector2 originRight = new Vector2(pos.x + 0.5f - 0.2f, pos.y + 1f);

        RaycastHit2D cielLeft = Physics2D.Raycast(originLeft, Vector2.down, rb.velocity.y * Time.deltaTime, floorMask);
        RaycastHit2D cielMiddle = Physics2D.Raycast(originMiddle, Vector2.down, rb.velocity.y * Time.deltaTime, floorMask);
        RaycastHit2D cielRight = Physics2D.Raycast(originRight, Vector2.down, rb.velocity.y * Time.deltaTime, floorMask);

        if (cielLeft.collider != null || cielMiddle.collider != null || cielRight.collider != null)
        {
            RaycastHit2D hitRay = cielLeft;

            if (cielLeft) {
                hitRay = cielLeft;
            } else if (cielMiddle) {
                hitRay = cielMiddle;
            } else if (cielRight) {
                hitRay = cielRight;
            }

            if (hitRay.collider.tag == "QuestionBlock")
            {
                hitRay.collider.GetComponent<QuestionBlock>().QuestionBlockBounce();
            }
            if (hitRay.collider.tag == "bricks")
            {
                hitRay.collider.GetComponent<bricks>().BrickBounce();
            }
        }
        return pos;
    }
}
    // checks if the player is on the ground layer
    void CheckIfGrounded() { 

        if (health == 1)
        {
             Collider2D col = Physics2D.OverlapCircle(isGroundedChecker.position, checkGroundRadius, groundLayer);

                 if (col != null) { 
                  isGrounded = true;
                  } else { 
                  isGrounded = false; 
            }
        }

        if (health >= 2)
        {
            isGroundedChecker.localPosition = new Vector3(0,-1,0);
            Vector3 newSpot = isGroundedChecker.position;
            Collider2D newCol = Physics2D.OverlapCircle(newSpot, checkGroundRadius, groundLayer);

               if (newCol != null) { 
                  isGrounded = true;
                  
                  } else { 
                  isGrounded = false; 
                  }
        }
    }

    void Jump() 
    { 
    if (Input.GetKeyDown(KeyCode.Space) && isGrounded && canInput) { 
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        playerState = PlayerState.jumping;
        }
    }

    void BetterJump() {
    if (rb.velocity.y < 0) {
        bounce = false;
        rb.velocity += Vector2.up * Physics2D.gravity * (fallMultiplier - 1) * Time.deltaTime;
    } else if (rb.velocity.y > 0 && !Input.GetKey(KeyCode.Space)) {
        rb.velocity += Vector2.up * Physics2D.gravity * (lowJumpMultiplier - 1) * Time.deltaTime;
    }   
}

    void move()
    {
        if (canInput)
        {
           Vector3 scale = transform.localScale;

      float x = Input.GetAxisRaw("Horizontal"); 
     float moveBy = x * speed;
        if (moveBy > 0)
        {
            scale.x = 1;
        }
        if (moveBy < 0)
        {
            scale.x = -1;
        }
        transform.localScale = scale;
        rb.velocity = new Vector2(moveBy, rb.velocity.y); 
        if (x != 0)
        {
            if (isGrounded)
            {
                playerState = PlayerState.walking;
            }
        } else {
            if (isGrounded)
            {
                playerState = PlayerState.idle;
            }
        }
     }
    }

    //mushroom collection
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            if (!invincible)
            {
                 health--;
                 invincible = true;
            }
           
        }

        if (other.gameObject.CompareTag("mushroom"))
        {
            health = 2;
            score_manager.instance.ChangeScore(1000);
            score_manager.instance.createPoint(transform.localPosition.x + 0.5f, transform.localPosition.y + 0.5f, 1000);
            // change the sprite to big mario here
            Destroy(other.gameObject);
        }
        if (other.gameObject.CompareTag("fireflower"))
        {
            score_manager.instance.ChangeScore(1000);
            score_manager.instance.createPoint(transform.localPosition.x + 0.5f, transform.localPosition.y + 0.5f, 1000);
            health = 3;
            Destroy(other.gameObject);
        }

    }    

    // collecting coins
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("coin"))
        {   
            Destroy(other.gameObject);
        }

        if (other.gameObject.CompareTag("flag"))
        {
            
            // change sprite to sliding
            gameObject.GetComponent<SpriteRenderer>().color = Color.green;
            gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
            gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
            rb.velocity = new Vector2(0, 0);
            if (canInput)
            {
              
                canInput = false;
            }
            Debug.Log(castleTime.ToString());
            playerState = PlayerState.sliding;
           
            
            GameObject.Find("flag").GetComponent<flag>().dropTheFlag();

                if (!gotPoints && transform.position.y <= 2)
            {
                score_manager.instance.createPoint(transform.localPosition.x + 0.5f, transform.localPosition.y + 0.5f, 100);
                score_manager.instance.ChangeScore(100);
                gotPoints = true;
            }
        
            if (!gotPoints && transform.position.y > 2 && transform.position.y <= 4)
            {
                score_manager.instance.createPoint(transform.localPosition.x + 0.5f, transform.localPosition.y + 0.5f, 400);
                score_manager.instance.ChangeScore(400);
                gotPoints = true;
            }

            if (!gotPoints && transform.position.y > 4 && transform.position.y <= 6)
            {
                score_manager.instance.createPoint(transform.localPosition.x + 0.5f, transform.localPosition.y + 0.5f, 800);
                score_manager.instance.ChangeScore(800);
                gotPoints = true;
            }

            if (!gotPoints && transform.position.y > 6 && transform.position.y <= 8)
            {
                score_manager.instance.createPoint(transform.localPosition.x + 0.5f, transform.localPosition.y + 0.5f, 2000);
                score_manager.instance.ChangeScore(2000);
                gotPoints = true;
            }

            if (!gotPoints && transform.position.y > 8 && transform.position.y <= 10)
            {
                score_manager.instance.createPoint(transform.localPosition.x + 0.5f, transform.localPosition.y + 0.5f, 4000);
                score_manager.instance.ChangeScore(4000);
                gotPoints = true;
            }

            if (!gotPoints && transform.position.y > 10 && transform.position.y <= 12)
            {
                score_manager.instance.createPoint(transform.localPosition.x + 0.5f, transform.localPosition.y + 0.5f, 5000);
                score_manager.instance.ChangeScore(5000);
                gotPoints = true;
            }

        }
    }
}