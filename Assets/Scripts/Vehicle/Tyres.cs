using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace a1Jam
{ 
    public class Tyres : MonoBehaviour
    {
        VehicleController vcScript;

        public float spinSpeed = 3.0f;
        public float decelerationRate;
        public float deceleration = 0;

        private void Start()
        {
            vcScript = FindObjectOfType<VehicleController>();
        }

        void Update()
        {
            //create rotation vector
            Vector3 rotation = new Vector3(0, 0, -45 * spinSpeed);

            //Attach tyre rotation to acceleration
            if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W) && vcScript.canRotate)
            {
                // rotate object
                transform.Rotate(rotation * Time.deltaTime);
                deceleration = -45 * spinSpeed; // set deceleration to the current speed when the key is pressed
            }
            else
            {
                deceleration = Mathf.Lerp(deceleration, 0, Time.deltaTime * decelerationRate); // interpolate deceleration towards 0
                transform.Rotate(new Vector3(0, 0, deceleration * Time.deltaTime));
            }
        }
    }
}
