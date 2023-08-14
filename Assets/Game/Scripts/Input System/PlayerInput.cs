using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Player Input", menuName = "Game/Player Input")]
public class PlayerInput : InputSystem
{
    [SerializeField] private bool limitToScreenBounds;
    private Vector3 mousePosition, targetPosition;
    private Camera mainCamera;
    private Vector2 screenBounds;

    private Vector2 touchStartPos;
    private Vector2 touchEndPos;
    private const float minSwipeDistance = 50f;

    public override void Init()
    {
        mainCamera = Camera.main;
        screenBounds =
            mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.transform.position.z));
    }


    public override void ControlTetris(Transform player, float snappingDistance)
    {
        mousePosition = Input.mousePosition;
        targetPosition = mainCamera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y,
            player.position.z - mainCamera.transform.position.z));

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

        player.position = new Vector3(snappedX, player.position.y, player.position.z);


        DetectSwipe();
    }


    private void DetectSwipe()
    {
        if (Input.GetMouseButtonDown(0))
        {
            touchStartPos = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            touchEndPos = Input.mousePosition;

            float swipeDistance = Vector2.Distance(touchStartPos, touchEndPos);

            if (swipeDistance > minSwipeDistance && (touchEndPos.y - touchStartPos.y) < 0)
            {
                // Perform your desired action here
                GameEvents.TetrisDashInvoke();
            }

            GameEvents.TetrisRotateInvoke();
        }
    }
}