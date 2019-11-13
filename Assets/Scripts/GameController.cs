using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public Transform mainLightTransform;
    private float randomYRotation;
    private float randomPowerUpSpawnX;
    private float randomPowerUpSpawnZ;
    public RectTransform borderTransform;
    public RectTransform[] scoreTexts;
    public GameObject powerUpPrefab;

    void Start()
    {
        randomYRotation = Random.Range(0.0f, 360.0f);
        mainLightTransform.rotation = Quaternion.Euler(new Vector3(50.0f, Mathf.Floor(randomYRotation), 0));
        borderTransform.sizeDelta = new Vector2(Screen.width, 10.0f);
        scoreTexts[0].sizeDelta = new Vector2(Screen.width - 20.0f, 30.0f);
        scoreTexts[1].sizeDelta = new Vector2(Screen.width - 100.0f, 30.0f);
        StartCoroutine(PowerUpSpawnTimer());
    }
    IEnumerator PowerUpSpawnTimer()
    {
        yield return new WaitForSeconds(Random.Range(4, 10));
        if (GameObject.FindWithTag("PowerUp") == null) 
        { 
            randomPowerUpSpawnX = Random.Range(-3.0f, 3.0f);
            randomPowerUpSpawnZ = Random.Range(-3.0f, 3.0f);
            Instantiate(powerUpPrefab, new Vector3(randomPowerUpSpawnX, 2.0f, randomPowerUpSpawnZ), transform.rotation);
        }
    }
}
