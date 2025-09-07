using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SoundController : MonoBehaviour
{
    public Slider volumeSlider;
    [Range(0f, 1)]
    public float Intesity = 100f;

    void Start()
    {
        volumeSlider.value = GameDataManager.Instance.data.Volume.ConvertTo<int>();
        volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    private void SetVolume(float arg0)
    {
        GameDataManager.Instance.data.Volume = volumeSlider.value.ConvertTo<int>();
    }
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            volumeSlider.value -= Intesity;
        }
        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            volumeSlider.value += Intesity;
        }
    }
}
