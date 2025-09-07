using UnityEngine;

public class SpaceToGoToScene : MonoBehaviour
{
    public string sceneName;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
            Debug.Log("Loading scene: " + sceneName);
        }
    }
}
