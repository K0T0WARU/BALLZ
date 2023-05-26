using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEditor;
using TMPro;

public class MenuUIHandler : MonoBehaviour
{
    //This is the handler of the main menu scene

    [SerializeField] TextMeshProUGUI PlayerNameInput;
    public TMP_InputField nameInputField;
    private int charecterLimit = 10;

    private void Start()
    {
        nameInputField.characterLimit = charecterLimit;
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void SetPlayerName()
    {
        PlayerDataHandle.Instance.PlayerName = PlayerNameInput.text;
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
}