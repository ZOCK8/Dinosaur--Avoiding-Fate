using TMPro;
using UnityEngine;
using Unity.Services.Leaderboards.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

public class LeaderboardDisplay : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Text leaderboardText;
    public TMP_Text statusText;
    
    [Header("Display Settings")]
    public int maxResults = 10;
    public bool autoRefresh = true;
    public float refreshInterval = 30f; // Sekunden
    
    private float lastRefreshTime;
    private List<LeaderboardEntry> cachedScores; // Cache für alte Daten
    private string lastSuccessfulUpdate = "Never";

    void Start()
    {
        if (leaderboardText == null)
        {
            leaderboardText = GetComponent<TMP_Text>();
        }

        // Initialisiere mit leerem Cache
        cachedScores = new List<LeaderboardEntry>();
        
        _ = RefreshLeaderboard();
    }

    void Update()
    {
        if (autoRefresh && Time.time - lastRefreshTime > refreshInterval)
        {
            _ = RefreshLeaderboard();
        }
    }

    public async Task RefreshLeaderboard()
    {
        lastRefreshTime = Time.time;

        if (statusText != null)
            statusText.text = "Loading...";

        if (UGSManager.Instance == null)
        {
            // Verwende cached Daten falls vorhanden, sonst Fehlermeldung
            if (cachedScores.Count > 0)
            {
                UpdateDisplayWithCachedData("UGSManager not found! (Using cached data)");
            }
            else
            {
                UpdateDisplay("UGSManager not found!");
                if (statusText != null)
                    statusText.text = "Error: UGSManager missing";
            }
            return;
        }

        try
        {
            var scores = await UGSManager.Instance.GetTopScores(maxResults);
            
            if (scores.Count == 0 && cachedScores.Count > 0)
            {
                // Keine neuen Daten, aber alte Daten vorhanden
                UpdateDisplayWithCachedData("No new data available (Using cached data)");
                return;
            }
            else if (scores.Count == 0)
            {
                // Weder neue noch alte Daten
                UpdateDisplay("No scores available");
                if (statusText != null)
                    statusText.text = "No data";
                return;
            }

            // Erfolgreiche Aktualisierung - Cache updaten
            cachedScores = new List<LeaderboardEntry>(scores);
            lastSuccessfulUpdate = System.DateTime.Now.ToString("HH:mm:ss");
            
            string display = FormatLeaderboard(cachedScores);
            UpdateDisplay(display);
            
            if (statusText != null)
                statusText.text = $"Updated: {lastSuccessfulUpdate}";
        }
        catch (System.Exception e)
        {
            Debug.LogWarning($"Leaderboard refresh failed: {e.Message}");
            
            // Bei Fehler (z.B. Rate Limit) - verwende cached Daten
            if (cachedScores.Count > 0)
            {
                UpdateDisplayWithCachedData($"Update failed: {GetShortErrorMessage(e.Message)}");
            }
            else
            {
                UpdateDisplay($"Failed to load scores: {GetShortErrorMessage(e.Message)}");
                if (statusText != null)
                    statusText.text = "Error: No cached data";
            }
        }
    }

    private void UpdateDisplayWithCachedData(string warningMessage)
    {
        string display = FormatLeaderboard(cachedScores);
        UpdateDisplay(display);
        
        if (statusText != null)
            statusText.text = $"{warningMessage} | Last update: {lastSuccessfulUpdate}";
    }

    private string GetShortErrorMessage(string fullError)
    {
        // Kürze lange Fehlermeldungen für die UI
        if (fullError.Contains("rate") || fullError.Contains("limit"))
            return "Too many requests";
        else if (fullError.Contains("network") || fullError.Contains("connection"))
            return "Connection issue";
        else if (fullError.Contains("timeout"))
            return "Timeout";
        else
            return "Unknown error";
    }

    private string FormatLeaderboard(List<LeaderboardEntry> scores)
    {
        string display = "=== TOP SCORES ===\n\n";
        
        for (int i = 0; i < scores.Count; i++)
        {
            var entry = scores[i];
            string playerName = GetPlayerNameFromEntry(entry);
            
            // Formatierung: Rang. Name - Score
            display += $"{entry.Rank + 1}. {playerName} - {entry.Score:N0}\n";
        }

        return display;
    }

    private string GetPlayerNameFromEntry(LeaderboardEntry entry)
    {
        // Priorität 1: UGS PlayerName (falls gesetzt)
        if (!string.IsNullOrEmpty(entry.PlayerName) && entry.PlayerName != entry.PlayerId)
        {
            return entry.PlayerName;
        }
        
        // Priorität 2: Versuche Metadata (falls vorhanden)
        if (entry.Metadata != null)
        {
            string metadataStr = entry.Metadata.ToString();
            if (!string.IsNullOrEmpty(metadataStr) && metadataStr.Contains("playerName"))
            {
                // Einfache Extraktion für JSON-Format: {"playerName":"SomeName"}
                int startIdx = metadataStr.IndexOf("\"playerName\":\"");
                if (startIdx >= 0)
                {
                    startIdx += 14; // Länge von "playerName":"
                    int endIdx = metadataStr.IndexOf("\"", startIdx);
                    if (endIdx > startIdx)
                    {
                        return metadataStr.Substring(startIdx, endIdx - startIdx);
                    }
                }
            }
        }
        
        // Fallback: Generiere ansprechenden Namen aus PlayerId
        string playerId = entry.PlayerId ?? "Unknown";
        
        // Kürze lange PlayerIds zu lesbaren Namen
        if (playerId.Length > 12)
        {
            return "Player" + playerId.Substring(playerId.Length - 4);
        }
        else if (playerId.Length > 6)
        {
            return "Player" + playerId.Substring(playerId.Length - 3);
        }
        
        return "Player" + playerId;
    }

    private void UpdateDisplay(string text)
    {
        if (leaderboardText != null)
        {
            leaderboardText.text = text;
        }
        else
        {
            Debug.Log("Leaderboard Display:\n" + text);
        }
    }

    // Public method für manuellen Refresh
    public void RefreshLeaderboardButton()
    {
        _ = RefreshLeaderboard();
    }
}