using TMPro;
using UniRx;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI scorePointText;
    [SerializeField] 
    private TextMeshProUGUI matchesText;
    [SerializeField]
    private TextMeshProUGUI turnsText;
    
    
    
    void Start()
    {
        MessageBroker.Default.Receive<ScoreUpdateMessage>()
            .Subscribe(OnScoreUpdate)
            .AddTo(this);
        
        MessageBroker.Default.Receive<CorrectMatchMessage>()
            .Subscribe(OnCorrectMatchMessage)
            .AddTo(this);
        
        MessageBroker.Default.Receive<WrongMatchMessage>()
            .Subscribe(OnWrongMatchMessage)
            .AddTo(this);
        
    }
    
    private void OnScoreUpdate(ScoreUpdateMessage message)
    {
        scorePointText.text = message.Score.ToString();
    }
    
    private void OnCorrectMatchMessage(CorrectMatchMessage message)
    {
        matchesText.text = message.totalCorrectMatches.ToString();
        turnsText.text = message.totalAttempts.ToString();
    }
    
    private void OnWrongMatchMessage(WrongMatchMessage message)
    {
        turnsText.text = message.totalAttempts.ToString();
    }


}
