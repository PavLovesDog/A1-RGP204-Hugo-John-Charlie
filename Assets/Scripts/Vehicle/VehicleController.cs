using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace a1Jam
{
    public class VehicleController : MonoBehaviour
    {
        //Script References
        GameManager GM;

        //Vehicle Variables
        public float speed = 10.0f;
        public float tiltSpeed = 100.0f; // The speed at which the vehicle tilts
        public Rigidbody2D vehicleRB;
        public bool canDrive = true;
        public bool canRotate = true;
        public bool isOnGround = false;

        //Physics materials
        public PhysicsMaterial2D friction;
        public PhysicsMaterial2D noFriction;

        //Ray Variables
        public Collider2D[] rcColliders;
        public float raycastDistance = 5.0f;
    
        void Start()
        {
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
                // Get input from the left and right arrow keys or the 'a' and 'd' keys
                float tilt = Input.GetAxis("Horizontal"); // This also tied top player movement...?

                // Apply a rotation around the Z axis based on the input
                transform.Rotate(0, 0, -tilt * tiltSpeed * Time.deltaTime);
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
                        continue; // If the ray hits the launch zone, ignore it
                    }

                    if (hit.collider.CompareTag("LaunchZone"))
                    {
                        //Do Stuff
                    }

                    canDrive = false; // stop movement
                    canRotate = false; // stop rotation

                    SetRollcageCollision(false);
                    GM.wipeoutText.gameObject.SetActive(true);
                }
            }
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
            }

            // check if the ground has been hit
            if (collision.gameObject.tag == "Ground")
            {
                canRotate = false; // stop all rotations
                isOnGround = true;
                GM.StartCoroutine(GM.Countdown());
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
            }
        }
    }
}
