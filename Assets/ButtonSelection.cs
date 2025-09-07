using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSelection : MonoBehaviour
{
    public GameObject ButtonContainer;
    public int CurrentButtonIndex = 0;
    public AudioSource ButtonClickSound;
    public AudioSource ButtonConfirmSound;

    void Start()
    {
        HighlightButton(CurrentButtonIndex);
    }

    void Update()
    {
        int buttonCount = ButtonContainer.transform.childCount;

        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            CurrentButtonIndex = (CurrentButtonIndex + 1) % buttonCount;
            HighlightButton(CurrentButtonIndex);
            ButtonClickSound.Play();
        }
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            CurrentButtonIndex = (CurrentButtonIndex - 1 + buttonCount) % buttonCount;
            HighlightButton(CurrentButtonIndex);
            ButtonClickSound.Play();
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            StartCoroutine(PressButton(CurrentButtonIndex));
            ButtonConfirmSound.Play();
        }
    }

    void HighlightButton(int index)
    {
        int buttonCount = ButtonContainer.transform.childCount;
        for (int i = 0; i < buttonCount; i++)
        {
            Button btn = ButtonContainer.transform.GetChild(i).GetComponent<Button>();
            btn.animator.SetTrigger("Normal");
            btn.animator.SetBool("HighlightetButton", false);
            btn.animator.SetBool("PressedButton", false);
        }
        Button current = ButtonContainer.transform.GetChild(index).GetComponent<Button>();
        current.animator.SetBool("HighlightetButton", true);
        current.animator.SetBool("PressedButton", true);
    }

    IEnumerator PressButton(int index)
    {
        Button current = ButtonContainer.transform.GetChild(index).GetComponent<Button>();
        current.animator.SetTrigger("Pressed");
        yield return new WaitForSeconds(0.1f);
        current.onClick.Invoke();
    }
}