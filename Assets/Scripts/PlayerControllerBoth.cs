using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerBoth : MonoBehaviour
{
    public Light playerGlow;
    public ParticleSystem floatParticle;
    public Rigidbody rb;
    public GameObject player1;
    public GameObject player2;
    public PlayerController playerController1;
    public PlayerController playerController2;
    public Renderer renderer;
    public ColorPicker colorPicker;
    Color savedColor;
    public float multiplier;
    
    void Start()
    {
        playerController1 = GameObject.Find("Player 1").GetComponent<PlayerController>();
        playerController2 = GameObject.Find("Player 2").GetComponent<PlayerController>();
        colorPicker.onValueChanged.AddListener(color =>
        {
            PlayerPrefsX.SetColor("Color", color);
            Debug.Log("hoi");
            renderer.material.color = color;
        });
        savedColor = PlayerPrefsX.GetColor("Color", new Color(0.0f, 0.0f, 0.0f, 1.0f));
        if (PlayerPrefs.HasKey("Color"))
        {
            renderer.material.color = savedColor;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Players")
        {
            Rigidbody playerCol = collision.gameObject.GetComponent<Rigidbody>();
            playerCol.velocity = playerCol.velocity * multiplier;
        }
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "PowerUp")
        {
            Destroy(collision.gameObject);
            if (collision.gameObject.layer == 11)
            {    
                transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                playerGlow.intensity = 4.2f;
                rb.mass = 1.0f;
                if (gameObject.layer == 30)
                {
                    playerController1.force = 20.0f;
                }
                if (gameObject.layer == 31)
                {
                    playerController2.force = 20.0f;
                }
            }
            if (collision.gameObject.layer == 12)
            {
                transform.position = new Vector3(transform.position.x, 2.0f, transform.position.z);
                rb.constraints = RigidbodyConstraints.FreezePositionY;
                floatParticle.startSize = 0.3f;
                floatParticle.startLifetime = 3;
            }
            StartCoroutine(PowerUpTimer());
        }
    }
    IEnumerator PowerUpTimer() 
    {
        yield return new WaitForSeconds(3);
        transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
        playerGlow.intensity = 0.0f;
        rb.mass = 0.05f;
        rb.constraints = RigidbodyConstraints.None;
        floatParticle.startSize = 0;
        floatParticle.startLifetime = 0;
        if (gameObject.layer == 30)
        {
            playerController1.force = 1.0f;
        }
        if (gameObject.layer == 31)
        {
            playerController2.force = 1.0f;
        }
    }
}
