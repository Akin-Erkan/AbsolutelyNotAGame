using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class CardView : MonoBehaviour
{
    public Image image;
    public Sprite frontSprite;
    public Sprite backSprite;

    public UnityEvent onCardClicked;

    public void SetFront(Sprite sprite)
    {
        frontSprite = sprite;
        frontSprite.name = sprite.name;
    }

    public void ShowFront()
    {
        if (frontSprite == null)
        {
            Debug.LogError("CardView: frontImage not assigned!");
            return;
        }
        image.sprite = frontSprite;

    }

    public void ShowBack()
    {
        if (backSprite == null)
        {
            Debug.LogError("CardView: backImage not assigned!");
            return;
        }
        image.sprite = backSprite;
    }
    
    public void OnClick() // Button veya EventTrigger'dan çağrılmalı
    {
        onCardClicked?.Invoke();
    }
}