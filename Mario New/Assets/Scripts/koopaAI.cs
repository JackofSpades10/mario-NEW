using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class koopaAI : MonoBehaviour
{

    public float gravity;
    public Vector2 velocity;
    public Vector2 shellVel;
    public bool isWalkingLeft = true;

    public LayerMask floorMask;
    public LayerMask wallMask;

    private bool grounded = false;
    public Sprite koopa;
    public Sprite shell;
    player_script player;

    private float standTimer = 0;
    public float timeBeforeUp = 3.0f;

    private enum KoopaState {
        walking,
        falling,
        shell,
        shellKicked,
        dead
    }

    private KoopaState state = KoopaState.falling;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("player").GetComponent<player_script>();
        enabled = false;
        Fall();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == KoopaState.shellKicked)
        {
            gameObject.GetComponent<SpriteRenderer>().color = Color.blue;
        }
        UpdateEnemyPosition();

        if (state == KoopaState.shell)
        {
            if (standTimer < timeBeforeUp)
            {
                standTimer += Time.deltaTime;
            } else
            {
                state = KoopaState.walking;
                gameObject.GetComponent<SpriteRenderer>().sprite = koopa;
            }
        }
    }

    public void Crush()
    {
        if (state == KoopaState.walking)
        {
            state = KoopaState.shell;
            //change sprite to shell
            gameObject.GetComponent<SpriteRenderer>().sprite = shell;
        }
        
    }

    void OnCollisionEnter2D(Collision2D other) {
        if (state == KoopaState.shell)
        {   
            if (other.gameObject.CompareTag("Player"))
            {
                //check the location of player then send the shell
                if (other.gameObject.transform.position.x < transform.position.x)
                {
                    state = KoopaState.shellKicked;
                    isWalkingLeft = false;
                } else {
                    state = KoopaState.shellKicked;
                    isWalkingLeft = true;
                }
            }
      }
      else
      {
          if (other.gameObject.CompareTag("Player"))
            {
                player.health--;
            }
      }

      if (state == KoopaState.shellKicked)
      {
          if (other.gameObject.CompareTag("Enemy"))
          {
              other.collider.GetComponent<enemyAI>().Crush();
          }
      }


    }

    void UpdateEnemyPosition()
    {
        if (state != KoopaState.dead)
        {
            Vector3 pos = transform.localPosition;
            Vector3 scale = transform.localScale;

            if (state == KoopaState.falling)
            {
                pos.y += velocity.y * Time.deltaTime;

                velocity.y -= gravity * Time.deltaTime;
            }

            if (state == KoopaState.walking)
            {
                if (isWalkingLeft)
                {
                    pos.x -= velocity.x * Time.deltaTime;

                    scale.x = -1;
                }
                else
                {
                    pos.x += velocity.x * Time.deltaTime;

                    scale.x = 1;
                }
            }
            if (state == KoopaState.shellKicked)
            {
                if (isWalkingLeft)
                {
                    pos.x -= shellVel.x * Time.deltaTime;

                    scale.x = -1;
                }
                else
                {
                    pos.x += shellVel.x * Time.deltaTime;

                    scale.x = 1;
                }
            }

            if (velocity.y <= 0)
            {
                pos = CheckGround(pos);
            }

            CheckWalls(pos, scale.x);

            transform.localPosition = pos;
            transform.localScale = scale;
        }
    }

    Vector3 CheckGround(Vector3 pos)
    {
        Vector2 originLeft = new Vector2(pos.x - 0.5f + 0.2f, pos.y - 0.5f);
        Vector2 originMiddle = new Vector2(pos.x, pos.y - 0.5f);
        Vector2 originRight = new Vector2(pos.x + 0.5f - 0.2f, pos.y - 0.5f);

        RaycastHit2D groundLeft = Physics2D.Raycast (originLeft, Vector2.down, 1.0f, floorMask);
        RaycastHit2D groundMiddle = Physics2D.Raycast (originMiddle, Vector2.down, 1.0f, floorMask);
        RaycastHit2D groundRight = Physics2D.Raycast (originRight, Vector2.down, 1.0f, floorMask);

        if (groundLeft.collider != null || groundMiddle.collider != null || groundRight.collider != null)
        {
            RaycastHit2D hitRay = groundLeft;
            if (groundLeft)
            {
                
                hitRay = groundLeft;
            }
            else if (groundMiddle)
            {
                
                hitRay = groundMiddle;
            }
            else if (groundRight)
            {
               
                hitRay = groundRight;
            }
            
            if (hitRay.collider.tag == "Player")
            {
                player.health--;
            }

            pos.y = hitRay.collider.bounds.center.y + hitRay.collider.bounds.size.y/2 + 0.5f;
            grounded = true;
            velocity.y = 0;
            if (state != KoopaState.shell && state != KoopaState.shellKicked)
            {
                state = KoopaState.walking;
            }
            

        } else {
            if (state != KoopaState.falling)
            {
                Fall();
            }
        }
        return pos;
    }

    void CheckWalls(Vector3 pos, float direction)
    {
        Vector2 originTop = new Vector2(pos.x + direction * 0.4f, pos.y + 0.5f - 0.2f);
        Vector2 originMiddle = new Vector2(pos.x + direction * 0.4f, pos.y);
        Vector2 originBottom = new Vector2(pos.x + direction * 0.4f, pos.y - 0.5f + 0.2f);

        RaycastHit2D wallTop = Physics2D.Raycast(originTop, new Vector2(direction, 0), 0.5f, wallMask);
        RaycastHit2D wallMiddle = Physics2D.Raycast(originMiddle, new Vector2(direction, 0), 0.5f, wallMask);
        RaycastHit2D wallBottom = Physics2D.Raycast(originBottom, new Vector2(direction, 0), 0.5f, wallMask);

        if (wallTop.collider != null || wallMiddle.collider != null || wallBottom.collider != null)
        {
            RaycastHit2D hitRay = wallTop;

            if (wallTop)
            {
                hitRay = wallTop;
          
            } else if (wallMiddle)
            {
                hitRay = wallMiddle;
                
            } else if (wallBottom)
            {
                hitRay = wallBottom;
               
            }

            if (hitRay.collider.tag == "Player" && state != KoopaState.shell)
            {
                player.health--;
            }

            isWalkingLeft = !isWalkingLeft;
        }

    }
    // when enemy is on screen turn them on
    void OnBecameVisible()
    {
        enabled = true;
    }


    //method for falling
    void Fall()
    {
        velocity.y = 0;
        state = KoopaState.falling;
        grounded = false;
    }

      void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.gameObject.CompareTag("fireball"))
        {
            score_manager.instance.ChangeScore(200);
            score_manager.instance.createPoint(transform.localPosition.x + 0.5f, transform.localPosition.y + 0.5f, 200);
            Destroy(other.gameObject);
            Destroy(this.gameObject);
        }

    }
}