using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataHandle : MonoBehaviour
{
    //Static Class for save the current player data;
    //Singleton pattern

    public static PlayerDataHandle Instance;

    public string PlayerName;

    public int Score;

    public float soundVolume;

    private void Awake()
    {
        //we dont actually need this if statement because when we are in the Main Scene we cant return
        //to the Start Menu Scene where the Player Data Handle Object already exists.
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}