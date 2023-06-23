using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace a1Jam
{ 
    public class GameManager : MonoBehaviour
    {
        //Script References
        PlayerController playerScript;
        public VehicleController vcScript;
        public RotationTracker rtScript;
        public RocketControl rcScript;
        TimeManager tmScript;
        UnusualAftermath uaScript;
        public UpgradeManager umScript;
        public RandomPrefabPlacer rrpScript;

        //GameObjects
        public Collider2D flightCheck;
        public GameObject player;
        public GameObject playerSpawnPoint;
        public GameObject vehicle;
        public GameObject measurePoint;

        //Tracking Variables
        public bool beginTracking = false;
        public bool maxPoints = false;
        public float maxPointMultiplier;
        public float maxDistance;
        public float maxHeight = 0;
        public int numFlips = 0;
        public float flipMultiplier;
        public float playerScore;
        public float highScore;
        float heightBase;
        public float countdownTimer = 3.0f;

        //Scores
        public List<float> scores;
        public float totalScore;

        //Game State Variables
        public bool gameRunning = false;
        public bool crashed = false;
        public bool canReset = false;
        public bool canDisplayEndScreen = true;

        //Title UI
        public GameObject TitleScreen;

        //UI Overlay variables
        public GameObject UI_OverlayPanel;
        public TMP_Text distanceText;
        public TMP_Text heightText;
        public TMP_Text flipsText;
        public GameObject scorePanel;
        public TMP_Text scoreText;
        public TMP_Text totalScoreText;
        public TMP_Text highScoreText;
        public TMP_Text wipeoutText;

        //Scoring Overlay Variables
        public GameObject s_scoringPanel;
        public GameObject maxPointsText;
        public TMP_Text s_distanceCalcText;
        public TMP_Text s_heightCalcText;
        public TMP_Text s_flipCalcText;
        public TMP_Text s_crashedCalcTest;
        public TMP_Text s_totalScorePointsText;

        //Upgrades Overlay Varoables
        public GameObject u_upgradePanel;
        public TMP_Text currencyText;
        // shoe upgrades
        public TMP_Text u_y_shoePriceText;
        public TMP_Text u_s_shoePriceText;
        public TMP_Text u_h_shoePriceText;
        // rocket upgrades
        public TMP_Text u_1RocketPriceText;
        public TMP_Text u_2RocketPriceText;
        public TMP_Text u_largeRocketPriceText;


        //Positions
        Vector3 playerStartPos;
        Quaternion playerRotation;
        Vector3 vehicleStartPos;
        Quaternion vehicleRotation;

        void Start()
        {
            //find references
            playerScript = FindObjectOfType<PlayerController>();
            rtScript = FindObjectOfType<RotationTracker>();
            vcScript = FindObjectOfType<VehicleController>();
            rcScript = FindObjectOfType<RocketControl>();
            tmScript = FindObjectOfType<TimeManager>();
            umScript = FindObjectOfType<UpgradeManager>();
            uaScript = FindObjectOfType<UnusualAftermath>();
            rrpScript = FindObjectOfType<RandomPrefabPlacer>();
            playerSpawnPoint = FindObjectOfType<SpawnMe>().gameObject;
            vehicle = FindObjectOfType<VehicleController>().gameObject; // load whatever vehicle is in the scene

            //Tracking variables etuo
            heightBase = measurePoint.transform.position.y; // set the base for max height
            //scorePanel.SetActive(false);
            //scoreText.gameObject.SetActive(false); // turn off score display
            wipeoutText.gameObject.SetActive(false);

            //Store positions for reset
            playerStartPos = playerSpawnPoint.transform.position;
            playerRotation = playerSpawnPoint.transform.rotation;
            vehicleStartPos = vehicle.transform.position;
            vehicleRotation = vehicle.transform.rotation;
            player.transform.position = playerSpawnPoint.transform.position; // set pplayers p[osition on start

            //setup highscore if there is one
            if (PlayerPrefs.HasKey("Highscore"))
            {
                highScore = PlayerPrefs.GetFloat("Highscore");
                Debug.Log("Loaded high score: " + Mathf.Ceil(highScore));
            }
            else
            {
                Debug.Log("No high score saved!");
            }
        }
    
    
        void Update()
        {
            if(Input.GetKeyUp(KeyCode.Escape))
            {
                //player wants to exit
                Application.Quit();
            }

            //Track distance & height from player to flight check -----------------------------------------------------
            if(beginTracking && playerScript != null)
            {
                //  - Start tracking distance travelled 
                float distance = Vector3.Distance(player.transform.position, measurePoint.transform.position); // Calculate the distance between the two objects
                if (distance > maxDistance) // If the current distance is greater than the maximum distance recorded
                {
                    maxDistance = distance; // Update the maximum distance
                }

                //  - Start tracking player max height
                if(player.transform.position.y > heightBase)
                {
                    if (player.transform.position.y - heightBase > maxHeight)
                        maxHeight = player.transform.position.y - heightBase;
                }

                // Track rotations from rotationtracker script
                numFlips = rtScript.fullRotations;
            }

            // Update UI ---------------------------------------------------------------------------
            distanceText.text = "Distance: " + Mathf.Ceil(maxDistance) + "m";
            heightText.text = "Height: " + Mathf.Ceil(maxHeight) + "m";
            flipsText.text = "Flips: " + numFlips;

            // SCORE ----------------------------------------------------------------------------
            playerScore = (maxDistance / 4) + (maxHeight / 2);

            switch (numFlips)
            {
                case 0:
                    flipMultiplier = 1;
                    playerScore = playerScore * 1;
                    break;
                case 1:
                    flipMultiplier = 1.25f;
                    playerScore = playerScore * 1.25f;
                    break;
                case 2:
                    flipMultiplier = 1.75f;
                    playerScore = playerScore * 1.75f;
                    break;
                case 3:
                    flipMultiplier = 2;
                    playerScore = playerScore * 2;
                    break;
                case 4:
                    flipMultiplier = 3;
                    playerScore = playerScore * 3;
                    break;
                default:
                    flipMultiplier = 3;
                    playerScore = playerScore * 3;
                    break;
            }

            //check for maxpoint multiplier
            if(maxPoints)
            {
                flipMultiplier = maxPointMultiplier;
                playerScore = playerScore * maxPointMultiplier;

            }

            if (crashed)
            {
                playerScore *= 0.1f; // deduct points for failure
                scoreText.text = "Your Score: " + Mathf.Ceil(playerScore) + "\n\n...press 'r' to reset...";
            }
            else
            {
                scoreText.text = "Your Score: " + Mathf.Ceil(playerScore) + "\n\n...press 'r' to reset...";
            }

            //run through list of scores to find highest score
            foreach (var score in scores)
            {
                if (score > highScore)
                {
                    highScore = score;

                    //save highscore to disk
                    //PlayerPrefs.SetInt("Highscore", (int)Mathf.Ceil(highScore));
                    PlayerPrefs.SetFloat("Highscore", highScore);
                    PlayerPrefs.Save();
                }
            }

            // update UI scores
            totalScoreText.text = "Score: " + totalScore;
            highScoreText.text = "Highscore: " + highScore;
        }

        public void StartGame()
        {
            gameRunning = true;

            // turn off title screen
            if(TitleScreen.activeSelf)
                TitleScreen.SetActive(false);

            //enable UI screen overlay
            if (!TitleScreen.activeSelf)
                UI_OverlayPanel.SetActive(true);
        }

        public void ResetDriver()
        {
            // Reset Driver ----------------------------------------------------------------------
            if (canReset)
            {
                // reset launchzones across the map
                rrpScript.PlaceRandomPrefabs();

                //update Vehicle and Vehicle Controllewr script (if car was upgraded)
                vehicle = umScript.currentShoe;
                vcScript = FindObjectOfType<VehicleController>();

                // reset player position
                player.transform.position = playerStartPos;
                player.transform.rotation = playerRotation;
                // reset shoe position
                vehicle.transform.position = vehicleStartPos;
                vehicle.transform.rotation = vehicleRotation;

                //reset velocity
                player.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
                vehicle.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);

                //reset friction
                vcScript.vehicleRB.sharedMaterial = vcScript.noFriction;

                //reset car rollcage collisions
                vcScript.SetRollcageCollision(true);

                //Find approriate Rocket Control Script
                rcScript = FindObjectOfType<RocketControl>();

                //reset bools
                maxPoints = false;
                canDisplayEndScreen = true;
                vcScript.isOnGround = false;
                beginTracking = false;
                playerScript.canControl = true;
                vcScript.canDrive = true;
                vcScript.canRotate = true;
                crashed = false;
                canReset = false;
                if(rcScript != null)
                    rcScript.canFireRocket = true;
                uaScript.canShakeCam = true;
                vcScript.affectDrag = false;

                // reset max height
                maxHeight = 0;
                heightText.text = "Height: " + maxHeight;
                // reset max distance
                maxDistance = 0;
                distanceText.text = "Distance: " + maxDistance;
                // reset flips (in both scripts)
                numFlips = 0;
                rtScript.fullRotations = 0;
                flipsText.text = "Flips: " + numFlips;

                //Reset Big Foot position
                uaScript.ResetPosition();

                // reset score amount?
                playerScore = 0;

                //Update vehicle being used to correctly track upgrades
                umScript.currentShoe = FindObjectOfType<VehicleController>().gameObject;
                vcScript = FindObjectOfType<VehicleController>(); // ensure vehicle script updates

                // set UI elements correctly
                UI_OverlayPanel.SetActive(true);
                //scorePanel.SetActive(false);
                //scoreText.gameObject.SetActive(false);
                wipeoutText.gameObject.SetActive(false);
                if(u_upgradePanel.activeSelf)
                    u_upgradePanel.SetActive(false);

                //Increment reset counter for time change
                tmScript.resetCount++;

            }
        }

        public void DisplayUpgradesScreen()
        {
            if(s_scoringPanel.activeSelf)
                s_scoringPanel.SetActive(false);
            if(!u_upgradePanel.activeSelf)
                u_upgradePanel.SetActive(true);
            // Save score for upgrade menu
            scores.Add(Mathf.Ceil(playerScore));
            totalScore += Mathf.Ceil(playerScore);
        }

        //Countdown timer to display score text ---------------------------------------
        public IEnumerator Countdown()
        {
            float timer = countdownTimer;

            while(timer > 0 && vcScript.isOnGround)
            {
                yield return null;
                timer -= Time.deltaTime;
            }

            //timer over, do the shit
            //deactivate overlayUI/activate scoring panel
            UI_OverlayPanel.SetActive(false);
            if (!s_scoringPanel.activeSelf && canDisplayEndScreen)
            {
                canDisplayEndScreen = false;
                s_scoringPanel.SetActive(true);
            }

            // calculate scores
            s_distanceCalcText.text = Mathf.Ceil(maxDistance) + "m x 0.25 =    " + Mathf.Ceil(maxDistance / 4); 
            s_heightCalcText.text = Mathf.Ceil(maxHeight) + "m x 0.5 =    " + Mathf.Ceil(maxHeight / 2);
            if(maxPoints)
            {
                maxPointsText.SetActive(true);
                s_flipCalcText.text = numFlips + " =    x" + maxPointMultiplier;
                s_flipCalcText.color = Color.green;
            }
            else
            {
                maxPointsText.SetActive(false);
                s_flipCalcText.text = numFlips + " =    x" + flipMultiplier;
                s_flipCalcText.color = Color.white;
            }
            if(crashed)
            {
                s_crashedCalcTest.text = "               =    x0.1";
                s_crashedCalcTest.color = Color.red;
            }
            else
            {
                s_crashedCalcTest.text = "               =    x0";
                s_crashedCalcTest.color = Color.green;
            }
            s_totalScorePointsText.text = "Score: " + Mathf.Ceil(playerScore);

            canReset = true;
        }
    }
}
