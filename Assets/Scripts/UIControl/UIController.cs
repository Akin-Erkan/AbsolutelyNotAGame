using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

[DefaultExecutionOrder(-1000)] 
public class UIController : MonoBehaviour
{

    [SerializeField] 
    private GameObject startPanel;
    [SerializeField] 
    private GameObject gamePanel;
    [SerializeField]
    private GameObject endPanel;
    
    [SerializeField]
    private TextMeshProUGUI scorePointText;
    [SerializeField] 
    private TextMeshProUGUI matchesText;
    [SerializeField]
    private TextMeshProUGUI turnsText;
    
    [SerializeField]
    private Button newGameButton;
    [SerializeField]
    private Button loadGameButton;
    [SerializeField]
    private Button mainMenuButton;
    
    
    
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
        
        MessageBroker.Default.Receive<LoadedGameSaveDataMessage>()
            .Subscribe(OnGameSaveDataLoaded)
            .AddTo(this);
        
        newGameButton.onClick.AddListener(() =>
        {
            startPanel.SetActive(false);
            gamePanel.SetActive(true);
            endPanel.SetActive(false);
            MessageBroker.Default.Publish(new NewGameMessage());
        });
        
        loadGameButton.onClick.AddListener(() =>
        {
            startPanel.SetActive(false);
            gamePanel.SetActive(true);
            endPanel.SetActive(false);
            MessageBroker.Default.Publish(new LoadGameMessage());
        });
        
        mainMenuButton.onClick.AddListener(() =>
        {
            MessageBroker.Default.Publish(new MainMenuMessage());
        });
        
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
    
    private void OnGameSaveDataLoaded(LoadedGameSaveDataMessage message)
    {
        print("Game Save Data Loaded in UIController");
        if (message.GameSaveData != null)
        {
            scorePointText.text = message.GameSaveData.score.ToString();
            matchesText.text = message.GameSaveData.matches.ToString();
            turnsText.text = message.GameSaveData.turns.ToString();
        }
    }


}
