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
