using TMPro;
using UnityEngine;

public class SettingsTutorial : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMeshProUGUI;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z) && GameDataManager.Instance.data.PlayTutorial)
        {
            GameDataManager.Instance.data.PlayTutorial = false;
        textMeshProUGUI.text = GameDataManager.Instance.data.PlayTutorial.ToString();
        textMeshProUGUI.text = GameDataManager.Instance.data.PlayTutorial.ToString();
        }
        else if (Input.GetKeyDown(KeyCode.Z) && !GameDataManager.Instance.data.PlayTutorial)
        {
            GameDataManager.Instance.data.PlayTutorial = true;
            textMeshProUGUI.text = GameDataManager.Instance.data.PlayTutorial.ToString();
        }
        textMeshProUGUI.text = GameDataManager.Instance.data.PlayTutorial.ToString();
    }
}
