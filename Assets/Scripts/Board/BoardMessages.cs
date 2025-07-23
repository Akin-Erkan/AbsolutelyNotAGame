using System.Collections.Generic;

public class CurrentBoardStateMessage
{
    public List<CardController> CardControllers { get; }
    
    public CurrentBoardStateMessage(List<CardController> controllers)
    {
        CardControllers = controllers;
    }
}

public class CorrectMatchMessage
{
    public CardController Card1 { get; }
    public CardController Card2 { get; }

    public CorrectMatchMessage(CardController card1, CardController card2)
    {
        Card1 = card1;
        Card2 = card2;
    }
}

public class WrongMatchMessage
{
    public CardController Card1 { get; }
    public CardController Card2 { get; }

    public WrongMatchMessage(CardController card1, CardController card2)
    {
        Card1 = card1;
        Card2 = card2;
    }
}