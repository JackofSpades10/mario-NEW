using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coin : MonoBehaviour
{
    public int coinValue = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            score_manager.instance.ChangeScore(200);
            score_manager.instance.createPoint(transform.localPosition.x + 0.5f, transform.localPosition.y + 0.5f, 200);
            score_manager.instance.ChangeCoin(1);
        }
    }
}
