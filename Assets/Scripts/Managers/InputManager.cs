using UnityEngine;
using UnityEngine.Events;

public class InputManager : MonoBehaviour
{
    public UnityEvent<Vector2> OnTouchBegan;
    public UnityEvent<Vector2> OnTouchMoved;
    public UnityEvent<Vector2> OnTouchCancelled;

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    OnTouchBegan.Invoke(touch.position);
                    break;

                case TouchPhase.Moved:
                    OnTouchMoved.Invoke(touch.deltaPosition);
                    break;

                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    OnTouchCancelled.Invoke(touch.position);
                    break;
            }
        }
    }
}