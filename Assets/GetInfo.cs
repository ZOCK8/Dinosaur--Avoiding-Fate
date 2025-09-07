using UnityEngine;

public class GetInfo : MonoBehaviour
{
    public string playerName;
    public int highestlevel;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        playerName = GameDataManager.Instance.data.playerName; // Example of setting a default name
        highestlevel = GameDataManager.Instance.data.highestlevelOne;
        
    }
}
