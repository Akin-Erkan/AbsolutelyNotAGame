using System.Collections.Generic;

[System.Serializable]
public class GameSaveData
{
    public List<CardController> cardControllers;
    public int score;
    public int turns;
}