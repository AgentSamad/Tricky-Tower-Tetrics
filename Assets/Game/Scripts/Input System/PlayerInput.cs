using UnityEngine;
using UnityEngine.Serialization;

public class PlayerInput : InputSystem
{
    [SerializeField] private bool limitToScreenBounds;
    private Vector3 mousePosition, targetPosition;
    private Camera mainCamera;
    private Vector2 screenBounds;

    private Vector2 touchStartPos;
    private Vector2 touchEndPos;
    private const float minSwipeDistance = 50f;

    void Start()
    {
        mainCamera = Camera.main;
        screenBounds =
            mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.transform.position.z));
    }


    public override void ControlTetris(float snappingDistance)
    {
        mousePosition = Input.mousePosition;
        targetPosition = mainCamera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y,
            transform.position.z - mainCamera.transform.position.z));

        // Snap the object's X position to the mouse position on the grid
        float snappedX = Mathf.Round(targetPosition.x / snappingDistance) * snappingDistance;

        if (limitToScreenBounds)
        {
            snappedX = Mathf.Clamp(snappedX, screenBounds.x, -screenBounds.x);
        }
        else
        {
            snappedX = Mathf.Clamp(snappedX, bounds.x, -bounds.x);
        }

        transform.position = new Vector3(snappedX, transform.position.y, transform.position.z);


        DetectSwipe();
    }


    private void DetectSwipe()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    touchStartPos = touch.position;
                    break;

                case TouchPhase.Ended:
                    touchEndPos = touch.position;

                    float swipeDistance = Vector2.Distance(touchStartPos, touchEndPos);

                    if (swipeDistance > minSwipeDistance)
                    {
                        // Calculate the swipe direction
                        Vector2 swipeDirection = touchEndPos - touchStartPos;

                        // Check if it's a downward swipe
                        if (swipeDirection.y < 0)
                        {
                            Debug.Log("Downward swipe detected!");
                            // Perform your desired action here
                        }
                    }

                    break;
            }
        }
    }
}