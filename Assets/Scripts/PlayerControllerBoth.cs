using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerBoth : MonoBehaviour
{
    public Light playerGlow;
    public ParticleSystem floatParticle;
    public ParticleSystem nuclearExplosion;
    public Rigidbody rb;
    public GameObject player1;
    public GameObject player2;
    public PlayerController1 playerController1;
    public PlayerController2 playerController2;
    public Renderer renderer;
    public ColorPicker colorPicker;
    public float multiplier;
    
    void Start()
    {
        playerController1 = GameObject.Find("Player 1").GetComponent<PlayerController1>();
        playerController2 = GameObject.Find("Player 2").GetComponent<PlayerController2>();
        colorPicker.onValueChanged.AddListener(color =>
        {
            renderer.material.color = color;
        });
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
                    player1.GetComponent<PlayerController1>().force = 20.0f;
                }
                if (gameObject.layer == 31)
                {
                    player2.GetComponent<PlayerController2>().force = 20.0f;
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
            player1.GetComponent<PlayerController1>().force = 1.0f;
        }
        if (gameObject.layer == 31)
        {
            player2.GetComponent<PlayerController2>().force = 1.0f;
        }
    }
}
