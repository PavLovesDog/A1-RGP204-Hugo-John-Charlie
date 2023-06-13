using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace a1Jam
{
    public class RocketFire : MonoBehaviour
    {
        // Rocket variables
        SpriteRenderer flame;
        bool flip;
        bool startCoroutine;
        public float delayTime;
        public bool doubleRocket;

        private void Start()
        {
            flame = GetComponent<SpriteRenderer>();
            flip = true;
            startCoroutine = true;
        }
        void Update()
        {
            //initiate routine to flip the fire sprite to emulate flame movement
            if(startCoroutine)
            {
                startCoroutine = false;
                StartCoroutine(FlipFlameSprite(delayTime));
            }
        }

        IEnumerator FlipFlameSprite(float time)
        {
            // delay
            yield return new WaitForSeconds(time);
            //delay over, perform  code below
            if(doubleRocket)
            {
                flame.flipY = flip;
            }
            else
            {
                flame.flipX = flip; // flip sprite
            }

            // set flip bool to opposite
            if (flip)
            {
                flip = false;
            }
            else
            {
                flip = true;
            }
            //reset starting coroutine bool
            startCoroutine = true;
        }
    }
}
