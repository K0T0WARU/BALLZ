using UnityEngine;
using System.IO;
using TMPro;

public class LoadDataHandle : MonoBehaviour
{
    public TextMeshProUGUI BestPlayerName;

    private static int BestScore;
    private static string BestPlayer;

    private void Awake()
    {
        LoadGameRank();
    }

    private void LoadGameRank()
    {
        string path = Application.persistentDataPath + "/savefile.json";

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            BestPlayer = data.TheBestPlayer;
            BestScore = data.HighiestScore;
            SetBestPlayer();
        } else
            BestPlayerName.text = "No one played";
    }

    private void SetBestPlayer()
    {
        if (BestPlayer == null || BestScore == 0)
        {
            BestPlayerName.text = "Error";
        }
        else
        {
            BestPlayerName.text = $"Best Play - {BestPlayer}: {BestScore}";
        }
    }

    [System.Serializable]
    class SaveData
    {
        public int HighiestScore;
        public string TheBestPlayer;
    }
}
