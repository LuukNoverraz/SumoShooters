using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController2 : MonoBehaviour
{
    public Rigidbody rb;
    public float force;
    public Image player2RectImage;
    private float shootLaunch = 1f;
    private bool player2Launching = false;
    private Vector2 startSwipe;
    private Vector2 endSwipe;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Camera.main.ScreenToViewportPoint(Input.mousePosition).y >= 0.5)
        {
            startSwipe = Camera.main.ScreenToViewportPoint(Input.mousePosition);
            player2Launching = true;
        }

        if (Input.GetMouseButtonUp(0) && player2Launching)
        {
            endSwipe = Camera.main.ScreenToViewportPoint(Input.mousePosition);
            Swipe();
            player2Launching = false;
        }

        if (transform.position.y < -5.0f)
        {
            transform.position = new Vector3(0.0f, 2.0f, 2.0f);
            rb.velocity = Vector3.zero;
            GameController.redScore++;
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
