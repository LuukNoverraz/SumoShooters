using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death : MonoBehaviour
{
    private GameController gameController;

    void Awake() => gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();

    void OnTriggerEnter(Collider collision)
    {
        // Check if collision with death box is player and respawn and call life lost function
        if (collision.gameObject.tag == "Players")
        {
            collision.gameObject.transform.position = new Vector3(0.0f, 2.0f, collision.gameObject.GetComponent<PlayerController>().startZ);
            collision.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            if (collision.gameObject.layer == 30)
            {
                gameController.LifeLostPlayer1();
            }
            if (collision.gameObject.layer == 31)
            {
                gameController.LifeLostPlayer2();
            }
        }
    }
}
