
// In case of loading contents dynamically, we are using bare CardData class to load data from outside the project.
[System.Serializable]
public class CardData
{
    public int CardId;
    public int Points;
    public string Name;
    public string SpritePathOrURL;


    public CardData(int cardId, int points, string name)
    {
        CardId = cardId;
        Points = points;
        Name = name;
    }
}
