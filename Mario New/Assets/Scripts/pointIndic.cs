using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class pointIndic : MonoBehaviour
{
    public TextMeshProUGUI pointText;
    public float time = 0;
    public float timer = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (time < timer)
        {
            time += Time.deltaTime;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void setPoints(int points)
    {
        pointText.text = points.ToString();
    }
}
