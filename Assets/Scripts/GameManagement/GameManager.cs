using UniRx;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private void Awake()
    {
        MessageBroker.Default.Receive<MainMenuMessage>().Subscribe(OnMainMenuMessage).AddTo(this);
    }
    
    private void OnMainMenuMessage(MainMenuMessage message)
    {
        // Restart the scene here
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);

    }
}
