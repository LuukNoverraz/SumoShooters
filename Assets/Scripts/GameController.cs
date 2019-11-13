using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public Transform mainLightTransform;
    private float randomYRotation;
    private float randomPowerUpSpawnX;
    private float randomPowerUpSpawnZ;
    public RectTransform borderTransform;
    public RectTransform[] scoreTexts;
    public GameObject powerUpPrefab;
    public Text redText;
    public Text blueText;
    public Text redWin;
    public Text blueWin;
    public Text[] restartText;
    public static int redScore = 0;
    public static int blueScore = 0;
    public float[] colorList;

    void Start()
    {
        randomYRotation = Random.Range(0.0f, 360.0f);
        mainLightTransform.rotation = Quaternion.Euler(new Vector3(50.0f, Mathf.Floor(randomYRotation), 0));
        borderTransform.sizeDelta = new Vector2(Screen.width, 10.0f);
        scoreTexts[0].sizeDelta = new Vector2(Screen.width - 20.0f, 30.0f);
        scoreTexts[1].sizeDelta = new Vector2(Screen.width - 100.0f, 30.0f);
        StartCoroutine(PowerUpSpawnTimer());
    }
    void Update()
    {
        redText.text = "SCORE: " + redScore;
        if (redScore >= 3)
        {
            redWin.text = blueWin.text = "RED WINS";
            redWin.color = blueWin.color = new Color(colorList[0], colorList[1], colorList[2], 1.0f);
            restartText[0].color = restartText[1].color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            Time.timeScale = 0;
            if (Input.GetMouseButtonDown(0))
            {
                Time.timeScale = 1;
                SceneManager.LoadScene("LocalMultiplayer");
            }
        }
        blueText.text = "SCORE: " + blueScore;
        if (blueScore >= 3)
        {
            redWin.text = blueWin.text = "BLUE WINS";
            redWin.color = blueWin.color = new Color(colorList[3], colorList[4], colorList[5], 1.0f);
            restartText[0].color = restartText[1].color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            Time.timeScale = 0;
            if (Input.GetMouseButtonDown(0))
            {
                Time.timeScale = 1;
                SceneManager.LoadScene("LocalMultiplayer");
            }
        }
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
