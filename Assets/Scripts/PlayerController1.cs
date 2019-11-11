using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController1 : MonoBehaviour
{
    public Rigidbody rb;
    public float force = 2f;
    public Image player1RectImage;
    private float shootLaunch = 1f;
    // public bool isTarget = false;
    public float zFactor = 2f;
    public Vector3 startPosition;
    private Vector2 startSwipe;
    private Vector2 endSwipe;
    public Text blueText;
    public Text redWin;
    public Text blueWin;
    public Text[] restartText;
    int blueScore = 0;
    public float[] colorList;

    void Start()
    {
        startPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
    }
    void Update()
    {
        blueText.text = "SCORE: " + blueScore;
        if (blueScore >= 3)
        {
            redWin.text = "BLUE WINS";
            redWin.color = new Color(colorList[3], colorList[4], colorList[5], 1.0f);
            restartText[0].color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            restartText[1].color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            blueWin.text = "BLUE WINS";
            blueWin.color = new Color(colorList[3], colorList[4], colorList[5], 1.0f);
            Time.timeScale = 0;
            if (Input.GetMouseButtonDown(0))
            {
                Time.timeScale = 1;
                SceneManager.LoadScene("LocalMultiplayer");
            }
        }
        if (Input.GetMouseButtonDown(0) && Camera.main.ScreenToViewportPoint(Input.mousePosition).y <= 0.5)
        {
            startSwipe = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        }

        if (Input.GetMouseButtonUp(0) && Camera.main.ScreenToViewportPoint(Input.mousePosition).y <= 0.5)
        {
            endSwipe = Camera.main.ScreenToViewportPoint(Input.mousePosition);
            // if (isTarget == true)
            // {
            Swipe();
            // isTarget = false;
            // }
        }

        if (transform.position.y < -5.0f)
        {
            transform.position = startPosition;
            rb.velocity = new Vector3(0.0f, 0.0f, 0.0f);
            blueScore++;
        }
    }
 
    void Swipe()
    {
        Vector3 swipe = endSwipe - startSwipe;
        swipe.z = swipe.y / zFactor;
        swipe.y = 0.0f;
        rb.AddForce(swipe * (force * shootLaunch), ForceMode.Impulse);
    }
 
    private void OnMouseDown()
    {
        rb.constraints = RigidbodyConstraints.None;
        // isTarget = true;
    }
}
