using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum PlayState
{
    PLAY,
    PAUSE,
    EXIT
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
    private Color white = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    private Color black = new Color(0.0f, 0.0f, 0.0f, 1.0f);
    public Text redWin;
    public Text blueWin;
    public Text[] restartText;
    public Image[] winBackground;
    public int maxScore;
    public float[] colorList;
    [HideInInspector] public int player1DeathCount = 0;
    [HideInInspector] public int player2DeathCount = 0;
    bool spawning = false;
    public Color player1Color;
    public Color player2Color;
    public ParticleSystem powerUpDisappear;
    public Vector3[] powerUpSpawnLocations;
    public MeshFilter Sphere;
    public MeshFilter Cube;
    public RectTransform player1Blooper;
    public RectTransform player2Blooper;
    public int currentSumocoins;
    

    void Start()
    {
        if (!PlayerPrefs.HasKey("Sumocoins"))
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
        player1Blooper.localPosition = new Vector2(0, (Screen.height / 2) - 315);
        player2Blooper.localPosition = new Vector2(0, -((Screen.height / 2) - 315));
        player1Blooper.sizeDelta = player2Blooper.sizeDelta = new Vector2(Screen.width, Screen.height / 2);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ChangePlayState();
        }
        if (player2DeathCount >= maxScore)
        {
            redWin.text = blueWin.text = "PLAYER 1 WINS";
            redWin.color = blueWin.color = restartText[0].color = restartText[1].color = white;
            winBackground[0].color = winBackground[1].color = black;
            PlayerPrefs.SetInt("Sumocoins", (currentSumocoins + 1));
            Time.timeScale = 0;
            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.R))
            {
                Time.timeScale = 1;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }

        if (player1DeathCount >= maxScore)
        {
            redWin.text = blueWin.text = "PLAYER 2 WINS";
            // redWin.color = blueWin.color = new Color(colorList[3], colorList[4], colorList[5], 1.0f);
            redWin.color = blueWin.color = restartText[0].color = restartText[1].color = white;
            winBackground[0].color = winBackground[1].color = black;
            PlayerPrefs.SetInt("Sumocoins", (currentSumocoins + 1));
            Time.timeScale = 0;
            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.R))
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

        if (gameObject.tag == "Exit")
        {
            currentPlayState = PlayState.EXIT;
        }
        else if (currentPlayState == PlayState.PLAY)
        {
            currentPlayState = PlayState.PAUSE;
        }
        else
        {
            currentPlayState = PlayState.PLAY;
        }
        switch(currentPlayState)
        {
            case PlayState.EXIT:
                SceneManager.LoadScene("TitleScreen");
                break;
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
        if (player1DeathCount < maxScore)
        {
            Destroy(player1Stocks[player1DeathCount]);
            player1DeathCount++;
        }
    }

    public void LifeLostPlayer2()
    {
        if (player2DeathCount < maxScore)
        {
            Destroy(player2Stocks[player2DeathCount]);
            player2DeathCount++;
        }
    }

    void PowerUpSpawn()
    {
        if (GameObject.FindGameObjectWithTag("PowerUp") == null) 
        {
            Instantiate(powerUpPrefab, powerUpSpawnLocations[(int) Random.Range(0, 3)], transform.rotation);
            spawning = false;
        }
    }
}
