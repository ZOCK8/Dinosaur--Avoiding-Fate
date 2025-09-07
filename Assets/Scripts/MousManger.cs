using UnityEngine;
using UnityEngine.InputSystem;

public class MousManger : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() //deaktivates the mosu at the start
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
