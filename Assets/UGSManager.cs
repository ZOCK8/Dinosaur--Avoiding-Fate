using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.Leaderboards;
using Unity.Services.Leaderboards.Models;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

public class UGSManager : MonoBehaviour
{
    public static UGSManager Instance { get; private set; }
    
    [Header("Leaderboard Settings")]
    public string leaderboardId = "Highscore";
    
    [Header("Player Settings")]
    public string playerName;
    
    private bool isInitialized = false;
    private bool isInitializing = false;

    void Awake()
    {
        // Singleton Pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            // Generiere oder lade Spielername
            InitializePlayerName();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    async void Start()
    {
        await InitializeUGS();
    }

    private void InitializePlayerName()
    {
        // Prüfe ob bereits ein Name im GameDataManager gespeichert ist
        if (GameDataManager.Instance?.data != null && !string.IsNullOrEmpty(GameDataManager.Instance.data.playerName))
        {
            playerName = GameDataManager.Instance.data.playerName;
        }
        else
        {
            // Generiere neuen Namen
            playerName = PlayerNameGenerator.GenerateRandomName();
            
            // Speichere im GameDataManager wenn verfügbar
            if (GameDataManager.Instance?.data != null)
            {
                GameDataManager.Instance.data.playerName = playerName;
                // Hier würden Sie normalerweise die Daten speichern
                // GameDataManager.Instance.SaveData();
            }
        }
        
        Debug.Log($"Player Name initialized: {playerName}");
    }

    public async Task InitializeUGS()
    {
        if (isInitialized || isInitializing) 
            return;

        isInitializing = true;

        try
        {
            await UnityServices.InitializeAsync();

            if (!AuthenticationService.Instance.IsSignedIn)
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
            }

            // Setze den Spielernamen in UGS (falls unterstützt)
            try
            {
                await AuthenticationService.Instance.UpdatePlayerNameAsync(playerName);
            }
            catch (Exception e)
            {
                Debug.LogWarning($"Couldn't update player name in UGS: {e.Message}");
            }

            isInitialized = true;
            Debug.Log($"UGS Initialized & Signed In with PlayerId: {AuthenticationService.Instance.PlayerId} and PlayerName: {playerName}");
        }
        catch (Exception e)
        {
            Debug.LogError($"UGS Initialization failed: {e.Message}");
            isInitialized = false;
        }
        finally
        {
            isInitializing = false;
        }
    }

    public async Task<bool> UploadScore()
    {
        // Stelle sicher, dass UGS initialisiert ist
        if (!isInitialized)
        {
            await InitializeUGS();
            if (!isInitialized) return false;
        }

        if (GameDataManager.Instance?.data == null)
        {
            Debug.LogWarning("GameDataManager.Instance oder data ist null!");
            return false;
        }

        int score = GameDataManager.Instance.data.highestlevelOne;

        try
        {
            var metadata = new System.Collections.Generic.Dictionary<string, object>
            {
                { "playerName", playerName }
            };

            await LeaderboardsService.Instance.AddPlayerScoreAsync(leaderboardId, score, new AddPlayerScoreOptions
            {
                Metadata = metadata
            });
            
            Debug.Log($"Score {score} uploaded for PlayerName: {playerName} (PlayerId: {AuthenticationService.Instance.PlayerId})");
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError($"Fehler beim Score Upload: {e.Message}");
            return false;
        }
    }

    public async Task<List<LeaderboardEntry>> GetTopScores(int maxResults = 10)
    {
        // Stelle sicher, dass UGS initialisiert ist
        if (!isInitialized)
        {
            await InitializeUGS();
            if (!isInitialized) return new List<LeaderboardEntry>();
        }

        try
        {
            var scoresResponse = await LeaderboardsService.Instance.GetScoresAsync(
                leaderboardId, 
                new GetScoresOptions { 
                    Limit = maxResults,
                    IncludeMetadata = true
                }
            );

            Debug.Log($"Retrieved {scoresResponse.Results.Count} scores from leaderboard");
            return scoresResponse.Results;
        }
        catch (Exception e)
        {
            Debug.LogError($"Fehler beim Abrufen der Scores: {e.Message}");
            return new List<LeaderboardEntry>();
        }
    }

    public string GetPlayerName()
    {
        return playerName;
    }
}