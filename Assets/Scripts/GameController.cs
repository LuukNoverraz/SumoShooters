using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public Transform mainLightTransform;
    public RectTransform borderTransform;
    public RectTransform[] scoreTexts;
    public GameObject[] powerUpPrefab;
    public Text redText;
    public Text blueText;
    public Text redWin;
    public Text blueWin;
    public Text[] restartText;
    public int redScore = 0;
    public int blueScore = 0;
    int[] powerUps;
    public float[] colorList;
    bool spawning = false;

    void Start()
    {
        mainLightTransform.rotation = Quaternion.Euler(new Vector3(50.0f, Mathf.Floor(Random.Range(0.0f, 360.0f)), 0));
        borderTransform.sizeDelta = new Vector2(Screen.width, 10.0f);
        scoreTexts[0].sizeDelta = new Vector2(Screen.width - 20.0f, 30.0f);
        scoreTexts[1].sizeDelta = new Vector2(Screen.width - 100.0f, 30.0f);
    }
    void Update()
    {
        redText.text = "SCORE: " + redScore;
        if (redScore >= 5)
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

        blueText.text = "SCORE: " + blueScore;
        if (blueScore >= 5)
        {
            redWin.text = blueWin.text = "BLUE WINS";
            redWin.color = blueWin.color = new Color(colorList[3], colorList[4], colorList[5], 1.0f);
            restartText[0].color = restartText[1].color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            Time.timeScale = 0;
            if (Input.GetMouseButtonDown(0))
            {
                Time.timeScale = 1;
                SceneManager.LoadScene("LocalMultiplayer");
            }
        }

        if (!spawning)
        {
            Invoke("PowerUpSpawn", Random.Range(6, 12));
            spawning = true;
        }
    }
    void PowerUpSpawn()
    {
        if (GameObject.FindGameObjectWithTag("PowerUp") == null) 
        {
            Instantiate(powerUpPrefab[Random.Range(0, 2)], new Vector3(Random.Range(-3.0f, 3.0f), 0.75f, Random.Range(-3.0f, 3.0f)), transform.rotation);
            spawning = false;
        }
    }
}
