using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ScoreUpdateMessage
{
    public int Score { get; }

    public ScoreUpdateMessage(int score)
    {
        Score = score;
    }
}

public class HighScoreUpdateMessage
{
    public int HighScore { get; }

    public HighScoreUpdateMessage(int highScore)
    {
        HighScore = highScore;
    }
}

public class EndGameScoreMessage
{
    public int FinalScore { get; }
    
    public EndGameScoreMessage(int finalScore)
    {
        FinalScore = finalScore;

    }
}

