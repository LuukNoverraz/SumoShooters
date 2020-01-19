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

    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        shootStyle = PlayerPrefs.GetFloat("ShootStyle", 0.0f);
        playerController1 = GameObject.Find("Player 1").GetComponent<PlayerController>();
        playerController2 = GameObject.Find("Player 2").GetComponent<PlayerController>();
        audioController = GameObject.Find("Audio Controller").GetComponent<AudioController>();
        colorPicker.onValueChanged.AddListener(color =>
        {
            PlayerPrefsX.SetColor("Color", color);
            renderer.material.color = color;
        });
        savedColor = PlayerPrefsX.GetColor("Color", new Color(0.0f, 0.0f, 0.0f, 1.0f));
        if (PlayerPrefs.HasKey("Color"))
        {
            renderer.material.color = savedColor;
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
            StartCoroutine(Timer());
            // if (collision.gameObject.layer == 11)
            // {    
            //     transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            //     rb.mass = 1.0f;
            //     if (gameObject.layer == 30)
            //     {
            //         playerController1.force = 20.0f;
            //     }
            //     if (gameObject.layer == 31)
            //     {
            //         playerController2.force = 20.0f;
            //     }
            // }
            // if (collision.gameObject.layer == 12)
            // {
            //     transform.position = new Vector3(transform.position.x, 2.0f, transform.position.z);
            //     rb.constraints = RigidbodyConstraints.FreezePositionY;
            // }
            // StartCoroutine(PowerUpTimer());
        }
    }

    IEnumerator Timer()
    {
        yield return new WaitForSeconds(waitTime);
        Destroy(GameObject.FindGameObjectWithTag("PowerUp"));
        yield return new WaitForSeconds(waitTime);
        Destroy(GameObject.FindWithTag("PowerUpParticle"));
    }
    IEnumerator PowerUpTimer() 
    {
        yield return new WaitForSeconds(3);
        transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
        rb.mass = 0.05f;
        rb.constraints = RigidbodyConstraints.None;
        if (gameObject.layer == 30)
        {
            playerController1.force = 1.0f;
        }
        if (gameObject.layer == 31)
        {
            playerController2.force = 1.0f;
        }
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
