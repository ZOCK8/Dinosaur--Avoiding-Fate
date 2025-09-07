using UnityEngine;
using UnityEngine.UI;

public class OpenLink : MonoBehaviour
{
    public string url;
    public Button openLinkButton;

    void Start()
    {
        openLinkButton.onClick.AddListener(OpenLinkInBrowser);
    }
    void OpenLinkInBrowser()
    {
        Application.OpenURL(url);
        Debug.Log("Link Opened: " + url);
    }
}
