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
    private bool exiting = false;
    public GameObject[] maps;
    public Image[] player1Stocks;
    public Image[] player2Stocks;
    public GameObject powerUpPrefab;
    private Color white = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    private Color black = new Color(0.0f, 0.0f, 0.0f, 1.0f);
    [SerializeField] private Text player1Win;
    [SerializeField] private Text player2Win;
    [SerializeField] private Text[] restartText;
    public Image[] winBackground;
    public int maxScore;
    [HideInInspector] public int player1DeathCount = 0;
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
        // Set Sumocoins from playerprefs and choose random map
        if (!PlayerPrefs.HasKey("Sumocoins"))
        {
            PlayerPrefs.SetInt("Sumocoins", 0);
        }
        if (PlayerPrefs.HasKey("Sumocoins"))
        {
            currentSumocoins = PlayerPrefs.GetInt("Sumocoins", 0);
        }
        Instantiate(maps[(int) Random.Range(0, 4)]);
    }

    private void Awake()
    {
         //Set screen size for Standalone
        #if UNITY_STANDALONE
            Screen.SetResolution(564, 960, false);
            Screen.fullScreen = false;
        #endif
    }

    void Update()
    {
        // Check for player pausing with escape key
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ChangePlayState();
        }
        // Check for player deaths using player death count that gets incremented in life lost functions
        // Show UI elements and set Sumocoins +1
        if (player1DeathCount >= maxScore)
        {
            player1Win.text = player2Win.text = "PLAYER 2 WINS";
            player1Win.color = player2Win.color = restartText[0].color = restartText[1].color = white;
            winBackground[0].color = winBackground[1].color = black;
            PlayerPrefs.SetInt("Sumocoins", (currentSumocoins + 1));
            Time.timeScale = 0;
            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.R))
            {
                Time.timeScale = 1;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
        
        if (player2DeathCount >= maxScore)
        {
            player1Win.text = player2Win.text = "PLAYER 1 WINS";
            player1Win.color = player2Win.color = restartText[0].color = restartText[1].color = white;
            winBackground[0].color = winBackground[1].color = black;
            PlayerPrefs.SetInt("Sumocoins", (currentSumocoins + 1));
            Time.timeScale = 0;
            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.R))
            {
                Time.timeScale = 1;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }

        // Spawns powerup within 6 to 12 seconds if powerup is not currently on the field
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

    public void ExitButton() => exiting = true;

    public void ChangePlayState()
    {
        if (exiting)
        {
            currentPlayState = PlayState.EXIT;
            exiting = false;
        }
        else if (currentPlayState == PlayState.PAUSE)
        {
            currentPlayState = PlayState.PLAY;
        }
        else
        {
            currentPlayState = PlayState.PAUSE;
        }
        switch(currentPlayState)
        {
            case PlayState.EXIT:
                Time.timeScale = 1;
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
            Debug.Log(player1DeathCount);
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
