using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI Text;
    [SerializeField] private GameObject ScoreGameObject;
    [SerializeField] private Tutorial tutorial;
    void Start()
    {
        ScoreGameObject.SetActive(false);
        Text.text = "Score: 0";
    }

    void Update()
    {
        if (!GameDataManager.Instance.data.PlayTutorial)
        {
            ScoreGameObject.SetActive(true);
        }

        Text.text = "Score: " + GameDataManager.Instance.data.level.ToString();
    }
}
