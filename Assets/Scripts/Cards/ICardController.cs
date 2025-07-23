public interface ICardController
{
    CardData Data { get; }
    CardView View { get; }
    bool IsMatched { get; }
    void OnClick();
    void Reveal();
    void Hide();
    void SetMatched();
}