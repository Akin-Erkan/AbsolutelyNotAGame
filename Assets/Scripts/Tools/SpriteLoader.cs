using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public static class SpriteLoader
{
    public static IEnumerator LoadSpriteFromUrl(string url, Action<Sprite> onLoaded)
    {
        if (string.IsNullOrEmpty(url))
        {
            Debug.LogError("[SpriteLoader] Sprite URL null!");
            onLoaded?.Invoke(null);
            yield break;
        }

        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            Texture2D texture = DownloadHandlerTexture.GetContent(www);
            Sprite sprite = Sprite.Create(texture, new Rect(0,0,texture.width,texture.height), new Vector2(0.5f, 0.5f));
            onLoaded?.Invoke(sprite);
        }
        else
        {
            Debug.LogError($"[SpriteLoader] Sprite load failed for: {url} - Error: {www.error} - HTTP: {www.responseCode}");
            onLoaded?.Invoke(null);
        }
    }

}