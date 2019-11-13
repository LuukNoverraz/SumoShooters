using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController1 : MonoBehaviour
{
    public Rigidbody rb;
    public float force;
    public Image player1RectImage;
    private float shootLaunch = 1f;
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
            GameController.blueScore++;
        }
    }
    void Swipe()
    {
        Vector3 swipe = endSwipe - startSwipe;
        swipe.z = swipe.y;
        swipe.y = 0.0f;
        rb.AddForce(swipe * (force * shootLaunch), ForceMode.Impulse);
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "PowerUp")
        {
            Destroy(collision.gameObject);
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            StartCoroutine(PowerUpTimer());
        }
    }
    IEnumerator PowerUpTimer() 
    {
        yield return new WaitForSeconds(3);
        transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
    }
}
