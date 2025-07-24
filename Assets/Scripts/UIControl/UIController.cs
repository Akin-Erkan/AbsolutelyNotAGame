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
    private TextMeshProUGUI endgameScoreText;
    
    [SerializeField]
    private Button newGameButton;
    [SerializeField]
    private Button loadGameButton;
    [SerializeField]
    private Button mainMenuButton;
    [SerializeField]
    private Button endGameContinueButton;
    
    
    
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

        MessageBroker.Default.Receive<GameOverMessage>().Subscribe(OnGameOverMessage).AddTo(this);
        
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
        
        endGameContinueButton.onClick.AddListener(() =>
        {
            MessageBroker.Default.Publish(new EndGameNewGameMessage());
        });
    }
    
    private void OnScoreUpdate(ScoreUpdateMessage message)
    {
        scorePointText.text = message.Score.ToString();
    }
    
    private void OnGameOverMessage(GameOverMessage message)
    {
        startPanel.SetActive(false);
        gamePanel.SetActive(false);
        endPanel.SetActive(true);
        endgameScoreText.text = scorePointText.text;
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
        else
        {
            loadGameButton.gameObject.SetActive(false);
        }
    }


}
