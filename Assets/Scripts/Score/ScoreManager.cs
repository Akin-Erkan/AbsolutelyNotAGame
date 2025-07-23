using UniRx;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private int score;

    [SerializeField]
    private int negativeScore = -1;
    private int currentScoreCombo = 1;
    
    
    private void Start()
    {
        MessageBroker.Default.Receive<CorrectMatchMessage>().Subscribe(OnCorrectMatch).AddTo(this);
        MessageBroker.Default.Receive<WrongMatchMessage>().Subscribe(OnWrongMatch).AddTo(this);
        MessageBroker.Default.Receive<LoadedGameSaveDataMessage>().Subscribe(OnGameSaveDataLoaded).AddTo(this);
    }
    
    private void OnGameSaveDataLoaded(LoadedGameSaveDataMessage message)
    {
        if (message.GameSaveData != null)
        {
            score = message.GameSaveData.score;
            currentScoreCombo = 1; // Reset combo on game load
        }
    }
    
    private void OnCorrectMatch(CorrectMatchMessage message)
    {
        score += message.Card1.Data.Points * currentScoreCombo;
        currentScoreCombo++;
        MessageBroker.Default.Publish(new ScoreUpdateMessage(score));
    }
    
    private void OnWrongMatch(WrongMatchMessage message)
    {
        score += negativeScore;
        currentScoreCombo = 1; // Reset combo on wrong match
        MessageBroker.Default.Publish(new ScoreUpdateMessage(score));
    }
    
    

}
