using System.Linq;
using UniRx;
using UnityEngine;

[DefaultExecutionOrder(1000)]
public class SaveLoadManager : MonoBehaviour
{
    //TODO: we are using PlayerPrefs for simplicity, but we can use a more robust solution like a database or cloud storage for saving game data later with json after integrating user info etc.
    private GameSaveData gameSaveData;

    private bool isLoadingSavedGame = false;
    
    private void Start()
    {
        MessageBroker.Default.Receive<CorrectMatchMessage>().Subscribe(OnCorrectMatch).AddTo(this);
        MessageBroker.Default.Receive<WrongMatchMessage>().Subscribe(OnWrongMatch).AddTo(this);
        MessageBroker.Default.Receive<CurrentBoardStateMessage>().Subscribe(OnCurrentBoardState).AddTo(this);
        MessageBroker.Default.Receive<ScoreUpdateMessage>().Subscribe(OnScoreUpdate).AddTo(this);
        MessageBroker.Default.Receive<NewGameMessage>().Subscribe(OnNewGame).AddTo(this);
        MessageBroker.Default.Receive<LoadGameMessage>().Subscribe(OnLoadGame).AddTo(this);
    }
    
    private void OnNewGame(NewGameMessage message)
    {
        if (isLoadingSavedGame)
        {
            isLoadingSavedGame = false;
            return;
        }
        ClearGameSaveData();
    }
    
    private void OnLoadGame(LoadGameMessage message)
    {
        isLoadingSavedGame = true;
        LoadGameSaveData();
    }
    
    public void LoadGameSaveData()
    {
        gameSaveData = LoadGamePlayPrefs();
        MessageBroker.Default.Publish(new LoadedGameSaveDataMessage(gameSaveData));
    }

    private void ClearGameSaveData()
    {
        PlayerPrefs.DeleteAll();
    }
    
    private void OnCurrentBoardState(CurrentBoardStateMessage message)
    {
        if (gameSaveData == null)
        {
            gameSaveData = new GameSaveData();
        }
        var cardDatas = message.CardControllers.ConvertAll(controller => controller.Data);
        gameSaveData.cardDatas = cardDatas;
        gameSaveData.matchedCardIds = message.CardControllers
            .Where(controller => controller.IsMatched)
            .Select(controller => controller.Data.CardId)
            .ToList();
        SaveGamePlayPrefs(gameSaveData);
    }
    
    private void OnCorrectMatch(CorrectMatchMessage message)
    {
        if (gameSaveData == null)
        {
            gameSaveData = new GameSaveData();
        }
        gameSaveData.matches = message.totalCorrectMatches;
        gameSaveData.turns = message.totalAttempts;
        SaveGamePlayPrefs(gameSaveData);
    }
    
    private void OnWrongMatch(WrongMatchMessage message)
    {
        if (gameSaveData == null)
        {
            gameSaveData = new GameSaveData();
        }
        gameSaveData.turns = message.totalAttempts;
        SaveGamePlayPrefs(gameSaveData);
    }
    
    private void OnScoreUpdate(ScoreUpdateMessage message)
    {
        if (gameSaveData == null)
        {
            gameSaveData = new GameSaveData();
        }
        gameSaveData.score = message.Score;
        SaveGamePlayPrefs(gameSaveData);
    }


    //TODO: either use these methods or create custom serialization methods to save json online
    public void SaveGamePlayPrefs(GameSaveData data)
    {
        print("Saving Game Play Preferences");
        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString("save_game", json); 
        PlayerPrefs.Save();
    }
    
    public GameSaveData LoadGamePlayPrefs()
    {
        if (!PlayerPrefs.HasKey("save_game")) 
            return null;
        string json = PlayerPrefs.GetString("save_game");
        return JsonUtility.FromJson<GameSaveData>(json);
    }
    
}

public class LoadedGameSaveDataMessage
{
    public GameSaveData GameSaveData { get; }

    public LoadedGameSaveDataMessage(GameSaveData gameSaveData)
    {
        GameSaveData = gameSaveData;
    }
}
