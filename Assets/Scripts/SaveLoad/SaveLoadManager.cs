using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{
    
    
    
    
    //TODO: either use these methods or create custom serialization methods to save json online
    public void SaveGamePlayPrefs(GameSaveData data)
    {
        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString("save_game", json); 
        PlayerPrefs.Save();
    }
    
    public GameSaveData LoadGamePlayPrefs()
    {
        if (!PlayerPrefs.HasKey("save_game")) return null;
        string json = PlayerPrefs.GetString("save_game");
        return JsonUtility.FromJson<GameSaveData>(json);
    }
    
}
