using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deathPit : MonoBehaviour
{
  void OnTriggerEnter2D(Collider2D other)
  {
      if (other.tag == "Player")
      {
          Application.LoadLevel("lose");
      }
  }
}
