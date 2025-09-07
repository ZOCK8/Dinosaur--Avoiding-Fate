using UnityEngine;
using UnityEngine.UI;

public class ExitGame : MonoBehaviour
{
    public Button button;

    void Start()
    {
        button.onClick.AddListener(Exit);
    }
    void Exit()
    {
        Application.Quit(); // For standalone builds
        // UnityEditor.EditorApplication.isPlaying = false; // For editor testing
        Screen.fullScreen = false; // Optional: Exit full screen mode
        Debug.Log("Game Exited");
    }
}
