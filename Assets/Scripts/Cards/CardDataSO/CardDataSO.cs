using UnityEngine;

// Firstly though, we can use ScriptableObject to store card data in a more Unity-friendly way. But this is not a dynamic solution. So we switched to a more dynamic approach using CardData class.
[CreateAssetMenu(menuName = "Card Data", fileName = "NewCardData")]
public class CardDataSO : ScriptableObject
{
    public string Name;
    public int CardId;
    public int Points;
    public Sprite FrontSprite;
}