using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace a1Jam
{
    public class UpgradeManager : MonoBehaviour
    {
        GameManager GM;
        VehicleController vcScript;

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
        public float wingsPrice;

        //TMP Text References
        //currency
        public TMP_Text currencyText;
        //shoes
        public TMP_Text s_youngPriceText;
        public TMP_Text s_hightopPriceText;
        public TMP_Text s_sportsPriceText;
        public TMP_Text s_oldPriceText;
        //rockets
        public TMP_Text r_1PriceText;
        public TMP_Text r_2PriceText;
        public TMP_Text r_3PriceText;
        //Wings
        public TMP_Text wingsText;


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
        public GameObject wings;
        public bool ownsWings;

        //Shoe References
        public GameObject youngShoe; // child
        public GameObject hightopShoe; // teen
        public GameObject sportsShoe; // adult
        public GameObject oldShoe; // elderly
        public bool hasYoungShoe;
        public bool hasHightopShoe;
        public bool hasSportsShoe;
        public bool hasOldShoe;
    
        private void Start()
        {
            GM = FindObjectOfType<GameManager>();
            vcScript = FindObjectOfType<VehicleController>();
            player = FindObjectOfType<PlayerController>().gameObject;
            playerLook = player.GetComponent<SpriteRenderer>();

            //always start with young shoe equipped
            youngShoe.SetActive(true);
            currentShoe = youngShoe;
            wings = FindChildWithTag(currentShoe.transform, "Wings"); // set wings object

            // Initial Setup
            currentShoe = FindObjectOfType<VehicleController>().gameObject;
            rocket_1 = FindChildWithTag(currentShoe.transform, "1Rocket");
            rocket_2 = FindChildWithTag(currentShoe.transform, "2Rocket");
            rocket_Large = FindChildWithTag(currentShoe.transform, "LRocket");
            hasRocket1 = false;
            hasRocket2 = false;
            hasRocket3 = false;

            //setup prices
            s_youngPriceText.text = "$" + shoeYoungPrice;
            s_hightopPriceText.text = "$" + shoeHightopPrice;
            s_sportsPriceText.text = "$" + shoeSportsPrice;
            s_oldPriceText.text = "$" + shoeOldPrice;
            r_1PriceText.text = "$" + rocket1Price;
            r_2PriceText.text = "$" + rocket2Price;
            r_3PriceText.text = "$" + rocket3Price;
            wingsText.text = "$" + wingsPrice;
        }

        private void Update()
        {
            currencyText.text = "Points($): " + Mathf.Ceil(GM.totalScore);
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

        //WINGS -------------------------------
        public void ActivateWings()
        {
            if (GM.totalScore >= wingsPrice || ownsWings)
            {
                //minus points, and check if already own
                if (!ownsWings)
                { 
                    GM.totalScore -= wingsPrice;
                    ownsWings = true;
                }

                //activate wings
                wings = FindChildWithTag(currentShoe.transform, "Wings"); // find wings on current vehicle
                wings.SetActive(true);
                vcScript = FindObjectOfType<VehicleController>(); // find vehicle script of current vehicle
                vcScript.hasWings = true;
            }
        }

        public void DeactivateWings()
        {
            if(ownsWings && wings.activeSelf) // if players owns wings and they're active
            {
                wings = FindChildWithTag(currentShoe.transform, "Wings");
                wings.SetActive(false);
                vcScript = FindObjectOfType<VehicleController>(); // find vehicle script of current vehicle
                vcScript.hasWings = false;
            }
        }

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
                r_1PriceText.text = "Purchased";

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
                r_2PriceText.text = "Purchased";

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
                r_3PriceText.text = "Purchased";

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
            vcScript = currentShoe.GetComponent<VehicleController>();
            vcScript.UpdateUnusualAftermathScript();
            //vcScript.canRotate = true;

            //Activate young shoe
            youngShoe.SetActive(true);
            GM.rtScript = FindObjectOfType<RotationTracker>(); 
            TransferRocketUpgrades();

        }

        // select hightop shoe
        public void ActivateHightopShoe()
        {
            if (GM.totalScore >= shoeHightopPrice || hasHightopShoe)
            {
                //Spend points
                if(!hasHightopShoe)
                {
                    GM.totalScore -= shoeHightopPrice;
                    //change price in UI element
                    s_hightopPriceText.text = "Purchased";
                    hasHightopShoe = true;
                }

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

                //Update vehicle being used to correctly track upgrades
                currentShoe = hightopShoe;
                UpdateVehicleUpgrades();

                // Update the vehicle script of selected shoe to hold the new reference for the Unusual Aftermath
                vcScript = currentShoe.GetComponent<VehicleController>();
                vcScript.UpdateUnusualAftermathScript();
                //vcScript.canRotate = true;

                //Activate young shoe
                hightopShoe.SetActive(true);
                GM.rtScript = FindObjectOfType<RotationTracker>();
                TransferRocketUpgrades();
            }
        }

        // select sports shoe
        public void ActivateSportsShoe()
        {
            if (GM.totalScore >= shoeSportsPrice || hasSportsShoe)
            {
                //Spend points
                if(!hasSportsShoe)
                {
                    GM.totalScore -= shoeSportsPrice;
                    //change price in UI element
                    s_sportsPriceText.text = "Purchased";
                    hasSportsShoe = true;
                }
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

                //Update vehicle being used to correctly track upgrades
                currentShoe = sportsShoe;
                UpdateVehicleUpgrades();

                // Update the vehicle script of selected shoe to hold the new reference for the Unusual Aftermath
                vcScript = currentShoe.GetComponent<VehicleController>();
                vcScript.UpdateUnusualAftermathScript();
                //vcScript.canRotate = true;

                //Activate young shoe
                sportsShoe.SetActive(true);
                GM.rtScript = FindObjectOfType<RotationTracker>();
                TransferRocketUpgrades();
            }
        }

        // select old shoe
        public void ActivateOldShoe()
        {
            if (GM.totalScore >= shoeOldPrice || hasOldShoe)
            {
                if(!hasOldShoe)
                {
                    GM.totalScore -= shoeOldPrice;
                    //change price in UI element
                    s_oldPriceText.text = "Purchased";
                    hasOldShoe = true;
                }
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

                //Update vehicle being used to correctly track upgrades
                currentShoe = oldShoe;
                UpdateVehicleUpgrades();

                // Update the vehicle script of selected shoe to hold the new reference for the Unusual Aftermath
                vcScript = currentShoe.GetComponent<VehicleController>();
                vcScript.UpdateUnusualAftermathScript();
                //vcScript.canRotate = true;

                //Activate young shoe
                oldShoe.SetActive(true);
                GM.rtScript = FindObjectOfType<RotationTracker>();
                TransferRocketUpgrades();
            }
        }
    }
}
