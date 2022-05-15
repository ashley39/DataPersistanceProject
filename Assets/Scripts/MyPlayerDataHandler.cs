using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class MyPlayerDataHandler : MonoBehaviour
{
    public static MyPlayerDataHandler Instance;

    public string playerName;
    public int score;
    [SerializeField] Text menuScore; //high score displayed on menu screen

    public int bestScore;
    public string bestPlayer;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        LoadGame();
        menuScore.text = $"Best Score: {bestPlayer} : {bestScore}";
    }

    [Serializable]
    class SaveData
    {
        public int highestScore;
        public string theBestPlayer;
    }

    public void SaveGame(string bestPlayerName, int bestPlayerScore)
    {
        SaveData data = new SaveData();

        data.theBestPlayer = bestPlayerName;
        data.highestScore = bestPlayerScore;

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void LoadGame()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            bestPlayer = data.theBestPlayer;
            bestScore = data.highestScore;
        }
    }
}
