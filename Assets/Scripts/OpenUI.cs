using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenUI : MonoBehaviour
{
    public List<GameObject> uiParts;
    public Button openButton;
    public Button closeButton;
    public bool CloseUIOnStart = true;
    void Start()
    {
        if (CloseUIOnStart == true)
        {
            for (int i = 0; i < uiParts.Count; i++)
            uiParts[i].SetActive(false);
        }

        openButton.onClick.AddListener(ToggleUI);
        closeButton.onClick.AddListener(ToggleUIOff);
    }
    void ToggleUI()
    {
        for (int i = 0; i < uiParts.Count; i++)
        if (!uiParts[i].activeSelf)
        {
            uiParts[i].SetActive(true);
            Debug.Log("UI Opened");
        }
    }
    void ToggleUIOff()
    {
        for (int i = 0; i < uiParts.Count; i++)
        if (uiParts[i].activeSelf)
        {
            uiParts[i].SetActive(false);
            Debug.Log("UI Closed");
        }
    }
}
