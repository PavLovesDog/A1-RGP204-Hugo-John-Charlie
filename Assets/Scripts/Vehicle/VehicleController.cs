using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace a1Jam
{
    public class VehicleController : MonoBehaviour
    {
        //Script References
        GameManager GM;

        // For Unusual Aftermath
        public UnusualAftermath unusualAftermath;

        //Vehicle Variables
        public float speed = 10.0f;
        public float tiltSpeed = 100.0f; // The speed at which the vehicle tilts
        public Rigidbody2D vehicleRB;
        public bool canDrive = true;
        public bool canRotate = true;
        public bool isOnGround = false;

        //Wing Varibales
        public bool hasWings;
        float maxLinearDrag = 3f;
        float minLinearDrag = 0f;
        float maxGravityScale = 12f;
        float aKeyDownDuration = 0f;            // Track the duration the A key has been held down
        public float thrusterForce = 10f;       // Adjust the thruster force as needed

        //Physics materials
        public PhysicsMaterial2D friction;
        public PhysicsMaterial2D noFriction;

        //Ray Variables
        public Collider2D[] rcColliders;
        public float raycastDistance = 5.0f;
    
        void Start()
        {
            unusualAftermath = FindObjectOfType<UnusualAftermath>();

            GM = FindObjectOfType<GameManager>();
            vehicleRB = GetComponent<Rigidbody2D>(); 
            rcColliders = GetComponentsInChildren<Collider2D>();

        }
    
        
        void Update()
        {
            // Vehicle Control ----------------------------------------------------------------------------------------------
            if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W) && canDrive) // Check if 'arrow up' or 'w' is pressed
            {
                Vector2 direction = transform.right; // Get the direction the vehicle is facing
                vehicleRB.AddForce(direction * speed); // Add a force in the direction the vehicle is facing
            }

            // Vehicle Rotation ----------------------------------------------------------------------------------------------
            if (canRotate)
            {
                if (hasWings)
                {
                    tiltSpeed = 2f;

                    // Read user input
                    float horizontalInput = Input.GetAxis("Horizontal");
                    float verticalInput = Input.GetAxis("Vertical");
                    bool isAKeyDown = Input.GetKey(KeyCode.A);           

                    // Adjust the rotation based on horizontal input
                    float rotation = -horizontalInput * tiltSpeed;
                    vehicleRB.rotation = Mathf.Clamp(vehicleRB.rotation + rotation, -45f, 45f); // Clamp the rotation value between -45 and 45 degrees

                    // Calculate the flight direction based on the rotation
                    Vector2 flightDirection = Quaternion.Euler(0, 0, vehicleRB.rotation) * Vector2.right;

                    // Adjust gravity scale based on tilt
                    float tilt = Mathf.Clamp(horizontalInput, -4f, 4f) ; // Clamp the tilt value between -1 and 1
                    float gravityScale = 8f + (tilt / 2f) * 8f; // Add the entire tilt value to the gravity scale

                    vehicleRB.gravityScale = gravityScale;

                    // Adjust linear drag based on gravity scale
                    float linearDrag = Mathf.Lerp(maxLinearDrag, minLinearDrag, gravityScale / maxGravityScale);
                    vehicleRB.drag = linearDrag;

                    /*// Apply thruster force when A key is held down
                    if (isAKeyDown)
                     {
                         Vector2 thrusterForceVector = flightDirection * thrusterForce;
                         vehicleRB.AddForce(thrusterForceVector);
                     }*/

                    // Apply thruster force when A key is held down for 1 second
                    if (isAKeyDown)
                    {
                        aKeyDownDuration += Time.deltaTime;

                        if (aKeyDownDuration <= 0.75f)
                        {
                            Vector2 thrusterForceVector = flightDirection * thrusterForce;
                            vehicleRB.AddForce(thrusterForceVector);
                        }
                    }
                    else
                    {
                        // Reset the timer when the A key is released
                        aKeyDownDuration = 0f;
                    }
                }
                else 
                {
                    // Get input from the left and right arrow keys or the 'a' and 'd' keys
                    float tilt = Input.GetAxis("Horizontal"); // This also tied top player movement...?

                    // Apply a rotation around the Z axis based on the input
                    transform.Rotate(0, 0, -tilt * tiltSpeed * Time.deltaTime);
                }
            }


            //Check if vehicle flipped! ----------------------------------------------------------------------------------------
            for (int angle = 60; angle <= 120; angle += 15) // Loop from 45 degrees to 135 degrees in 15 degree increments
            {
                Vector2 direction = transform.right;
                Vector2 rotatedDirection = Quaternion.Euler(0, 0, angle) * direction; // Rotate the direction vector by the current angle around the Z axis
    
                RaycastHit2D hit = Physics2D.Raycast(transform.position, rotatedDirection, raycastDistance); // Cast a ray at the current angle
                Debug.DrawRay(transform.position, rotatedDirection * raycastDistance, Color.red); // Visualise
    
                if (hit.collider != null) // If the ray hits an object (Note, player and shoe objects are labeled "ignore raycast")
                {
                    Debug.Log(hit.collider.name);

                    if (hit.collider.CompareTag("checkPoint"))
                    {
                        continue; // Do stuff
                    }

                    if (hit.collider.CompareTag("LaunchZone"))
                    {
                        continue; // If the ray hits the launch zone, ignore it
                    }

                    if (hit.collider.CompareTag("BigFoot"))
                    {
                        continue; // This is code is pretty wet huh?
                    }

                    canDrive = false; // stop movement
                    canRotate = false; // stop rotation

                    SetRollcageCollision(false);
                    GM.wipeoutText.gameObject.SetActive(true);
                }
            }

            if (isOnGround) { unusualAftermath.StartLerp(); }
        }

        // Function which sets the rollcages colliders of vehicle active or not
        public void SetRollcageCollision(bool method)
        {
            foreach (var collider in rcColliders) // run through rollcage colliders & disable
            {
                if (collider.tag == "rollcage")
                    collider.enabled = method;
                GM.crashed = true; // set crashed bool for scoring
            }
        }

        // COLLISIONS ------------------------------------------------------------------------------------------------------
        private void OnTriggerEnter2D(Collider2D collision)
        {
            // check if we're past the flight checkpoint
            if (collision.gameObject.tag == "checkPoint")
            {
                canDrive = false; // no more accelerating in the air
                collision.sharedMaterial = friction; // DOESNT DO ANYTHIN??
                vehicleRB.sharedMaterial = friction; // set physics material for friction

                // TODO: if the player bought wings, can trigger hasWings, else continue
                // TEMP!, if player hits check point, attach wings
                if(transform.GetChild(7).gameObject.activeSelf) 
                {
                    hasWings = true;
                }
                
            }

            // check if the ground has been hit
            if (collision.gameObject.tag == "Ground")
            {
                canRotate = false; // stop all rotations
                isOnGround = true;
                GM.StartCoroutine(GM.Countdown());

                unusualAftermath.delayTimer = 0f;
            }

            // Reset Values
            if (collision.gameObject.tag == "BigFoot")
            {
                hasWings = false;
                vehicleRB.drag = 0;
                vehicleRB.gravityScale = 8;
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            // check if the ground has been hit
            if (collision.gameObject.tag == "Ground")
            {
                canRotate = false; // stop all rotations
                isOnGround = true;
                GM.StartCoroutine(GM.Countdown()); // start scoring message

                // 
                hasWings = false;
                vehicleRB.drag = 0;

                unusualAftermath.delayTimer = 0f;
            }
        }

        // Function to call when the player upgrades their car, to update the script attachment
        public void UpdateUnusualAftermathScript()
        {
            unusualAftermath = FindObjectOfType<UnusualAftermath>();
        }
    }
}
