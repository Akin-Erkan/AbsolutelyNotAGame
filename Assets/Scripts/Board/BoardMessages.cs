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
    public int totalCorrectMatches;
    public int totalAttempts;

    public CorrectMatchMessage(CardController card1, CardController card2,int totalCorrectMatches,int totalAttempts)
    {
        Card1 = card1;
        Card2 = card2;
        this.totalCorrectMatches = totalCorrectMatches;
        this.totalAttempts = totalAttempts;
    }
}

public class WrongMatchMessage
{
    public CardController Card1 { get; }
    public CardController Card2 { get; }
    public int totalAttempts;

    public WrongMatchMessage(CardController card1, CardController card2, int totalAttempts)
    {
        Card1 = card1;
        Card2 = card2;
        this.totalAttempts = totalAttempts;
    }
}

public class AllCardsMatchedMessage
{
    public List<CardController> MatchedCards { get; }

    public AllCardsMatchedMessage(List<CardController> matchedCards)
    {
        MatchedCards = matchedCards;
    }
}

public class CardSelectedMessage
{
    public CardController SelectedCard { get; }

    public CardSelectedMessage(CardController selectedCard)
    {
        SelectedCard = selectedCard;
    }
}