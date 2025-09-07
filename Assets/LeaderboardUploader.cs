using UnityEngine;
using System.Threading.Tasks;

public class LeaderboardUploader : MonoBehaviour
{
    [Header("Auto Upload Settings")]
    public bool uploadOnStart = true;
    public bool uploadOnNewHighscore = true;

    private int lastUploadedScore = -1;

    async void Start()
    {
        if (uploadOnStart)
        {
            await UploadCurrentScore();
        }
    }

    void Update()
    {
        // Prüfe auf neuen Highscore
        if (uploadOnNewHighscore && GameDataManager.Instance?.data != null)
        {
            int currentScore = GameDataManager.Instance.data.highestlevelOne;
            if (currentScore > lastUploadedScore)
            {
                _ = UploadCurrentScore(); // Fire-and-forget async call
            }
        }
    }

    public async Task<bool> UploadCurrentScore()
    {
        if (UGSManager.Instance == null)
        {
            Debug.LogWarning("UGSManager.Instance ist null! Stelle sicher, dass UGSManager in der Szene vorhanden ist.");
            return false;
        }

        bool success = await UGSManager.Instance.UploadScore();
        
        if (success && GameDataManager.Instance?.data != null)
        {
            lastUploadedScore = GameDataManager.Instance.data.highestlevelOne;
        }

        return success;
    }

    // Public method für manuellen Upload
    public void UploadScore()
    {
        _ = UploadCurrentScore();
    }
}