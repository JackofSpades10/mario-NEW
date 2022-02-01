using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class score_manager : MonoBehaviour
{
    public static score_manager instance;
    public TextMeshProUGUI coinText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI lifeText;
    public TextMeshProUGUI timerText;
    int coins;
    public int score;
    public float timer = 3f;
    public float tick = 0;
    public float uiTimer = 400f;
    public bool levelDone = false;

    public static int lives = 3;
    // Start is called before the first frame update
    void Start()
    {
        lifeText.text = "x   "+lives;
        DontDestroyOnLoad(this.gameObject);
        tick = 0;
        if (instance == null)
        {
            instance = this;
        }

    }

    public void createPoint(float x, float y, int points)
    {
                GameObject pointey = (GameObject)Instantiate(Resources.Load("Prefabs/pointIndic",typeof(GameObject)));
                pointey.transform.localPosition = new Vector2 (x, y);
                pointey.transform.SetParent(GameObject.Find("worldCanvas").transform);
                pointey.GetComponent<pointIndic>().setPoints(points);
    }

    public void changeLives(int num)
    {
        tick = 0;
        lives += num;
        lifeText.text = "x   "+lives;
        SceneManager.LoadScene(3);
    }

    public void ChangeScore(int Value)
    {
        //change the score
        score += Value;
        if (score > 0 && score < 1000)
        {
            scoreText.text = "000"+score.ToString();
        }
        else if (score > 999 && score < 10000)
        {
            scoreText.text = "00"+score.ToString();
        }
        else if (score > 9999 && score < 100000)
        {
            scoreText.text = "0"+score.ToString();
        }
        else
        {
            scoreText.text = score.ToString();
        }
    }

    public void ChangeCoin(int coinValue)
    {
        //change the coin total
        coins += coinValue;
        if (coins > 0 && coins < 10)
        {
            coinText.text = "x0"+coins.ToString();
        }
        else
        {
            coinText.text = "x"+coins.ToString();
        }
        
    }
    // Update is called once per frame
    void Update()
    {   
        if (SceneManager.GetActiveScene().buildIndex == 3)
        {
            lifeText.text = "x   "+lives;
        }

        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            lifeText.text = "";
            tick = 0;
            if (!levelDone)
            {
                uiTimer -= Time.deltaTime/0.4f;
            }
            timerText.text = Mathf.Round(uiTimer).ToString();

            if (uiTimer <= 0 && !levelDone)
            {
                GameObject.Find("player").GetComponent<player_script>().health = 0;
            }

        }


        if(tick < timer && SceneManager.GetActiveScene().buildIndex != 2)
        {
            tick += Time.deltaTime;
        }
        else if (SceneManager.GetActiveScene().buildIndex != 2)
        { 
            SceneManager.LoadScene(2);
            uiTimer = 400f;
        }

    }
}
