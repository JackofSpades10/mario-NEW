using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bricks : MonoBehaviour
{
    // 0 for coin, 1 for infinite coin, 2 for powerup
    public float bounceHeight = 0.5f;
    public float bounceSpeed = 4f;

    private Vector2 originalPosition;
    bool destroySoon = false;
    float destroyTime = 0;
    float destroyTimer = 0.25f;
    private bool canBounce = true;
    float time = 0;
    float timer = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        originalPosition = transform.localPosition;
    }

    //called by player bounces block
    public void BrickBounce()
    {
        if (canBounce)
        {
            canBounce = false;

            if (GameObject.Find("player").GetComponent<player_script>().health == 1)
            {
                StartCoroutine(Bounce());
            }
            if (GameObject.Find("player").GetComponent<player_script>().health > 1)
            {
                // destroy block and give points

                score_manager.instance.ChangeScore(50);
                score_manager.instance.createPoint(transform.localPosition.x + 0.5f, transform.localPosition.y + 0.5f, 50);
                // change the sprite here
                gameObject.GetComponent<SpriteRenderer>().color = Color.blue;
                destroySoon = true;
           
            }
            
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (destroySoon)
        {
                 if (destroyTime < destroyTimer)
                {
                    destroyTime += Time.deltaTime;
                }
                else
                {
                    Destroy(gameObject);
                }
        }
        if (!canBounce)
        {
            if (time < timer)
            {
                time += Time.deltaTime;
            }
            else
            {
                canBounce = true;
                time = 0;
            }
            
        }
    }

    void ChangeSprite()
    {

    }

    IEnumerator Bounce()
    {   
 
          
        while(true)
        {
            transform.localPosition = new Vector2 (transform.localPosition.x, transform.localPosition.y + bounceSpeed * Time.deltaTime);
            
            if (transform.localPosition.y >= originalPosition.y + bounceHeight)
            break;

            yield return null;
        }

        while (true)
        {
            transform.localPosition = new Vector2 (transform.localPosition.x, transform.localPosition.y - bounceSpeed * Time.deltaTime);

            if (transform.localPosition.y <= originalPosition.y)
            {
                transform.localPosition = originalPosition;
                break;

                yield return null;
            }
        }



    }
}
