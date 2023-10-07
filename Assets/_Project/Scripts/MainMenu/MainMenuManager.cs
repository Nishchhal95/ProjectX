using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuUIContainer;
    [SerializeField] private TMP_InputField userNameInputField;
    private string USERNAME_KEY = "USERNAME";

    private void Start()
    {
        userNameInputField.text = PlayerPrefs.GetString(USERNAME_KEY, "");
    }

    public void OnJoinButtonClick()
    {
        ValidateAndSaveUsername();
        NetworkManager.Singleton.StartClient();
        mainMenuUIContainer.SetActive(false);
    }

    public void OnHostButtonClick()
    {
        ValidateAndSaveUsername();
        NetworkManager.Singleton.StartHost();
        mainMenuUIContainer.SetActive(false);
    }

    private void ValidateAndSaveUsername()
    {
        string userName = userNameInputField.text;
        if (string.IsNullOrEmpty(userName))
        {
            Debug.LogError("Please enter a username to join or host!");
            return;
        }
        PlayerPrefs.SetString(USERNAME_KEY, userName);
    }
}
