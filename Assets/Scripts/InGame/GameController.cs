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
    private bool paused = false;

    public GameObject tileObject;
    public GameObject gameOverPanel;

    public Transform playerTransform;

    public TMP_Text scoreText;
    
    private void Awake()
    {
        tileLength = GetTileLength();
        scoreText.color = Color.green;
    }

    private void FixedUpdate()
    {
        {
            //update score
            UpdateScore();
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
}
