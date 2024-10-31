using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivesPickUp : MonoBehaviour
{
    bool wasCollected = false;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !wasCollected)
        {
            if (FindObjectOfType<GameSession>().getLiveCount() == 3) 
            { 
                //do nothing
            }
            else
            {
                Debug.Log(FindObjectOfType<GameSession>().getLiveCount());
                wasCollected = true;
                Destroy(gameObject);
            }
            FindObjectOfType<GameSession>().IncreasePlayerLives();

        }
    }
}
