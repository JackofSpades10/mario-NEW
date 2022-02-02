using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class castle : MonoBehaviour
{
    bool varSet = false;
    int timeLeft;
    int bonusPoints;
    int pointsGiven;
    float time = 0;
    float timer = 10;
    public Sprite flagsprite;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = flagsprite;
            //destroy the player
            Destroy(other.gameObject);
            //determine the amount of score they should get for time 50/sec
            if (varSet == false)
            { 
                Debug.Log("do thing");
                timeLeft = Mathf.RoundToInt(GameObject.Find("score_manager").GetComponent<score_manager>().uiTimer);
                bonusPoints = timeLeft * 50;
                 Debug.Log("BP =" + bonusPoints.ToString());
                varSet = true;
            }

        }
    }

    void Update() 
    {
        if (varSet)
        {
          //increment score decrement time
            if (pointsGiven != bonusPoints)
            {
                score_manager.instance.ChangeScore(50);
                pointsGiven += 50;
                timeLeft -= 1;
                GameObject.Find("score_manager").GetComponent<score_manager>().uiTimer -= 1;
            }
            else
            {
                if (time < timer)
                {
                    time += Time.deltaTime;
                }
                else
                {
                    DeleteAll();
                   SceneManager.LoadScene(0);
                }
            }
        }

    }

    public void DeleteAll()
    {
         foreach (GameObject o in Object.FindObjectsOfType<GameObject>()) {
             Destroy(o);
        }
    }

}
