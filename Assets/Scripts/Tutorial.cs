using System.Collections;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public GameObject Image;
    public TextMeshProUGUI textMeshProUGUI;
    void Awake()
    {
        if (GameDataManager.Instance.data.PlayTutorial)
        {
            StartCoroutine(Text());
        }
        else
        {
            Image.SetActive(false);
            textMeshProUGUI.text = "Error";
        }
    }

    IEnumerator Text()
    {
        Debug.Log("started Tutorial");
        Image.SetActive(true);
        textMeshProUGUI.text = "Try not to get hit";
        yield return new WaitForSeconds(5);
        Image.SetActive(false);
        yield return new WaitForSeconds(3);
        Image.SetActive(true);
        textMeshProUGUI.text = "The pterodactyl drops you items";
        yield return new WaitForSeconds(5);
        Image.SetActive(false);
        yield return new WaitForSeconds(3);
        Image.SetActive(true);
        textMeshProUGUI.text = "You can shoot whit Z";
        yield return new WaitForSeconds(5);
        GameDataManager.Instance.data.PlayTutorial = false;
        Image.SetActive(false);}

}
