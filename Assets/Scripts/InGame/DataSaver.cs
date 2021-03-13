using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DataSaver
{
    public int highScore;

    public DataSaver(GameController gameController)
    {
        highScore = gameController.highScore;
    }
}
