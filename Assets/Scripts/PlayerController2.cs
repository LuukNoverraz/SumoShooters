using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController2 : MonoBehaviour
{
    public Rigidbody rb;
    public float force = 2f;
    public Image player2RectImage;
    private float shootLaunch = 1f;
    // public bool isTarget = false;
    public float zFactor = 2f;
    public Vector3 startPosition;
    private Vector2 startSwipe;
    private Vector2 endSwipe;
    public Text redText;
    public Text redWin;
    public Text blueWin;
    public Text[] restartText;
    int redScore = 0;
    public float[] colorList;

    void Start()
    {
        startPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
    }
    void Update()
    {
        redText.text = "SCORE: " + redScore;
        if (redScore >= 3)
        {
            redWin.text = "RED WINS";
            redWin.color = new Color(colorList[0], colorList[1], colorList[2], 1.0f);
            restartText[0].color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            restartText[1].color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            blueWin.text = "RED WINS";
            blueWin.color = new Color(colorList[0], colorList[1], colorList[2], 1.0f);
            Time.timeScale = 0;
            if (Input.GetMouseButtonDown(0))
            {
                Time.timeScale = 1;
                SceneManager.LoadScene("LocalMultiplayer");
            }
        }
        if (Input.GetMouseButtonDown(0) && Camera.main.ScreenToViewportPoint(Input.mousePosition).y >= 0.5)
        {
            startSwipe = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        }

        if (Input.GetMouseButtonUp(0) && Camera.main.ScreenToViewportPoint(Input.mousePosition).y >= 0.5)
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
            redScore++;
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
