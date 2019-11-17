using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController2 : MonoBehaviour
{
    public Rigidbody rb;
    public float force;
    private float shootLaunch = 1f;
    private bool player2Launching = false;
    private Vector2 startSwipe;
    private Vector2 endSwipe;
    public GameController gameController;

    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && gameController.pausing == false && Camera.main.ScreenToViewportPoint(Input.mousePosition).y >= 0.5)
        {
            startSwipe = Camera.main.ScreenToViewportPoint(Input.mousePosition);
            player2Launching = true;
        }

        if (Input.GetMouseButtonUp(0) && gameController.pausing == false && player2Launching)
        {
            endSwipe = Camera.main.ScreenToViewportPoint(Input.mousePosition);
            Swipe();
            player2Launching = false;
        }

        if (transform.position.y < -5.0f)
        {
            transform.position = new Vector3(0.0f, 2.0f, 2.0f);
            rb.velocity = Vector3.zero;
            gameController.redScore++;
        }
    }
    void Swipe()
    {
        Vector3 swipe = endSwipe - startSwipe;
        swipe.z = swipe.y;
        swipe.y = 0.0f;
        rb.AddForce(swipe * (force * shootLaunch), ForceMode.Impulse);
    }
}
