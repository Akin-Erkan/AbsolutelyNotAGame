public class NewGameMessage
{
    
}

public class EndGameNewGameMessage
{
    
}

public class LoadGameMessage
{
    
}

public class MainMenuMessage
{
    
}

public class GameOverMessage
{
    public int FinalScore { get; }

    public GameOverMessage(int finalScore)
    {
        FinalScore = finalScore;
    }
}