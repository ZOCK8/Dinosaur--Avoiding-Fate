using UnityEngine;
using System.IO;
using UnityEngine.Rendering;

public class GameDataManager : MonoBehaviour
{
    public static GameDataManager Instance;

    public GameData data = new GameData();

    private string savePath;

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
#if UNITY_WEBGL
            // Kein Pfad nötig für WebGL
#else
            savePath = Path.Combine(Application.persistentDataPath, "GameData.json");
#endif
            LoadData();
            SaveData();

        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void Update()
    {
        AudioListener.volume = Instance.data.Volume;
    }

    private void OnApplicationQuit()
    {
        SaveData();
    }
    public void SaveData()
    {
        string json = JsonUtility.ToJson(data, true);
#if UNITY_WEBGL
        PlayerPrefs.SetString("GameData", json);
        PlayerPrefs.Save();
        Debug.Log("Game Data in PlayerPrefs gespeichert (WebGL)");
#else
        File.WriteAllText(savePath, json);
        Debug.Log("Game Data als Datei gespeichert: " + savePath);
#endif
    }

    public void LoadData()
    {
#if UNITY_WEBGL
        if (PlayerPrefs.HasKey("GameData"))
        {
            string json = PlayerPrefs.GetString("GameData");
            data = JsonUtility.FromJson<GameData>(json);
            Debug.Log("Game Data von PlayerPrefs geladen (WebGL)");
        }
        else
        {
            data = new GameData();
            Debug.Log("Keine gespeicherten Daten gefunden, Standardwerte werden verwendet (WebGL).");
        }
#else
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            data = JsonUtility.FromJson<GameData>(json);
            Debug.Log("Game Data von Datei geladen!");
        }
        else
        {
            data = new GameData();
            Debug.Log("Keine gespeicherte Datei gefunden, Standardwerte werden verwendet.");
        }
#endif
    }

    public void DeleteData()
    {
#if UNITY_WEBGL
        PlayerPrefs.DeleteKey("GameData");
        data = new GameData();
        Debug.Log("Game Data aus PlayerPrefs gelöscht (WebGL)");
#else
        if (File.Exists(savePath))
            File.Delete(savePath);
        data = new GameData();
        Debug.Log("Game Data Datei gelöscht!");
#endif
    }
}
