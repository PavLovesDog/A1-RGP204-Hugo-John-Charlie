using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float flightSpeed = 5f;
    public float rotationSpeed = 4f;
    public float thrusterForce = 1000f; // Adjust the thruster force as needed

    Rigidbody2D rb;
    float maxGravityScale = 3f;
    float maxLinearDrag = 1f;
    float minLinearDrag = 0f;
    float glideForceMultiplier = 1.5f;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        // Read user input
        float verticalInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");
        bool isAKeyDown = Input.GetKey(KeyCode.A);

        // Adjust the rotation based on horizontal input
        float rotation = -horizontalInput * rotationSpeed;
        rb.rotation = Mathf.Clamp(rb.rotation + rotation, -75f, 75f); // Clamp the rotation value between -45 and 45 degrees

        // Calculate the flight direction based on the rotation
        Vector2 flightDirection = Quaternion.Euler(0, 0, rb.rotation) * Vector2.right;

        // Apply a force to move the character in the flight direction
        float forceMultiplier = verticalInput > 0f ? glideForceMultiplier : 1f;
        float upwardForce = verticalInput > 0f ? Mathf.Abs(verticalInput) : 0f;
        rb.AddForce(flightDirection * flightSpeed * upwardForce * forceMultiplier);

        // Adjust gravity scale based on tilt
        float tilt = Mathf.Clamp(horizontalInput, -45f, 45f); // Clamp the tilt value between -1 and 1
        float gravityScale = 4f + tilt; // Add 1 to the tilt value to achieve the desired range

        rb.gravityScale = gravityScale;

        // Adjust linear drag based on gravity scale
        float linearDrag = Mathf.Lerp(maxLinearDrag, minLinearDrag, gravityScale / maxGravityScale);
        rb.drag = linearDrag;

        // Apply thruster force when A key is held down
        if (isAKeyDown)
        {
            Vector2 thrusterForceVector = flightDirection * thrusterForce;
            rb.AddForce(thrusterForceVector);
        }

    }
}

/*public float flightSpeed = 5f;
    public float rotationSpeed = 2f;
    public float thrusterForce = 100f; // Adjust the thruster force as needed

    Rigidbody2D rb;
    float maxGravityScale = 3f;
    float maxLinearDrag = 1f;
    float minLinearDrag = 0f;
    float glideForceMultiplier = 1.5f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        // Read user input
        float verticalInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");
        bool isSpaceKeyDown = Input.GetKey(KeyCode.A);

        // Adjust the rotation based on horizontal input
        float rotation = -horizontalInput * rotationSpeed;
        rb.rotation += rotation;

        // Calculate the flight direction based on the rotation
        Vector2 flightDirection = Quaternion.Euler(0, 0, rb.rotation) * Vector2.up;

        // Apply a force to move the character in the flight direction
        float forceMultiplier = verticalInput > 0f ? glideForceMultiplier : 1f;
        float upwardForce = verticalInput > 0f ? Mathf.Abs(verticalInput) : 0f;
        rb.AddForce(flightDirection * flightSpeed * upwardForce * forceMultiplier);

        // Adjust gravity scale based on tilt
        float tilt = Mathf.Clamp(horizontalInput, -1f, 1f); // Clamp the tilt value between -1 and 1
        float gravityScale = 1f + tilt; // Add 1 to the tilt value to achieve the desired range

        rb.gravityScale = gravityScale;

        // Adjust linear drag based on gravity scale
        float linearDrag = Mathf.Lerp(maxLinearDrag, minLinearDrag, gravityScale / maxGravityScale);
        rb.drag = linearDrag;

        // Apply thruster force when space key is held down
        if (isSpaceKeyDown)
        {
            Vector2 thrusterForceVector = flightDirection * thrusterForce;
            rb.AddForce(thrusterForceVector);
        }
    }*/

