using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

#pragma warning disable 649

public class PlayerNameInput : MonoBehaviour
{
    [SerializeField] private InputField nameInputField = null;
    [SerializeField] private Button continueButton = null;

    private const string PlayerPrefsNameKey = "PlayerName";

    void Start() => SetUpInputField();

    private void SetUpInputField()
    {
        string defaultName = PlayerPrefs.GetString(PlayerPrefsNameKey, null);

        nameInputField.text = defaultName;

        SetPlayerName(defaultName);
    }

    public void SetPlayerName(string name)
    {
        name = nameInputField.text;
        continueButton.interactable = !string.IsNullOrEmpty(name);
    }

    public void SavePlayerName()
    {
        string playerName = nameInputField.text;

        PhotonNetwork.NickName = playerName;

        PlayerPrefs.SetString(PlayerPrefsNameKey, playerName);
    }
}
