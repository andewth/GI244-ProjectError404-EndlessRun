using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class GameResult
{
    public string coins;
    public string time;
}

[System.Serializable]
public class GameResultList
{
    public List<GameResult> results = new List<GameResult>();
}

public static class GameResultManager
{
    private static readonly string filePath = Path.Combine(Application.dataPath, "../game_results.json");

    public static void SaveResult(string coins, string time)
    {
        GameResultList resultList = LoadResults();

        // เพิ่มผลลัพธ์ใหม่ที่หัวรายการ
        resultList.results.Insert(0, new GameResult
        {
            coins = coins,
            time = time
        });

        // จำกัดแค่ 5 รายการล่าสุด
        if (resultList.results.Count > 5)
            resultList.results = resultList.results.GetRange(0, 5);

        string json = JsonUtility.ToJson(resultList, true);
        File.WriteAllText(filePath, json);

        Debug.Log("Coins: " + coins);
        Debug.Log("Time: " + time);
    }

    public static GameResultList LoadResults()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            return JsonUtility.FromJson<GameResultList>(json);
        }

        // ถ้าไม่มีไฟล์ ให้สร้างใหม่
        GameResultList emptyList = new GameResultList();
        string newJson = JsonUtility.ToJson(emptyList, true);
        File.WriteAllText(filePath, newJson);
        return emptyList;
    }
}







// Web GL
// using System.Collections.Generic;
// using UnityEngine;

// [System.Serializable]
// public class GameResult
// {
//     public string coins;
//     public string time;
// }

// [System.Serializable]
// public class GameResultList
// {
//     public List<GameResult> results = new List<GameResult>();
// }

// public static class GameResultManager
// {
//     private const string PlayerPrefsKey = "game_results";

//     public static void SaveResult(string coins, string time)
//     {
//         GameResultList resultList = LoadResults();

//         resultList.results.Insert(0, new GameResult
//         {
//             coins = coins,
//             time = time
//         });

//         if (resultList.results.Count > 5)
//             resultList.results = resultList.results.GetRange(0, 5);

//         string json = JsonUtility.ToJson(resultList);
//         PlayerPrefs.SetString(PlayerPrefsKey, json);
//         PlayerPrefs.Save();

//         Debug.Log("Coins: " + coins);
//         Debug.Log("Time: " + time);
//     }

//     public static GameResultList LoadResults()
//     {
//         if (PlayerPrefs.HasKey(PlayerPrefsKey))
//         {
//             string json = PlayerPrefs.GetString(PlayerPrefsKey);
//             return JsonUtility.FromJson<GameResultList>(json);
//         }

//         return new GameResultList();
//     }
// }
