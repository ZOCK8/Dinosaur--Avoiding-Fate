using TMPro;
using UnityEngine;

public class HighscoreTextManger : MonoBehaviour
{
    public TextMeshProUGUI highscoreText;

    void Start()
    {
        highscoreText.text = "Highscore: " + GameDataManager.Instance.data.highestlevelOne.ToString();
    }

    void Update()
    {
        highscoreText.text = "1: " + GameDataManager.Instance.data.highestlevelOne.ToString() +;
    }

}
