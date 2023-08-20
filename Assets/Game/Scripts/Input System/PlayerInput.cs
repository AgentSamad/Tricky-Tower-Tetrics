using Game.Scripts;
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
    private Vector2 touchStart_A;
    private Vector2 touchEnd_A;
    [SerializeField] private float minSwipeDistance = 700f;

    public override void Init()
    {
        mainCamera = Camera.main;
        screenBounds =
            mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.transform.position.z));
    }


    public override void ControlTetris(Transform player, float snappingDistance, Participant p)
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Record the initial touch or mouse position when the drag starts.
            touchStartPos = Input.mousePosition;
            touchStart_A = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            // Calculate the delta position of the drag.
            Vector2 deltaPosition = (Vector2)Input.mousePosition - touchStartPos;

            // Calculate the new X position based on the drag.
            float newX = player.position.x + (deltaPosition.x / 100f); // Adjust the sensitivity as needed.

            // Snap the X position to the grid.
            float snappedX = Mathf.Round(newX / snappingDistance) * snappingDistance;

            if (limitToScreenBounds)
            {
                snappedX = Mathf.Clamp(snappedX, screenBounds.x, -screenBounds.x);
            }
            else
            {
                snappedX = Mathf.Clamp(snappedX, bounds.x, -bounds.x);
            }

            player.position = new Vector3(snappedX, player.position.y, player.position.z);

            // Update the touch start position for the next frame.
            touchStartPos = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            touchEnd_A = Input.mousePosition;

            float swipeDistance = touchStart_A.y - touchEnd_A.y; // Vector2.Distance(touchStart_A, touchEnd_A);

            //  Debug.Log(swipeDistance);
            if (swipeDistance > minSwipeDistance)
            {
                // Perform your desired action here
                GameEvents.TetrisDashInvoke(p);
                //     Debug.Log("Dash");
            }
            else if (swipeDistance <= 0)
            {
                GameEvents.TetrisRotateInvoke(p);
            }
        }
    }
}