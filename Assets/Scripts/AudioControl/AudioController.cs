using UniRx;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    [SerializeField]
    private AudioSource audioSource;
    
    [SerializeField]
    private AudioClip cardMatchClip;
    [SerializeField]
    private AudioClip missMatchClip;
    [SerializeField] 
    private AudioClip cardFlipClip;
    [SerializeField]
    private AudioClip endGameClip;
    
    
    void Start()
    {
        MessageBroker.Default.Receive<CorrectMatchMessage>().Subscribe(OnCorrectMatch).AddTo(this);
        MessageBroker.Default.Receive<WrongMatchMessage>().Subscribe(OnMissMatch).AddTo(this);
        MessageBroker.Default.Receive<CardSelectedMessage>().Subscribe(OnCardFlip).AddTo(this);
        MessageBroker.Default.Receive<EndGameScoreMessage>().Subscribe(OnEndGame).AddTo(this);
    }
    
    private void OnCorrectMatch(CorrectMatchMessage message)
    {
        audioSource.PlayOneShot(cardMatchClip);
    }
    private void OnMissMatch(WrongMatchMessage message)
    {
        audioSource.PlayOneShot(missMatchClip);
    }
    private void OnCardFlip(CardSelectedMessage message)
    {
        audioSource.PlayOneShot(cardFlipClip);
    }
    private void OnEndGame(EndGameScoreMessage message)
    {
        audioSource.PlayOneShot(endGameClip);
    }


}
