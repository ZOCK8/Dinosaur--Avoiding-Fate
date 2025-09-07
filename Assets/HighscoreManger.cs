using System;
using System.Xml;
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
        highscoreText.text = "1: " + GameDataManager.Instance.data.highestlevelOne.ToString() + Environment.NewLine
        + "2: " + GameDataManager.Instance.data.highestlevelTwo.ToString() + Environment.NewLine
        + "3: " + GameDataManager.Instance.data.highestlevelTree.ToString() + Environment.NewLine
        + "4: " + GameDataManager.Instance.data.highestlevelFour.ToString() + Environment.NewLine
        + "5: " + GameDataManager.Instance.data.highestlevelFive.ToString()
        ;
    }

}
