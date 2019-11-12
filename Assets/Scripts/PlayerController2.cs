using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController2 : MonoBehaviour
{
    public Rigidbody rb;
    public float force;
    public Image player2RectImage;
    private float shootLaunch = 1f;
    private bool player2Launching = false;
    private Vector2 startSwipe;
    private Vector2 endSwipe;
    public Text redText;
    public Text redWin;
    public Text blueWin;
    public Text[] restartText;
    int redScore = 0;
    public float[] colorList;
    void Update()
    {
        redText.text = "SCORE: " + redScore;
        if (redScore >= 3)
        {
            redWin.text = blueWin.text = "RED WINS";
            redWin.color = blueWin.color = new Color(colorList[0], colorList[1], colorList[2], 1.0f);
            restartText[0].color = restartText[1].color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
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
            redScore++;
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
