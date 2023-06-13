using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace a1Jam
{ 
    public class CameraController : MonoBehaviour
    {
        public Transform target; // Target to follow (the player)
        public float smoothSpeed = 0.125f; // Speed of camera smoothing
        public Vector3 offset; // Offset position from the target
    
        void FixedUpdate()
        {
            Vector3 desiredPosition = target.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;
        }
    }
}
