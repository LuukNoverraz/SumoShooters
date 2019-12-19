using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum PlayState
{
    PLAY,
    PAUSE
}
public class GameController : MonoBehaviour
{
    public PlayState currentPlayState;
    public Image pausePlayImage;
    public Sprite pauseSprite;
    public Sprite playSprite;
    public bool pausing = false;
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
    public int maxScore;
    public float[] colorList;
    bool spawning = false;
    public RectTransform stagePositioning;

    void Start()
    {
        mainLightTransform.rotation = Quaternion.Euler(new Vector3(50.0f, Mathf.Floor(Random.Range(0.0f, 360.0f)), 0));
        stagePositioning.sizeDelta = new Vector2(Screen.width, Screen.height);
        borderTransform.sizeDelta = new Vector2(Screen.width, 10.0f);
        scoreTexts[0].sizeDelta = new Vector2(Screen.width - 20.0f, 30.0f);
        scoreTexts[1].sizeDelta = new Vector2(Screen.width - 100.0f, 30.0f);
    }
    void Update()
    {
        redText.text = "SCORE: " + redScore;
        if (redScore >= maxScore)
        {
            redWin.text = blueWin.text = "RED WINS";
            redWin.color = blueWin.color = new Color(colorList[0], colorList[1], colorList[2], 1.0f);
            restartText[0].color = restartText[1].color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            Time.timeScale = 0;
            if (Input.GetMouseButtonDown(0))
            {
                Time.timeScale = 1;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }

        blueText.text = "SCORE: " + blueScore;
        if (blueScore >= maxScore)
        {
            redWin.text = blueWin.text = "BLUE WINS";
            redWin.color = blueWin.color = new Color(colorList[3], colorList[4], colorList[5], 1.0f);
            restartText[0].color = restartText[1].color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            Time.timeScale = 0;
            if (Input.GetMouseButtonDown(0))
            {
                Time.timeScale = 1;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }

        if (!spawning)
        {
            Invoke("PowerUpSpawn", Random.Range(6, 12));
            spawning = true;
        }
    }
    public void ChangePlayState()
    {
        if (currentPlayState == PlayState.PLAY)
        {
            currentPlayState = PlayState.PAUSE;
        }
        else 
        {
            currentPlayState = PlayState.PLAY;
        }
        switch(currentPlayState)
        {
            case PlayState.PAUSE:
                pausePlayImage.sprite = playSprite;
                pausing = true;
                Time.timeScale = 0;
                break;
            case PlayState.PLAY:
                pausePlayImage.sprite = pauseSprite;
                pausing = false;
                Time.timeScale = 1;
                break;
        }
    }

    public void LocalPlayButton()
    {
        SceneManager.LoadScene("LocalMultiplayer");
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
