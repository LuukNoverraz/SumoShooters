using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public Rigidbody rb;
    public float force;
    public float startZ;
    private Vector2 startSwipe;
    private Vector2 endSwipe;
    public GameController gameController;
    float shootStyle;
    public GameObject player1;
    public GameObject player2;
    public PlayerController playerController1;
    public PlayerController playerController2;
    public AudioController audioController;
    public GameObject bruh;
    public Renderer renderer;
    public ColorPicker colorPicker;
    Color savedColor;
    public float multiplier;
    float waitTime;
    public int randomPowerUp;
    public GameObject slownessParticle;
    public MeshFilter meshFilter;
    public Collider boxCollider;
    Color newPlayer1Color;
    Color newPlayer2Color;

    void Start()
    {
        if (PlayerPrefs.HasKey("ChosenColorR") && gameObject.layer == 30)
        {
            Debug.Log("yup");
            newPlayer1Color = new Color(PlayerPrefs.GetFloat("ChosenColorR", 0.0f), PlayerPrefs.GetFloat("ChosenColorG", 0.0f), PlayerPrefs.GetFloat("ChosenColorB", 0.0f));
            renderer.material.color = newPlayer1Color;
        }
        if (PlayerPrefs.HasKey("ChosenColorR2") && gameObject.layer == 31)
        {
            Debug.Log("yup");
            newPlayer2Color = new Color(PlayerPrefs.GetFloat("ChosenColorR2", 0.0f), PlayerPrefs.GetFloat("ChosenColorG2", 0.0f), PlayerPrefs.GetFloat("ChosenColorB2", 0.0f));
            renderer.material.color = newPlayer2Color;
        }
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        if (PlayerPrefs.HasKey("ShootStyle"))
        {
            shootStyle = PlayerPrefs.GetFloat("ShootStyle", 0.0f);
        }
        if (PlayerPrefs.HasKey("ShootStyle") == false)
        {
            shootStyle = 1.0f;
        }
        playerController1 = GameObject.Find("Player 1").GetComponent<PlayerController>();
        playerController2 = GameObject.Find("Player 2").GetComponent<PlayerController>();
        audioController = GameObject.Find("Audio Controller").GetComponent<AudioController>();
    }

    void Awake()
    {
        gameController.player1Color = renderer.material.color;
        gameController.player2Color = renderer.material.color;
        for (int i = 0; i < 5; i++)
        {
            gameController.player1Stocks[i].color = new Color(newPlayer1Color.r * 255, newPlayer1Color.g * 255, newPlayer1Color.b * 255);
            gameController.player2Stocks[i].color = new Color(newPlayer2Color.r * 255, newPlayer2Color.g * 255, newPlayer2Color.b * 255);
        }
    }

    void Update()
    {
        if (transform.position.y < -5.0f)
        {
            transform.position = new Vector3(0.0f, 2.0f, startZ);
            rb.velocity = Vector3.zero;
            if (gameObject.layer == 30)
            {
                gameController.player1LifeLost = true;
                gameController.LifeLostPlayer1();
            }
            if (gameObject.layer == 31)
            {
                gameController.player2LifeLost = true;
                gameController.LifeLostPlayer2();
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Players")
        {
            Rigidbody playerCol = collision.gameObject.GetComponent<Rigidbody>();
            playerCol.velocity = playerCol.velocity * multiplier;
            // bruh = Instantiate(audioController.audioSources[0]);
            // bruh.GetComponent<AudioSource>().Play(0);
            // StartCoroutine(SoundTimer());
            // Destroy(bruh);
        }
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "PowerUp")
        {
            Instantiate(gameController.powerUpDisappear, new Vector3(collision.gameObject.transform.position.x, collision.gameObject.transform.position.y, collision.gameObject.transform.position.z), Quaternion.identity);
            waitTime = 0.5f;
            StartCoroutine(PowerUpParticleTimer());
            randomPowerUp = (int) Random.Range(0, 3);
            if (randomPowerUp == 0)
            {  
                // Players size increases
                transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                rb.mass = 1.0f;
                if (gameObject.layer == 30)
                {
                    playerController1.force = 20.0f;
                }
                if (gameObject.layer == 31)
                {
                    playerController2.force = 20.0f;
                }
                waitTime = 3.0f;
                StartCoroutine(PowerUpTimer());
            }
            if (randomPowerUp == 1)
            {
                // Other player gets immobilized
                if (gameObject.layer == 30)
                {
                    playerController2.force = 0.05f;
                    playerController2.slownessParticle.SetActive(true);
                }
                if (gameObject.layer == 31)
                {
                    playerController1.force = 0.05f;
                    playerController1.slownessParticle.SetActive(true);
                }
                waitTime = 4.0f;
                StartCoroutine(PowerUpTimer());
            }
            if (randomPowerUp == 2)
            {
                // Player gets turned into cube
                meshFilter.sharedMesh = gameController.Cube.sharedMesh;
                boxCollider.enabled = true;
                waitTime = 10.0f;
                StartCoroutine(PowerUpTimer());
            }
        }
    }

    IEnumerator PowerUpTimer() 
    {
        yield return new WaitForSeconds(waitTime);
        transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
        rb.mass = 0.05f;
        rb.constraints = RigidbodyConstraints.None;
        playerController1.force = playerController2.force = 1.0f;
        playerController1.slownessParticle.SetActive(false);
        playerController2.slownessParticle.SetActive(false);
        meshFilter.sharedMesh = gameController.Sphere.sharedMesh;
        boxCollider.enabled = false;
    }
    
    IEnumerator PowerUpParticleTimer()
    {
        yield return new WaitForSeconds(waitTime);
        Destroy(GameObject.FindGameObjectWithTag("PowerUp"));
        yield return new WaitForSeconds(waitTime);
        Destroy(GameObject.FindGameObjectWithTag("PowerUpParticle"));
    }
    
    public void BeginShoot()
    {
        if (!gameController.pausing)
        {
            startSwipe = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        }
    }

    public void EndShoot()
    {
        if (!gameController.pausing)
        {
            endSwipe = Camera.main.ScreenToViewportPoint(Input.mousePosition);
            Swipe();
        }
    }
    
    void Swipe()
    {
        Vector3 swipe = endSwipe - startSwipe;
        swipe.z = swipe.y;
        swipe.y = 0.0f;
        rb.AddForce(swipe * (force * shootStyle), ForceMode.Impulse);
    }
}
