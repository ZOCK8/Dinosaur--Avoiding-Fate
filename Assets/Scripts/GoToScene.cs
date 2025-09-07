using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GoToScene : MonoBehaviour
{
    public string sceneName;
    public Button button;
    void Start()
    {
        button.onClick.AddListener(OpenScene);
    }
    void OpenScene()
    {
        SceneManager.LoadScene(sceneName);
        Debug.Log("Loading scene: " + sceneName);
    }
}
