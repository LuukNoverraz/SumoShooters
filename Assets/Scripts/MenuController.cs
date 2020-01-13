using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{

    public RectTransform MainMenu;
    public RectTransform SettingsMenu;
    public RectTransform LevelSelection;
    public RectTransform Customization;
    public RectTransform MadeByText;
    public RectTransform Background;
    Vector3 offScreen = new Vector3(0, -Screen.height, 0);
    
    void Start()
    {
        Customization.sizeDelta = Background.sizeDelta = new Vector2(Screen.width, Screen.height);
        SettingsMenu.localPosition = LevelSelection.localPosition = Customization.localPosition = offScreen;
        MadeByText.localPosition = new Vector3(0, -Screen.height + (Screen.height / 2) + 30, 0);
    }
    
    public void LocalPlayButton()
    {
        SceneManager.LoadScene("LocalMultiplayer");
    }
    
    public void CustomizationButton()
    {
        MainMenu.localPosition = offScreen;
        Customization.localPosition = Vector3.zero;
    }

    public void CustomizationBack()
    {
        MainMenu.localPosition = Vector3.zero;
        Customization.localPosition = offScreen;
    }

    public void SettingsButton()
    {  
        MainMenu.localPosition = offScreen;
        SettingsMenu.localPosition = Vector3.zero;
    }

    public void SettingsBack()
    {  
        MainMenu.localPosition = Vector3.zero;
        SettingsMenu.localPosition = offScreen;
    }
}