using UnityEngine;

public class NBSController : MonoBehaviour
{
    public Animator NBSAnimator;

    void Update()
    {
        // Rechts
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            NBSAnimator.SetBool("RightHeld", true);
        else
            NBSAnimator.SetBool("RightHeld", false);

        // Links
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            NBSAnimator.SetBool("LeftHeld", true);
        else
            NBSAnimator.SetBool("LeftHeld", false);

        // Hoch
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            NBSAnimator.SetBool("UpHeld", true);
        else
            NBSAnimator.SetBool("UpHeld", false);

        // Runter
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            NBSAnimator.SetBool("DownHeld", true);
        else
            NBSAnimator.SetBool("DownHeld", false);
    }
}
