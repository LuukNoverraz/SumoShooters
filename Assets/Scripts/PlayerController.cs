using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public Rigidbody rb;
    public float force;
    public float startZ;
    private Vector2 startSwipe;
    private Vector2 endSwipe;
    public GameController gameController;

    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }
    void Update()
    {
        if (transform.position.y < -5.0f)
        {
            transform.position = new Vector3(0.0f, 2.0f, startZ);
            rb.velocity = Vector3.zero;
            if (gameObject.layer == 30)
            {
                gameController.player1LifeLost = true;
                gameController.LifeLostPlayer1();
            }
            if (gameObject.layer == 31)
            {
                gameController.player2LifeLost = true;
                gameController.LifeLostPlayer2();
            }
        }
    }

    public void BeginShoot()
    {
        if (!gameController.pausing)
        {
            startSwipe = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        }
    }

    public void EndShoot()
    {
        if (!gameController.pausing)
        {
            endSwipe = Camera.main.ScreenToViewportPoint(Input.mousePosition);
            Swipe();
        }
    }
    
    void Swipe()
    {
        Vector3 swipe = endSwipe - startSwipe;
        swipe.z = swipe.y;
        swipe.y = 0.0f;
        rb.AddForce(swipe * force, ForceMode.Impulse);
    }
}
