using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public GameObject GameOverText;
    
    private bool m_Started = false;
    private int m_Points;
    
    private bool m_GameOver = false;

    //Fields for player name and best score
    public Text currentPlayerName;
    public Text bestPlayerName;

    // Start is called before the first frame update
    void Start()
    {
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed += AddPoint;
            }
        }

        currentPlayerName.text = MyPlayerDataHandler.Instance.playerName;
        SetBestPlayer();
    }
    private void CheckBestPlayer()
    {
        int currentScore = MyPlayerDataHandler.Instance.score;

        if (currentScore > MyPlayerDataHandler.Instance.bestScore)
        {
            MyPlayerDataHandler.Instance.bestPlayer = MyPlayerDataHandler.Instance.playerName;
            MyPlayerDataHandler.Instance.bestScore = currentScore;

            bestPlayerName.text = $"Best Score: {MyPlayerDataHandler.Instance.bestPlayer} : " +
                                             $"{MyPlayerDataHandler.Instance.bestScore}";

            MyPlayerDataHandler.Instance.SaveGame(MyPlayerDataHandler.Instance.bestPlayer, 
                                            MyPlayerDataHandler.Instance.bestScore);
        }
    }
    public void SetBestPlayer()
    {
        if (MyPlayerDataHandler.Instance.bestPlayer == null && MyPlayerDataHandler.Instance.bestScore == 0)
        {
            bestPlayerName.text = "";
        }
        else
        {
            bestPlayerName.text = $"Best Score: {MyPlayerDataHandler.Instance.bestPlayer} : " +
                                    $"{MyPlayerDataHandler.Instance.bestScore}";
        }
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = UnityEngine.Random.Range(-0.5f, 0.5f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        MyPlayerDataHandler.Instance.score = m_Points;
        ScoreText.text = $"Score : {m_Points}";
    }

    public void GameOver()
    {
        m_GameOver = true;
        CheckBestPlayer();
        GameOverText.SetActive(true);
    }      
}
