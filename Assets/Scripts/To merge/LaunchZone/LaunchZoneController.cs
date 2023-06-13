using a1Jam;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchZoneController : MonoBehaviour
{
    public Vector2 launchForce;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Vehicle"))
        {
            Rigidbody2D playerRigidbody = other.GetComponent<Rigidbody2D>();
            playerRigidbody.AddForce(launchForce, ForceMode2D.Impulse);
        }

        VehicleController vehicle = other.GetComponent<VehicleController>();
        //vehicle.enabled = true;
    }
}
