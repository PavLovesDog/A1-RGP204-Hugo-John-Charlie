using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace a1Jam
{
    public class RotationTracker : MonoBehaviour
    {
        GameManager GM;

        public float totalRotation = 0; // The total rotation in degrees
        public int fullRotations = 0; // The number of full rotations

        public float lastRotation; // The last recorded rotation

        void Start()
        {
            lastRotation = transform.eulerAngles.z; // Initialize the last recorded rotation
            GM = FindObjectOfType<GameManager>();
        }

        void Update()
        {
            float currentRotation = transform.eulerAngles.z; // Get the current rotation

            // Calculate the change in rotation
            float rotationChange = currentRotation - lastRotation;

            // If the rotation change is large (i.e., the object has passed the 0/360 degree mark), adjust the rotation change
            if (rotationChange > 180)
            {
                rotationChange -= 360;
            }
            else if (rotationChange < -180)
            {
                rotationChange += 360;
            }

            totalRotation += rotationChange; // Add the rotation change to the total rotation

            if(!GM.crashed) // only calculate if the player hasn't crashed
            { 
                // If the total rotation reaches 360 degrees, increment the number of full rotations and reset the total rotation
                if (Mathf.Abs(totalRotation) >= 360)
                {
                    fullRotations++;
                    totalRotation %= 360;
                }
            }

            lastRotation = currentRotation; // Update the last recorded rotation
        }
    }
}
