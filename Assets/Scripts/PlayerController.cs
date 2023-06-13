using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace a1Jam
{
    public class PlayerController : MonoBehaviour
    {
        //references
        VehicleController vcScript;
        GameManager GM;

        //Player Variables
        public Rigidbody2D rb;
        public float speed;
        public float jumpForce;
        public bool canControl = true;
    
        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            vcScript = FindObjectOfType<VehicleController>(); // find the vehicle control script
            GM = FindObjectOfType<GameManager>();
        }
    
        void Update()
        {
            if(vcScript.canDrive != false && canControl == true)
            {
                //REMOVED MOVEMENT, WAS UNNECESSARY
                ////set up movement, attach to input
                //float moveHorizontal = Input.GetAxis("Horizontal");
                //
                ////apply input to velocity
                //rb.velocity = new Vector2(moveHorizontal * speed, rb.velocity.y);
    
                //Jump?? why not
                if (Input.GetButtonDown("Jump")) //could be GetButtonDown and cvheck for time held to multiply jump height
                {
                    rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "checkPoint")
            {
                Debug.Log("Hit Flight Point!");
                canControl = false;
                GM.beginTracking = true;
            }
        }
    }
}
