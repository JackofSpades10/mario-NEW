using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionBlock : MonoBehaviour
{

    public int blockType;
    // 0 for coin, 1 for infinite coin, 2 for powerup
    public float bounceHeight = 0.5f;
    public float bounceSpeed = 4f;

    public float coinMoveSpeed = 8f;
    public float coinMoveHeight = 3f;
    public float coinFallDistance = 2f;

    private Vector2 originalPosition;

    private bool canBounce = true;

    public Sprite emptyBlockSprite;

    // Start is called before the first frame update
    void Start()
    {
        originalPosition = transform.localPosition;
    }

    //called by player bounces block
    public void QuestionBlockBounce()
    {
        if (canBounce)
        {
            canBounce = false;
            if (blockType == 0)
            {
                score_manager.instance.ChangeScore(200);
                score_manager.instance.createPoint(transform.localPosition.x + 0.5f, transform.localPosition.y + 0.5f, 200);
                score_manager.instance.ChangeCoin(1);
            }
            

            StartCoroutine(Bounce());
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ChangeSprite()
    {
        GetComponent<Animator>().enabled = false;

        GetComponent<SpriteRenderer>().sprite = emptyBlockSprite;
    }

    void PresentCoin()
    {
        GameObject spinningCoin = (GameObject)Instantiate (Resources.Load("Prefabs/Spinning_Coin", typeof(GameObject)));
        spinningCoin.transform.SetParent(this.transform.parent);
        spinningCoin.transform.localPosition = new Vector2 (originalPosition.x, originalPosition.y + 1);

        StartCoroutine (MoveCoin (spinningCoin));
    }

    IEnumerator Bounce()
    {   
        if (blockType == 0 || blockType == 2)
        {
            ChangeSprite();
        }

        if (blockType == 0 || blockType == 1)
        {
            PresentCoin();
        } else
        {
            if (GameObject.Find("player").GetComponent<player_script>().health == 1)
            {
            GameObject mushroom = (GameObject)Instantiate(Resources.Load("Prefabs/mushroom",typeof(GameObject)));
            mushroom.transform.SetParent(this.transform.parent);
            mushroom.transform.localPosition = new Vector2 (originalPosition.x, originalPosition.y + 1);
            }
            if(GameObject.Find("player").GetComponent<player_script>().health == 2)
            {
            GameObject fireflower = (GameObject)Instantiate(Resources.Load("Prefabs/fireflower",typeof(GameObject)));
            fireflower.transform.SetParent(this.transform.parent);
            fireflower.transform.localPosition = new Vector2 (originalPosition.x, originalPosition.y + 1);
            }
        }
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

    IEnumerator MoveCoin(GameObject coin) {
        while(true)
        {
            coin.transform.localPosition = new Vector2(coin.transform.localPosition.x, coin.transform.localPosition.y + coinMoveSpeed * Time.deltaTime);
            if (coin.transform.localPosition.y >= originalPosition.y + coinMoveHeight + 1)
            break;
            
            yield return null;
        }
          while(true)
        {
            coin.transform.localPosition = new Vector2(coin.transform.localPosition.x, coin.transform.localPosition.y - coinMoveSpeed * Time.deltaTime);
            if (coin.transform.localPosition.y <= originalPosition.y + coinFallDistance + 1)
            {
                Destroy(coin.gameObject);
                break;
            }
            yield return null;
        }
    }
}
