using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace a1Jam
{
    public class TimeManager : MonoBehaviour
    {
        public int resetCount;

        //Background Object References
        public GameObject morningBG;
        public GameObject middayBG;
        public GameObject sunsetBG;
        public GameObject nightBG;

        private void Start()
        {
            //start in the morning
            morningBG.SetActive(true);
        }

        private void Update()
        {
            TrackTimeCounter(resetCount);
        }

        private void TrackTimeCounter(float counter)
        {
            if(counter >= 0 && counter < 4) // Morning
            {
                nightBG.SetActive(false);
                morningBG.SetActive(true);
            }
            else if(counter >= 4 && counter < 8) // Midday
            {
                morningBG.SetActive(false);
                middayBG.SetActive(true);
            }
            else if(counter >= 8 && counter < 12) // Sunset
            {
                middayBG.SetActive(false);
                sunsetBG.SetActive(true);
            }
            else if (counter >= 12 && counter < 16) // Night
            {
                sunsetBG.SetActive(false);
                nightBG.SetActive(true);
            }
            else if (counter >= 16) // reset counter!
            {
                resetCount = 0;
            }
            else
            {
                //chill
            }

        }
    }
}
