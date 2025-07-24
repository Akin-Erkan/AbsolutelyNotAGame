using UniRx;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private void Awake()
    {
        MessageBroker.Default.Receive<MainMenuMessage>().Subscribe(OnMainMenuMessage).AddTo(this);
        MessageBroker.Default.Receive<EndGameScoreMessage>().Subscribe(OnEndGameScoreMessage).AddTo(this);
        MessageBroker.Default.Receive<EndGameNewGameMessage>().Subscribe(OnEndGameNewGameMessage).AddTo(this);
    }
    
    private void OnMainMenuMessage(MainMenuMessage message)
    {
        // Restart the scene here
        LoadMainMenu();
    }
    
    
    private void LoadMainMenu()
    {
        // Load the main menu scene
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
    
    private void OnEndGameNewGameMessage(EndGameNewGameMessage message)
    {
        PlayerPrefs.DeleteAll();
        LoadMainMenu();
    }
    
    private void OnEndGameScoreMessage(EndGameScoreMessage message)
    {
        PlayerPrefs.DeleteAll();
        MessageBroker.Default.Publish(new GameOverMessage(message.FinalScore));
    }
    

    
}
