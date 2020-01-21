using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public GameController gameController;
    public RectTransform mainMenu;
    public RectTransform settingsMenu;
    public RectTransform customization;
    public GameObject players;
    public RectTransform[] colors;
    public RectTransform[] colorLocks;
    public RectTransform sumoCoins;
    public Text sumoText;
    public RectTransform customizationBackButton;
    public RectTransform madeByText;
    public RectTransform background;
    Vector3 offScreen = new Vector3(0, 9999, 0);
    
    void Start()
    {
        customization.sizeDelta = background.sizeDelta = new Vector2(Screen.width, Screen.height);
        players.transform.position = offScreen;
        sumoText.text = gameController.currentSumocoins.ToString();
        settingsMenu.localPosition = customization.localPosition = offScreen;
        madeByText.localPosition = new Vector3(0, -Screen.height + (Screen.height / 2) + 40, 0);
    }
    
    public void LocalPlayButton()
    {
        SceneManager.LoadScene("LocalMultiplayer");
    }
    
    public void CustomizationButton()
    {
        mainMenu.localPosition = offScreen;
        customization.localPosition = Vector3.zero;
        players.transform.position = Vector3.zero;
        colors[0].localPosition = colorLocks[0].localPosition = new Vector3(0, (-Screen.height / 2) + (Screen.height / 10), 0);
        colors[0].sizeDelta = colorLocks[0].sizeDelta = new Vector2(Screen.width, 232.5f);
        colors[1].localPosition = colorLocks[1].localPosition = new Vector3(0, (Screen.height / 2) - (Screen.height / 10), 0);
        colors[1].sizeDelta = colorLocks[1].sizeDelta = new Vector2(Screen.width, 232.5f);
        sumoCoins.localPosition = new Vector2(((-Screen.width / 2) + 80), 0);
        customizationBackButton.localPosition = new Vector2(((Screen.width / 2) - 120), 0);
    }

    public void BuyColor()
    {
        
    }

    public void CustomizationBack()
    {
        mainMenu.localPosition = Vector3.zero;
        customization.localPosition = offScreen;
        players.transform.position = offScreen;
    }

    public void SettingsButton()
    {  
        mainMenu.localPosition = offScreen;
        settingsMenu.localPosition = Vector3.zero;
    }

    public void SettingsBack()
    {  
        mainMenu.localPosition = Vector3.zero;
        settingsMenu.localPosition = offScreen;
    }
}