using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController2 : MonoBehaviour
{
    public Rigidbody rb;
    public float force;
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
            transform.position = new Vector3(0.0f, 2.0f, 2.0f);
            rb.velocity = Vector3.zero;
            gameController.player2LifeLost = true;
            gameController.Lifes();
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
