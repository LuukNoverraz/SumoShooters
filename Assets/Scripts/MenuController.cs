using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

#pragma warning disable 649

public class MenuController : MonoBehaviour
{
    public GameController gameController;
    [SerializeField] private RectTransform mainMenu;
    [SerializeField] private RectTransform onlineMenu;
    [SerializeField] private RectTransform settingsMenu;
    [SerializeField] private RectTransform customization;
    [SerializeField] private GameObject players;
    [SerializeField] private RectTransform[] colors;
    [SerializeField] private RectTransform[] colorLocks;
    [SerializeField] private RectTransform sumoCoins;
    [SerializeField] private Text sumoText;
    [SerializeField] private RectTransform customizationBackButton;
    [SerializeField] private RectTransform madeByText;
    [SerializeField] private RectTransform background;
    private Vector3 offScreen = new Vector3(0, 9999, 0);
    
    void Start()
    {
        customization.sizeDelta = background.sizeDelta = new Vector2(Screen.width, Screen.height);
        players.transform.position = offScreen;
        sumoText.text = gameController.currentSumocoins.ToString();
        onlineMenu.localPosition = settingsMenu.localPosition = customization.localPosition = offScreen;
        madeByText.localPosition = new Vector3(0, -Screen.height + (Screen.height / 2) + 40, 0);
    }
    
    private void mainMenuOffScreen()
    {
        mainMenu.localPosition = offScreen;
    }

    private void mainMenuOnScreen()
    {
        mainMenu.localPosition = Vector3.zero;
    }

    public void LocalPlayButton()
    {
        SceneManager.LoadScene("LocalMultiplayer");
    }
    
    private void OnlinePlayButton()
    {
        mainMenuOffScreen();
        onlineMenu.localPosition = Vector3.zero;
    }

    private void CustomizationButton()
    {
        mainMenuOffScreen();
        customization.localPosition = Vector3.zero;
        players.transform.position = Vector3.zero;
        colors[0].localPosition = colorLocks[0].localPosition = new Vector3(0, (-Screen.height / 2) + (Screen.height / 10), 0);
        colors[0].sizeDelta = colorLocks[0].sizeDelta = new Vector2(Screen.width, 232.5f);
        colors[1].localPosition = colorLocks[1].localPosition = new Vector3(0, (Screen.height / 2) - (Screen.height / 10), 0);
        colors[1].sizeDelta = colorLocks[1].sizeDelta = new Vector2(Screen.width, 232.5f);
        sumoCoins.localPosition = new Vector2(((-Screen.width / 2) + 80), 0);
        customizationBackButton.localPosition = new Vector2(((Screen.width / 2) - 120), 0);
    }

    private void CustomizationBack()
    {
        mainMenuOnScreen();
        customization.localPosition = offScreen;
        players.transform.position = offScreen;
    }

    private void SettingsButton()
    {  
        mainMenuOffScreen();
        settingsMenu.localPosition = Vector3.zero;
    }

    private void SettingsBack()
    {  
        mainMenuOnScreen();
        settingsMenu.localPosition = offScreen;
    }
}