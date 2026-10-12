using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class UserInput : MonoBehaviour
{
    public UnityAction OnMouseDown;
    public UnityAction OnMouseMove;
    public UnityAction OnMouseUp;

    private bool isPointerDown;

    private void Update()
    {
        if (Pointer.current == null)
            return;

        var press = Pointer.current.press;

        // Mouse click / Mobile touch started
        if (press.wasPressedThisFrame)
        {
            isPointerDown = true;
            OnMouseDown?.Invoke();
        }

        // Mouse drag / Finger held on screen
        if (isPointerDown && press.isPressed)
        {
            OnMouseMove?.Invoke();
        }

        // Mouse released / Finger lifted
        if (press.wasReleasedThisFrame)
        {
            isPointerDown = false;
            OnMouseUp?.Invoke();
        }
    }
}