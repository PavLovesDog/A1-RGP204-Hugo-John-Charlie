using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

// NOTE: tag set to checkPoint temporarily to not get wiped out when big foot stomps

public class UnusualAftermath : MonoBehaviour
{
    public Transform targetPosition;
    public float lerpDuration = 0.1f;
    public float startDelay = 2f; // Delay in seconds before starting the lerping

    private Vector3 currentPosition;
    private float currentLerpTime = 0f;
    public float delayTimer = 0f; // Timer for the start delay
    private bool isLerping = false; // Flag to indicate if lerping is active

    private bool hasStartedLerp = false; // Flag to indicate if lerping has started


    private void Start()
    {
     
    }

    public void StartLerp()
    {
        if (!hasStartedLerp) // Only start the delay timer if lerping hasn't started yet
        {
            delayTimer = 0f; // Reset the delay timer
            hasStartedLerp = true; // Set the flag to indicate that lerping has startedwwww
        }

        if (!isLerping && delayTimer >= startDelay) // Only start lerping if it's not already active and the delay has passed
        {
            currentPosition = transform.position;
            currentLerpTime = 0f;
            isLerping = true; // Activate lerping
        }
    }

    public void StopLerp()
    {
        isLerping = false; // Deactivate lerping
    }

    public void ResetPosition()
    {
        transform.position = transform.position + new Vector3(0, 40, 0); // Set the current position as the starting position
        currentLerpTime = 0f; // Reset the current lerp time
        isLerping = false; // deActivate lerping
        hasStartedLerp = false;
    }

    void Update()
    {
        if (hasStartedLerp) // Only increment the delay timer if lerping has started
        {
            delayTimer += Time.deltaTime; // Increment the delay timer
        }

        // TODO?? instead resetting position on m1 down, move it to when ever the game restarts?
        if (Input.GetMouseButtonUp(0)) // GetMouseButtonDown(0) represents the left mouse button (Mouse1)
        {
            // Call your function here
            ResetPosition();
        }

        if (isLerping)
        {
            if (currentLerpTime < lerpDuration)
            {
                currentLerpTime += Time.deltaTime;
                float t = currentLerpTime / lerpDuration;
                transform.position = Vector3.Lerp(currentPosition, targetPosition.position, t);
            }
            else
            {
                isLerping = false; // Deactivate lerping
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            CameraShake cameraShaker = Camera.main.GetComponent<CameraShake>();
            if (cameraShaker != null)
            {
                cameraShaker.Shake();
            }
        }
    }
}
