using System;
using System.Threading.Tasks;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.VisualScripting;
using UnityEngine;

public class YourName : MonoBehaviour
{
    public TextMeshProUGUI textMeshProUGUI;
    void Update()
    {
        InitializeUGS();
    }

    async Task InitializeUGS()
    {
        await UnityServices.InitializeAsync();

        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
        textMeshProUGUI.text = AuthenticationService.Instance.PlayerName;
    }

}