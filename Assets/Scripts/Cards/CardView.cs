using DG.Tweening;
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

    private void ShowFront()
    {
        if (frontSprite == null)
        {
            Debug.LogError("CardView: frontImage not assigned!");
            return;
        }
        image.sprite = frontSprite;
    }

    private void ShowBack()
    {
        if (backSprite == null)
        {
            Debug.LogError("CardView: backImage not assigned!");
            return;
        }
        image.sprite = backSprite;
    }
    
    public void FlipToFront(float duration = 0.4f)
    {
        transform.DORotate(new Vector3(0, 90, 0), duration / 2).OnComplete(() =>
        {
            ShowFront();
            transform.DORotate(Vector3.zero, duration / 2);
        });
    }

    public void FlipToBack(float duration = 0.4f)
    {
        transform.DORotate(new Vector3(0, 90, 0), duration / 2).OnComplete(() =>
        {
            ShowBack();
            transform.DORotate(Vector3.zero, duration / 2);
        });
    }
    
    public void OnClick()
    {
        onCardClicked?.Invoke();
    }
}