using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    private int score;
    private float tileLength;
    private float lastScoreSpeedIncrease = 0f;
    private bool paused = false;

    public int highScore;
    public float speedIncrement = 1f;

    public GameObject tileObject;
    public GameObject gameOverPanel;

    public Transform playerTransform;

    public PlayerController playerController;
    public TileManager tileManager;

    public TMP_Text scoreText;
    public TMP_Text highScoreText;
    
    private void Awake()
    {
        tileLength = GetTileLength();
        scoreText.color = Color.green;
        LoadGame();
    }

    private void FixedUpdate()
    {
        {
            //update score
            UpdateScore();

            if (score % 20 == 0 && score > lastScoreSpeedIncrease)
            {
                playerController.IncreaseSpeed(speedIncrement);
                lastScoreSpeedIncrease = speedIncrement;
                tileManager.IncreaseCactusBreak();
            }
        }
    }

    public void PauseButtonPressed()
    {
        if (!paused)
        {
            PauseGame();
        }
        else
        {
            ResumeGame();
        }
    }

    void UpdateScore()
    {
        //get & set score
        score = (int) playerTransform.position.x / (int) tileLength;
        scoreText.SetText(score.ToString());
    }

    public void UpdateHighScore()
    {
        if (highScore < score)
            highScore = score;
        
        highScoreText.SetText("HIGHSCORE \n" + highScore.ToString());
    }
    

    int GetTileLength()
    {
        var length = (int)(tileObject.GetComponent<BoxCollider2D>().size.x *
                           tileObject.transform.localScale.x);
        
        return length;
    }
    
    
    void PauseGame()
    {
        Time.timeScale = 0f;
        paused = true;
    }

    void ResumeGame()
    {
        Time.timeScale = 1f;
        paused = false;
    }

    public void RestartRun()
    {
        SceneManager.LoadScene("GameScene");
        ToggleGameOverPanel(false);
    }

    public void ToggleGameOverPanel(bool activate = true)
    {
        gameOverPanel.SetActive(activate);
    }

    public void SaveGame()
    {
        SaveSystem.SaveData(this);
    }

    private void LoadGame()
    {
        DataSaver data = SaveSystem.LoadData();

        highScore = data.highScore;
    }
}
