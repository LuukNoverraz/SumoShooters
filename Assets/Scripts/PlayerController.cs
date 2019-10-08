using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody rb;
    public float force = 2f;
    // public bool isTarget = false;
    public float zFactor = 2f;
    public Vector3 startPosition;
 
    private Vector2 startSwipe;
    private Vector2 endSwipe;
    void Start () {
        startPosition = new Vector3 (transform.position.x, transform.position.y, transform.position.z);
    }
    void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            startSwipe = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        }
 
        if (Input.GetMouseButtonUp(0))
        {
            endSwipe = Camera.main.ScreenToViewportPoint(Input.mousePosition);
            // if (isTarget == true)
            // {
            Swipe();
            // isTarget = false;
            // }
        }
        if (transform.position.y < -5.0f) {
            transform.position = startPosition;
            rb.velocity = new Vector3(0.0f, 0.0f, 0.0f);
        }
    }
 
    void Swipe()
    {
        Vector3 swipe = endSwipe - startSwipe;
        swipe.z = swipe.y / zFactor;
        swipe.y = 0.0f;
        rb.AddForce(swipe * -force, ForceMode.Impulse);
    }
 
    private void OnMouseDown()
    {
        rb.constraints = RigidbodyConstraints.None;
        // isTarget = true;
    }
}
