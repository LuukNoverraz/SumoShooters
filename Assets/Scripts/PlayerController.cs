﻿using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject player1;
    public GameObject player2;
    public Rigidbody rb;
    public float force;
    public float startZ;
    private Vector2 startSwipe;
    private Vector2 endSwipe;
    private float shootStyle;
    private GameController gameController;
    private PlayerController playerController1;
    private PlayerController playerController2;
    public Renderer playerRenderer;
    [SerializeField] private float multiplier;
    private bool touchingPowerUp = false;
    private float waitTime;
    private int randomPowerUp;
    public GameObject slownessParticle;
    public MeshFilter meshFilter;
    public Collider boxCollider;
    private Color newPlayer1Color;
    private Color newPlayer2Color;

    void Start()
    {
        // Get other scripts
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        playerController1 = player1.GetComponent<PlayerController>();
        playerController2 = player2.GetComponent<PlayerController>();

        // Get players chosen color from player preferences
        if (PlayerPrefs.HasKey("ChosenColorR") && gameObject.layer == 30)
        {
            newPlayer1Color = new Color(PlayerPrefs.GetFloat("ChosenColorR", 0.0f), PlayerPrefs.GetFloat("ChosenColorG", 0.0f), PlayerPrefs.GetFloat("ChosenColorB", 0.0f));
            playerRenderer.material.color = newPlayer1Color;
        }
        // Set player color to default if no preference has been set previously
        else 
        {
            newPlayer1Color = new Color(0.9803922f, 0.08627451f, 0.08627451f, 1.0f);
            playerRenderer.material.color = newPlayer1Color;
        }
        if (PlayerPrefs.HasKey("ChosenColorR2") && gameObject.layer == 31)
        {
            newPlayer2Color = new Color(PlayerPrefs.GetFloat("ChosenColorR2", 0.0f), PlayerPrefs.GetFloat("ChosenColorG2", 0.0f), PlayerPrefs.GetFloat("ChosenColorB2", 0.0f));
            playerRenderer.material.color = newPlayer2Color;
        }
        else
        {
            newPlayer2Color = new Color(0.0f, 0.172549f, 0.4901961f, 1.0f);
            playerRenderer.material.color = newPlayer1Color;
        }

        // Get shoot style from preferences, set to push if none are found
        if (PlayerPrefs.HasKey("ShootStyle"))
        {
            shootStyle = PlayerPrefs.GetFloat("ShootStyle", 0.0f);
        }
        if (!PlayerPrefs.HasKey("ShootStyle"))
        {
            shootStyle = 1.0f;
        }

        // Change life icons (stocks) from selected color
        if (gameObject.layer == 30)
        {
            gameController.player1Color = playerRenderer.material.color;
            for (int i = 0; i < 5; i++)
            {
                gameController.player1Stocks[i].color = new Color(newPlayer1Color.r, newPlayer1Color.g, newPlayer1Color.b);
            }
        }
        if (gameObject.layer == 31)
        {
            gameController.player2Color = playerRenderer.material.color;
            for (int i = 0; i < 5; i++)
            {
                gameController.player2Stocks[i].color = new Color(newPlayer2Color.r, newPlayer2Color.g, newPlayer2Color.b);
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Multiply other players force by impact (so other players get knocked off the map easier)
        if (collision.gameObject.tag == "Players")
        {
            if (gameObject.layer == 30)
            {
                Rigidbody playerCol = player2.GetComponent<Rigidbody>();
                playerCol.velocity = playerCol.velocity * multiplier;
            }
            if (gameObject.layer == 31)
            {
                Rigidbody playerCol = player1.GetComponent<Rigidbody>();
                playerCol.velocity = playerCol.velocity * multiplier;
            }
        }
    }

    void OnTriggerEnter(Collider collision)
    {
        // Check for powerup
        if (collision.gameObject.tag == "PowerUp" && !touchingPowerUp)
        {
            touchingPowerUp = true;
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
        // Changes revert back to normal after timer
        yield return new WaitForSeconds(waitTime);
        transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
        rb.mass = 0.05f;
        rb.constraints = RigidbodyConstraints.None;
        playerController1.force = playerController2.force = 0.75f;
        playerController1.slownessParticle.SetActive(false);
        playerController2.slownessParticle.SetActive(false);
        meshFilter.sharedMesh = gameController.Sphere.sharedMesh;
        boxCollider.enabled = false;
        touchingPowerUp = false;
    }
    
    IEnumerator PowerUpParticleTimer()
    {
        // Powerup and particle disappear after touching
        yield return new WaitForSeconds(waitTime);
        Destroy(GameObject.FindGameObjectWithTag("PowerUp"));
        yield return new WaitForSeconds(waitTime);
        Destroy(GameObject.FindGameObjectWithTag("PowerUpParticle"));
    }
    
    public void BeginShoot()
    {
        // Code gets called from either TouchInput or MultiplayerInput
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
        // Add force to player using begin and end point
        Vector3 swipe = endSwipe - startSwipe;
        swipe.z = swipe.y;
        swipe.y = 0.0f;
        rb.AddForce(swipe * (force * shootStyle), ForceMode.Impulse);
    }
}
