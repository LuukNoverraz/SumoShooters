using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController1 : MonoBehaviour
{
    public Rigidbody rb;
    public float force;
    private bool player1Launching = false;
    private Vector2 startSwipe;
    private Vector2 endSwipe;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Camera.main.ScreenToViewportPoint(Input.mousePosition).y <= 0.5)
        {
            startSwipe = Camera.main.ScreenToViewportPoint(Input.mousePosition);
            player1Launching = true;
        }

        if (Input.GetMouseButtonUp(0) && player1Launching)
        {
            endSwipe = Camera.main.ScreenToViewportPoint(Input.mousePosition);
            Swipe();
            player1Launching = false;
        }

        if (transform.position.y < -5.0f)
        {
            transform.position = new Vector3(0.0f, 2.0f, -2.0f);
            rb.velocity = Vector3.zero;
            GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().blueScore++;
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
