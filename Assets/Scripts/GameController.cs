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
    public GameObject[] maps;
    public Transform mainLightTransform;
    public RectTransform borderTransform;
    public Image[] player1Stocks;
    public Image[] player2Stocks;
    public GameObject powerUpPrefab;
    public Text redWin;
    public Text blueWin;
    public Text[] restartText;
    public int maxScore;
    public float[] colorList;
    [HideInInspector] public bool player1LifeLost = false;
    [HideInInspector] public int player1DeathCount = 0;
    [HideInInspector] public bool player2LifeLost = false;
    [HideInInspector] public int player2DeathCount = 0;
    bool spawning = false;
    public Color player1Color;
    public Color player2Color;
    public ParticleSystem powerUpDisappear;
    public Vector3[] powerUpSpawnLocations;
    public MeshFilter Sphere;
    public MeshFilter Cube;
    public int currentSumocoins;
    

    void Start()
    {
        if (PlayerPrefs.HasKey("Sumocoins") == false)
        {
            PlayerPrefs.SetInt("Sumocoins", 0);
        }
        if (PlayerPrefs.HasKey("Sumocoins"))
        {
            currentSumocoins = PlayerPrefs.GetInt("Sumocoins", 0);
        }
        Instantiate(maps[(int) Random.Range(0, 4)]);
        borderTransform.sizeDelta = new Vector2(Screen.width, 10.0f);
        mainLightTransform.rotation = Quaternion.Euler(new Vector3(50.0f, Mathf.Floor(Random.Range(0.0f, 360.0f)), 0));
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ChangePlayState();
        }
        if (player2DeathCount >= maxScore)
        {
            redWin.text = blueWin.text = "RED WINS";
            redWin.color = blueWin.color = new Color(colorList[0], colorList[1], colorList[2], 1.0f);
            restartText[0].color = restartText[1].color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            PlayerPrefs.SetInt("Sumocoins", (currentSumocoins + 1));
            Time.timeScale = 0;
            if (Input.GetMouseButtonDown(0))
            {
                Time.timeScale = 1;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }

        if (player1DeathCount >= maxScore)
        {
            redWin.text = blueWin.text = "BLUE WINS";
            redWin.color = blueWin.color = new Color(colorList[3], colorList[4], colorList[5], 1.0f);
            restartText[0].color = restartText[1].color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            PlayerPrefs.SetInt("Sumocoins", (currentSumocoins + 1));
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
    
    public void ShootStylePush()
    {
        PlayerPrefs.SetFloat("ShootStyle", 1.0f);
    }

    public void ShootStyleDrag()
    {
        PlayerPrefs.SetFloat("ShootStyle", -1.0f);
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

    public void LifeLostPlayer1()
    {
        if (player1LifeLost = true && player1DeathCount < 5)
        {
            Destroy(player1Stocks[player1DeathCount]);
            player1LifeLost = false;
            player1DeathCount++;
        }
    }

    public void LifeLostPlayer2()
    {
        if (player2LifeLost = true && player2DeathCount < 5)
        {
            Destroy(player2Stocks[player2DeathCount]);
            player2LifeLost = false;
            player2DeathCount++;
        }
    }

    void PowerUpSpawn()
    {
        Debug.Log("spawn time");
        if (GameObject.FindGameObjectWithTag("PowerUp") == null) 
        {
            Instantiate(powerUpPrefab, powerUpSpawnLocations[(int) Random.Range(0, 3)], transform.rotation);
            spawning = false;
        }
    }
}
