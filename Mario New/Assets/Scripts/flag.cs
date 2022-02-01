using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flag : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void dropTheFlag()
    {
        //code to do flag drop animation
        gameObject.GetComponent<SpriteRenderer>().color = Color.blue;
        GameObject.Find("score_manager").GetComponent<score_manager>().levelDone = true;
    }
}
