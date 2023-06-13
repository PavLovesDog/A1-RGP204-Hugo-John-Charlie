using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace a1Jam
{
    public class RocketControl : MonoBehaviour
    {
        //References
        VehicleController vcScript;
        CameraController camScript;

        public GameObject flameSprite;

        public float rocketDuration;
        public float rocketSpeed;
        public float cameraShiftDuration;
        public float cameraSmoothSpeed;
        public float cameraRevertSpeed;
        public bool canFireRocket;

        void Start()
        {
            vcScript = FindObjectOfType<VehicleController>();
            camScript = FindObjectOfType<CameraController>();
            flameSprite = GetComponentInChildren<RocketFire>().gameObject;

            flameSprite.SetActive(false);
            canFireRocket = true;
        }
    
        
        void Update()
        {
            // NOTE need to tie this to ifHasRocketUpgrade bool
            if (Input.GetKeyDown(KeyCode.Space) && canFireRocket)
            {
                canFireRocket = false; // only fire it once
                StartCoroutine(IgniteRocket(rocketDuration, rocketSpeed)); // begin coroutine to handle rocket
            }
        }
        
        // Rocket Ignition Process ------------------------------------------------------------------------------
        IEnumerator IgniteRocket(float time, float acceleration)
        {
            // activates flame sprite
            flameSprite.SetActive(true);

            //turn off car gravity
            float originalGravityScale = vcScript.vehicleRB.gravityScale;
            vcScript.vehicleRB.gravityScale = 2;

            //accelerate car
            StartCoroutine(ApplyForce(time, acceleration));

            //Shift camera for acceleration visual effect
            StartCoroutine(CameraShift(cameraShiftDuration, cameraSmoothSpeed, cameraRevertSpeed));

            yield return new WaitForSeconds(time);

            //stuff run its course, finish up
            flameSprite.SetActive(false);
            vcScript.vehicleRB.gravityScale = originalGravityScale;
        }

        // Force Applied from rocket propulsion for dur\ation -----------------------------------------------------
        IEnumerator ApplyForce(float time, float acceleration)
        {
            float timer = 0; // A timer to keep track of how long the force has been applied

            //get direction
            Vector2 direction = vcScript.gameObject.transform.right;
            Vector2 force = direction * acceleration;

            while (timer < time)
            {
                vcScript.vehicleRB.AddForce(force); // Apply the force

                timer += Time.deltaTime; // Increment the timer by the time since the last frame

                yield return null; // Wait for the next frame
            }
        }

        // camera offest -------------------------------------------------------------------------------
        IEnumerator CameraShift(float time, float cameraSmoothSpeed, float cameraRevertSpeed)
        {
            //create camera pull effect
            float originalSmoothSpeed = camScript.smoothSpeed;

            //change the camera smooth speed
            camScript.smoothSpeed = cameraSmoothSpeed;

            yield return new WaitForSeconds(time);

            // Transition back to the original smoothing speed
            float timer = 0;

            while (timer < cameraRevertSpeed)
            {
                // Interpolate between the current smooth speed and the original smooth speed
                camScript.smoothSpeed = Mathf.Lerp(cameraSmoothSpeed, originalSmoothSpeed, timer / cameraRevertSpeed);
                timer += Time.deltaTime;
                yield return null;
            }

            // Ensure the smooth speed is exactly the original smooth speed at the end of transition
            camScript.smoothSpeed = originalSmoothSpeed;
        }
    }
}
