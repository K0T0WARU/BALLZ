using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI roundNumber;
    public TextMeshProUGUI scoreNumber;
    public TextMeshProUGUI buffCounter;

    private SpawnManager spawnManager;
    private PlayerController player;

    public int score;
    public bool isGameActive;

    public GameObject gameOverMenu;

    private static int BestScore;
    private static string BestPlayer;

    private void Awake()
    {
        LoadDataHandle();
    }

    void Start()
    {
        isGameActive = true;
        spawnManager = FindObjectOfType<SpawnManager>();
        player = FindObjectOfType<PlayerController>();
        UpdateScore();
        UpdateBuff();
        UpdateRound();
    }

    public void UpdateRound()
    {
        roundNumber.text = $"Round: {spawnManager.waveNumber}";
    }

    public void UpdateScore()
    {
        scoreNumber.text = $"Score: {score}";
        PlayerDataHandle.Instance.Score = score;
    }

    public void UpdateBuff()
    {
        switch (player.currentPowerUp)
        {
            case PowerUpType.Pushback:
                buffCounter.text = "Buff: Pushback";
                break;
            case PowerUpType.Bullets:
                buffCounter.text = "Buff: Bullets";
                break;
            case PowerUpType.Smash:
                buffCounter.text = "Buff: Smash";
                break;
            default:
                buffCounter.text = "Buff: None";
                break;
        }
    }

    public void GameOver()
    {
        isGameActive = false;
        gameOverMenu.SetActive(true);
        CheckBestPlayer();
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void Restart()
    {
        SceneManager.LoadScene(1);
    }

    private void LoadDataHandle()
    {
        string path = Application.persistentDataPath + "/savefile.json";

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            BestPlayer = data.TheBestPlayer;
            BestScore = data.HighiestScore;
        }
    }

    private void CheckBestPlayer()
    {
        int CurrentScore = PlayerDataHandle.Instance.Score;

        if (CurrentScore > BestScore)
        {
            BestPlayer = PlayerDataHandle.Instance.PlayerName;
            BestScore = CurrentScore;

            SaveGameRank(BestPlayer, BestScore);
        }
    }

    public void SaveGameRank(string bestPlaterName, int bestPlayerScore)
    {
        SaveData data = new SaveData();

        data.TheBestPlayer = bestPlaterName;
        data.HighiestScore = bestPlayerScore;

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    [System.Serializable]
    class SaveData
    {
        public int HighiestScore;
        public string TheBestPlayer;
    }
}
    
