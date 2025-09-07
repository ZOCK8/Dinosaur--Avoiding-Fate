using UnityEngine;

public class AnyButtonOpenScene : MonoBehaviour
{
    public string sceneName; // Name of the scene to load
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.X))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        }
        
    }
}
