using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class MyPlayerDataHandler : MonoBehaviour
{
    public static MyPlayerDataHandler Instance;
    MainManager manager;
    MyMenuUI_Handler handler;

    public string playerName;
    public int score;
    private int bestScore;
    private string bestPlayer;

    private void Awake()
    {
        LoadGame();
        GameObject.Find("BestScore").GetComponentInChildren<Text>().text = $"Best Score: {bestPlayer} : {bestScore}";

        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    public void SetBestPlayer()
    {
        if (bestPlayer == null && bestScore == 0)
        {
            GameObject.Find("MainManager").
                GetComponent<MainManager>().bestPlayerName.text = "";
        }
        else
        {
            Debug.Log("Best Player");
            GameObject.Find("MainManager").
                GetComponent<MainManager>().bestPlayerName.text = $"Best Score: {bestPlayer} : {bestScore}";
        }
    }

    public void CheckBestPlayer()
    {
        int currentScore = MyPlayerDataHandler.Instance.score;

        if (currentScore > bestScore)
        {
            bestPlayer = MyPlayerDataHandler.Instance.playerName;
            bestScore = currentScore;

            GameObject.Find("MainManager").
                GetComponent<MainManager>().bestPlayerName.text = $"Best Score: {bestPlayer} : {bestScore}";

            SaveGame(bestPlayer, bestScore);
        }
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
