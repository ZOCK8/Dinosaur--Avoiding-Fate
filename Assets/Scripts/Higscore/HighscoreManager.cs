using UnityEngine;

public class HighscoreManager : MonoBehaviour
{
    void Start()
    {
    }

    void Update() //Updates the stats
    {
        int level = GameDataManager.Instance.data.level;

        if (level >= GameDataManager.Instance.data.highestlevelOne)
        {
            GameDataManager.Instance.data.highestlevelOne = level;
        }
        else if (level >= GameDataManager.Instance.data.highestlevelTwo)
        {
            GameDataManager.Instance.data.highestlevelTwo = level;
        }
        else if (level >= GameDataManager.Instance.data.highestlevelTree)
        {
            GameDataManager.Instance.data.highestlevelTree = level;
        }
        else if (level >= GameDataManager.Instance.data.highestlevelFour)
        {
            GameDataManager.Instance.data.highestlevelFour = level;
        }
        else if (level >= GameDataManager.Instance.data.highestlevelFive)
        {
            GameDataManager.Instance.data.highestlevelFive = level;
        }


        
    }
}
