using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace a1Jam
{
    public class UpgradeManager : MonoBehaviour
    {
        GameManager GM;
        VehicleController vehicleController;

        //current vehicle in use
        public GameObject currentShoe;

        // Player variables
        public GameObject player;
        public SpriteRenderer playerLook;
        public Sprite babySprite;
        public Sprite youngSprite;
        public Sprite adultSprite;
        public Sprite elderlySprite;

        //UI References
        //Upgrade Prices
        public float shoeYoungPrice;
        public float shoeHightopPrice;
        public float shoeSportsPrice;
        public float shoeOldPrice;

        public float rocket1Price;
        public float rocket2Price;
        public float rocket3Price;

        //currency
        public TMP_Text currencyText;
        //shoes
        public TMP_Text s_youngPrice;
        public TMP_Text s_hightopPrice;
        public TMP_Text s_sportsPrice;
        public TMP_Text s_oldPrice;
        //rockets
        public TMP_Text r_1Price;
        public TMP_Text r_2Price;
        public TMP_Text r_3Price;


        //Rocket References
        public GameObject rocket_1;
        public GameObject rocket_2;
        public GameObject rocket_Large;
        public bool hasRocket1;
        public bool hasRocket2;
        public bool hasRocket3;
        public bool rocket1Equipped;
        public bool rocket2Equipped;
        public bool rocket3Equipped;

        //Wing References
        //public GameObject wings;
        public bool hasWings;

        //Shoe References
        public GameObject youngShoe; // child
        public GameObject hightopShoe; // teen
        public GameObject sportsShoe; // adult
        public GameObject oldShoe; // elderly
    
        private void Start()
        {
            GM = FindObjectOfType<GameManager>();
            player = FindObjectOfType<PlayerController>().gameObject;
            playerLook = player.GetComponent<SpriteRenderer>();

            //always start with young shoe equipped
            youngShoe.SetActive(true);

            // Initial Setup
            currentShoe = FindObjectOfType<VehicleController>().gameObject;
            rocket_1 = FindChildWithTag(currentShoe.transform, "1Rocket");
            rocket_2 = FindChildWithTag(currentShoe.transform, "2Rocket");
            rocket_Large = FindChildWithTag(currentShoe.transform, "LRocket");
            hasRocket1 = false;
            hasRocket2 = false;
            hasRocket3 = false;

            //setup prices
            s_youngPrice.text = "$" + shoeYoungPrice;
            s_hightopPrice.text = "$" + shoeHightopPrice;
            s_sportsPrice.text = "$" + shoeSportsPrice;
            s_oldPrice.text = "$" + shoeOldPrice;
            r_1Price.text = "$" + rocket1Price;
            r_2Price.text = "$" + rocket2Price;
            r_3Price.text = "$" + rocket3Price;
        }

        private void Update()
        {
            currencyText.text = "$$$: " + Mathf.Ceil(GM.totalScore);
        }

        public void UpdateVehicleUpgrades()
        {
            //Rockets
            rocket_1 = FindChildWithTag(currentShoe.transform, "1Rocket");
            rocket_2 = FindChildWithTag(currentShoe.transform, "2Rocket");
            rocket_Large = FindChildWithTag(currentShoe.transform, "LRocket");
        }

        public void TransferRocketUpgrades()
        {
            //check if rocket active and set it
            if (hasRocket1 && rocket1Equipped)
            {
                OneRocket();
            }
            else if (hasRocket2 && rocket2Equipped)
            {
                TwoRocket();
            }
            else if (hasRocket3 && rocket3Equipped)
            {
                LargeRocket();
            }
        }

        //Function to help find/set child object based on Tag
        GameObject FindChildWithTag(Transform parent, string tag)
        {
            // Check if the current GameObject has the tag
            if (parent.tag == tag)
            {
                return parent.gameObject;
            }

            // Search the children
            for (int i = 0; i < parent.childCount; i++)
            {
                GameObject found = FindChildWithTag(parent.GetChild(i), tag);

                if (found != null)
                {
                    return found;
                }
            }

            // No GameObject found with the tag
            return null;
        }

        // UI BUTTON FUNCTIONS ----------------------------------------------------------

        //ROCKETS -----------------------------
        // select no rockets
        public void NoRockets()
        {
            // check if any rockets are selected (ACTIVE)
            if(rocket_1.activeSelf || rocket_2.activeSelf || rocket_Large.activeSelf)
            {
                // set them all as inactive
                if(rocket_1.activeSelf)
                {
                    rocket_1.SetActive(false);
                    rocket1Equipped = false;
                }
                if (rocket_2.activeSelf)
                { 
                    rocket_2.SetActive(false);
                    rocket2Equipped = false;
                }
                if (rocket_Large.activeSelf)
                {
                    rocket_Large.SetActive(false);
                    rocket3Equipped = false;
                }
            }
        }
    
        // select one rocket
        public void OneRocket()
        {
            if(GM.totalScore >= rocket1Price || hasRocket1)
            {
                //minus points, and check if already own
                if(!hasRocket1)
                    GM.totalScore -= rocket1Price;

                // check if other rockets are selected (ACTIVE)
                if (rocket_2.activeSelf || rocket_Large.activeSelf)
                {
                    if (rocket_2.activeSelf)
                    {
                        rocket_2.SetActive(false);
                        rocket2Equipped = false;
                    }
                    if (rocket_Large.activeSelf)
                    {
                        rocket_Large.SetActive(false);
                        rocket3Equipped = false;
                    }
                }

                //change price in UI element
                r_1Price.text = "Purchased";

                //Activate Upgrade
                rocket_1.SetActive(true);
                hasRocket1 = true;
                rocket1Equipped = true;
            }
        }

        // select 2 rockets
        public void TwoRocket()
        {
            if(GM.totalScore >= rocket2Price || hasRocket2)
            {
                //if not already own rocket, Spend points
                if(!hasRocket2)
                    GM.totalScore -= rocket2Price;

                // check if other rockets are selected (ACTIVE)
                if (rocket_1.activeSelf || rocket_Large.activeSelf)
                {
                    if (rocket_1.activeSelf)
                    {
                        rocket_1.SetActive(false);
                        rocket1Equipped = false;
                    }
                    if (rocket_Large.activeSelf)
                    {
                        rocket_Large.SetActive(false);
                        rocket3Equipped = false;
                    }
                }

                //change price in UI element
                r_2Price.text = "Purchased";

                //Activate Upgrade
                rocket_2.SetActive(true);
                hasRocket2 = true;
                rocket2Equipped = true;
            }
        }

        // select large rockets
        public void LargeRocket()
        {
            if (GM.totalScore >= rocket3Price || hasRocket3)
            {
                //Spend points
                if(!hasRocket3)
                    GM.totalScore -= rocket3Price;

                // check if any rockets are selected (ACTIVE)
                if (rocket_1.activeSelf || rocket_2.activeSelf)
                {
                    // set them all as inactive
                    if (rocket_1.activeSelf)
                    {
                        rocket_1.SetActive(false);
                        rocket1Equipped = false;
                    }
                    if (rocket_2.activeSelf)
                    {
                        rocket_2.SetActive(false);
                        rocket2Equipped = false;
                    }
                }

                //change price in UI element
                r_3Price.text = "Purchased";

                //Activate Upgrade
                rocket_Large.SetActive(true);
                hasRocket3 = true;
                rocket3Equipped = true;
            }
        }

        //SHOES -------------------------------
        // select young shoe
        public void ActivateYoungShoe()
        {
            //check for other vehicle active status
            if(hightopShoe.activeSelf || sportsShoe.activeSelf || oldShoe.activeSelf)
            {
                //set necessary shoe inactive
                if (hightopShoe.activeSelf)
                    hightopShoe.SetActive(false);
                if (sportsShoe.activeSelf)
                    sportsShoe.SetActive(false);
                if (oldShoe.activeSelf)
                    oldShoe.SetActive(false);
            }

            //change player sprite
            playerLook.sprite = babySprite;

            //Update vehicle being used to correctly track upgrades
            currentShoe = youngShoe;
            UpdateVehicleUpgrades();

            // Update the vehicle script of selected shoe to hold the new reference for the Unusual Aftermath
            vehicleController = currentShoe.GetComponent<VehicleController>();
            vehicleController.UpdateUnusualAftermathScript(); 

            //Activate young shoe
            youngShoe.SetActive(true);
            GM.rtScript = FindObjectOfType<RotationTracker>(); 
            TransferRocketUpgrades();

        }

        // select hightop shoe
        public void ActivateHightopShoe()
        {
            if (GM.totalScore >= shoeHightopPrice)
            {
                //Spend points
                GM.totalScore -= shoeHightopPrice;
                //check for other vehicle active status
                if (youngShoe.activeSelf || sportsShoe.activeSelf || oldShoe.activeSelf)
                {
                    //set necessary shoe inactive
                    if (youngShoe.activeSelf)
                        youngShoe.SetActive(false);
                    if (sportsShoe.activeSelf)
                        sportsShoe.SetActive(false);
                    if (oldShoe.activeSelf)
                        oldShoe.SetActive(false);
                }

                playerLook.sprite = youngSprite;

                //change price in UI element
                s_hightopPrice.text = "Purchased";

                //Update vehicle being used to correctly track upgrades
                currentShoe = hightopShoe;
                UpdateVehicleUpgrades();

                // Update the vehicle script of selected shoe to hold the new reference for the Unusual Aftermath
                vehicleController = currentShoe.GetComponent<VehicleController>();
                vehicleController.UpdateUnusualAftermathScript();

                //Activate young shoe
                hightopShoe.SetActive(true);
                GM.rtScript = FindObjectOfType<RotationTracker>();
                TransferRocketUpgrades();
            }
        }

        // select sports shoe
        public void ActivateSportsShoe()
        {
            if (GM.totalScore >= shoeSportsPrice)
            {
                //Spend points
                GM.totalScore -= shoeSportsPrice;
                //check for other vehicle active status
                if (youngShoe.activeSelf || hightopShoe.activeSelf || oldShoe.activeSelf)
                {
                    //set necessary shoe inactive
                    if (youngShoe.activeSelf)
                        youngShoe.SetActive(false);
                    if (hightopShoe.activeSelf)
                        hightopShoe.SetActive(false);
                    if (oldShoe.activeSelf)
                        oldShoe.SetActive(false);
                }

                playerLook.sprite = adultSprite;

                //change price in UI element
                s_sportsPrice.text = "Purchased";

                //Update vehicle being used to correctly track upgrades
                currentShoe = sportsShoe;
                UpdateVehicleUpgrades();

                // Update the vehicle script of selected shoe to hold the new reference for the Unusual Aftermath
                vehicleController = currentShoe.GetComponent<VehicleController>();
                vehicleController.UpdateUnusualAftermathScript();

                //Activate young shoe
                sportsShoe.SetActive(true);
                GM.rtScript = FindObjectOfType<RotationTracker>();
                TransferRocketUpgrades();
            }
        }

        // select old shoe
        public void ActivateOldShoe()
        {
            if (GM.totalScore >= shoeOldPrice)
            {
                //Spend points
                GM.totalScore -= shoeOldPrice;
                //check for other vehicle active status
                if (youngShoe.activeSelf || hightopShoe.activeSelf || sportsShoe.activeSelf)
                {
                    //set necessary shoe inactive
                    if (youngShoe.activeSelf)
                        youngShoe.SetActive(false);
                    if (hightopShoe.activeSelf)
                        hightopShoe.SetActive(false);
                    if (sportsShoe.activeSelf)
                        sportsShoe.SetActive(false);
                }

                playerLook.sprite = elderlySprite;

                //change price in UI element
                s_oldPrice.text = "Purchased";

                //Update vehicle being used to correctly track upgrades
                currentShoe = oldShoe;
                UpdateVehicleUpgrades();

                // Update the vehicle script of selected shoe to hold the new reference for the Unusual Aftermath
                vehicleController = currentShoe.GetComponent<VehicleController>();
                vehicleController.UpdateUnusualAftermathScript();

                //Activate young shoe
                oldShoe.SetActive(true);
                GM.rtScript = FindObjectOfType<RotationTracker>();
                TransferRocketUpgrades();
            }
        }
    }
}
