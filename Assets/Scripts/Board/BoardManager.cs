using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour
{
    public GridLayoutGroup gridParent;
    public GameObject cardViewPrefab;
    public int gridSizeX = 4;
    public int gridSizeY = 4;
    private List<CardData> allCardData;

    private List<CardController> controllers = new List<CardController>();
    private List<CardController> selectedCards = new List<CardController>();
    
    private int totalAttempts = 0;
    
    private bool canPlay = false;
    
    private List<int> matchedCardIds = new List<int>(); 


    private void Awake()
    {
        MessageBroker.Default.Receive<LoadedGameSaveDataMessage>()
            .Subscribe(OnGameSaveDataLoaded)
            .AddTo(this);
        MessageBroker.Default.Receive<NewGameMessage>()
            .Subscribe(_ => StartNewGame())
            .AddTo(this);
    }

    private void StartNewGame()
    {
        matchedCardIds.Clear();
        totalAttempts = 0;
        StartCoroutine(LoadCardsOnline());
    }


    private IEnumerator LoadCardsOnline()
    {
        string url = "https://raw.githubusercontent.com/Akin-Erkan/TouristGuideAIAssetBundle/main/Cards.json";
        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("JSON couldn't be loaded: " + www.error);
                yield break;
            }

            string json = www.downloadHandler.text;
            allCardData = JsonHelper.FromJson<CardData>(json).ToList();
            totalAttempts = 0;
            Debug.Log($"JSON Downloaded. Card Count: {allCardData.Count}");
        }

        SetupBoard();
    }
    
    private void LoadCardsFromLocal()
    {
        for (int i = 0; i < matchedCardIds.Count; i++)
        {
            var mathchedCards = controllers.FindAll(c => c.Data.CardId == matchedCardIds[i]);
            foreach (var c in mathchedCards)
            {
                c.SetMatched();
            }
        }
        SetupBoardWithLocalData(allCardData, matchedCardIds, totalAttempts);
    }
    
    private void OnGameSaveDataLoaded(LoadedGameSaveDataMessage message)
    {
        if (message.GameSaveData == null)
        {
            StartCoroutine(LoadCardsOnline());
            return;
        }
        allCardData = message.GameSaveData.cardDatas;
        totalAttempts = message.GameSaveData.turns;
        matchedCardIds = message.GameSaveData.matchedCardIds;
        LoadCardsFromLocal();
    }
    

    void SetupBoard()
    {
        foreach (Transform child in gridParent.transform)
            Destroy(child.gameObject);
        controllers.Clear();
        selectedCards.Clear();

        gridParent.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridParent.constraintCount = gridSizeX;
        gridParent.cellSize = new Vector2(gridParent.cellSize.x / gridSizeX, gridParent.cellSize.y / gridSizeY);
        

        int totalCardCount = gridSizeX * gridSizeY;
        int pairCount = totalCardCount / 2;
        int srcCardCount = allCardData.Count;

        List<CardData> pairs = new List<CardData>();
        int addIndex = 0;
        while (pairs.Count < pairCount)
        {
            pairs.Add(allCardData[addIndex % srcCardCount]);
            addIndex++;
        }


        List<CardData> gameCards = new List<CardData>();
        foreach (var c in pairs)
        {
            gameCards.Add(c);
            gameCards.Add(c);
        }

        if (gameCards.Count < totalCardCount)
            gameCards.Add(allCardData[0]); 

        gameCards = Shuffle(gameCards);

        var totalCardsToLoad = gameCards.Count;
        var loadedCardCount = 0;
        
        foreach (var cardData in gameCards)
        {
            var view = Instantiate(cardViewPrefab, gridParent.transform).GetComponent<CardView>();
            var controller = new CardController(cardData, view, OnCardSelected);
            controllers.Add(controller);

            view.StartCoroutine(SpriteLoader.LoadSpriteFromUrl(cardData.SpritePathOrURL, sprite =>
            {
                if (sprite != null) 
                    sprite.name = cardData.Name;
                view.SetFront(sprite);
                
                loadedCardCount++;
                if (loadedCardCount == totalCardsToLoad)
                {
                    StartCoroutine(ShowCards(2.5f));
                }

            }));
        }
    }
    
    void SetupBoardWithLocalData(List<CardData> cardDatas, List<int> matchedCardIds, int turns)
    {
        foreach (Transform child in gridParent.transform)
            Destroy(child.gameObject);
        controllers.Clear();
        selectedCards.Clear();

        gridParent.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridParent.constraintCount = gridSizeX;
        gridParent.cellSize = new Vector2(gridParent.cellSize.x / gridSizeX, gridParent.cellSize.y / gridSizeY);

        
        var totalCardsToLoad = cardDatas.Count;
        var loadedCardCount = 0;
        
        foreach (var cardData in cardDatas)
        {
            var view = Instantiate(cardViewPrefab, gridParent.transform).GetComponent<CardView>();
            var controller = new CardController(cardData, view, OnCardSelected);
            if (matchedCardIds.Contains(cardData.CardId))
                controller.SetMatched();
            controllers.Add(controller);

            view.StartCoroutine(SpriteLoader.LoadSpriteFromUrl(cardData.SpritePathOrURL, sprite =>
            {
                if (sprite != null) 
                    sprite.name = cardData.Name;
                view.SetFront(sprite);
                
                loadedCardCount++;
                if (loadedCardCount == totalCardsToLoad)
                {
                    StartCoroutine(ShowCards(2.5f));
                }

            }));
        }


    }

    void OnCardSelected(CardController controller)
    {
        if (!canPlay) 
            return;
        if (selectedCards.Contains(controller) || controller.IsMatched)
            return;

        controller.Reveal();
        selectedCards.Add(controller);

        if (selectedCards.Count == 2)
            CheckMatch();
    }

    void CheckMatch()
    {
        totalAttempts++;
        var c1 = selectedCards[0];
        var c2 = selectedCards[1];

        if (c1.Data.CardId == c2.Data.CardId)
        {
            c1.SetMatched();
            c2.SetMatched();
            matchedCardIds.Add(c1.Data.CardId);
            var totalCorrect = controllers.Count(c => c.IsMatched) / 2;
            MessageBroker.Default.Publish(new CorrectMatchMessage(c1, c2, totalCorrect, totalAttempts)); 
            MessageBroker.Default.Publish(new CurrentBoardStateMessage(controllers));
            if (matchedCardIds.Count == controllers.Count/2)
            {
                MessageBroker.Default.Publish(new AllCardsMatchedMessage(controllers));
            }
        }
        else
        {
            StartCoroutine(HideCardsAfterDelay(c1, c2));
            MessageBroker.Default.Publish(new WrongMatchMessage(c1, c2,totalAttempts)); 
        }
        
        selectedCards.Clear();
    }

    IEnumerator HideCardsAfterDelay(CardController a, CardController b)
    {
        yield return new WaitForSeconds(1f);
        a.Hide();
        b.Hide();
    }

    List<CardData> Shuffle(List<CardData> input)
    {
        var list = new List<CardData>(input);
        for (int i = 0; i < list.Count; i++)
        {
            int rnd = Random.Range(i, list.Count);
            var tmp = list[i];
            list[i] = list[rnd];
            list[rnd] = tmp;
        }
        return list;
    }
    
    IEnumerator ShowCards(float duration)
    {
        canPlay = false;
        foreach (var controller in controllers)
            controller.Reveal();

        yield return new WaitForSeconds(duration);

        foreach (var controller in controllers)
        {
            if(!controller.IsMatched)
                controller.Hide();
        }
        canPlay = true;
    }

}

