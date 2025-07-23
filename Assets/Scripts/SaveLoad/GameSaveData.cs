using System.Collections.Generic;

[System.Serializable]
public class GameSaveData
{
    public List<CardData> cardDatas;
    public List<int> matchedCardIds = new List<int>(); 
    public int score;
    public int turns;
    public int matches;
    
}