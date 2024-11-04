using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivesPickUp : MonoBehaviour
{
    bool wasCollected = false;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !wasCollected)
        {
            if (!(GameSession.Instance.getLiveCount() == 3))
            {
                wasCollected = true;
                Destroy(gameObject);
            }

            GameSession.Instance.IncreasePlayerLives();
        }
    }
}

