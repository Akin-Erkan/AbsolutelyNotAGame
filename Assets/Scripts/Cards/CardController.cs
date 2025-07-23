public class CardController : ICardController
{
    public CardData Data { get; private set; }
    public CardView View { get; private set; }
    public bool IsMatched { get; private set; }

    private System.Action<CardController> onCardSelected;
    
    public CardController(CardData data, CardView view, System.Action<CardController> onCardSelected)
    {
        Data = data;
        View = view;
        this.onCardSelected = onCardSelected;
        View.onCardClicked.AddListener(OnClick);
        View.FlipToBack();
    }

    public void OnClick()
    {
        if (IsMatched) 
            return;
        onCardSelected?.Invoke(this);
    }

    public void Reveal()
    {
        View.FlipToFront();
    }

    public void Hide()
    {
        View.FlipToBack();
    }

    public void SetMatched()
    {
        IsMatched = true;
        // Matched animasyonu vs.
    }
}