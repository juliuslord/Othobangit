using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class PlayerController : MonoBehaviour   // This script is attached to the free-moving player //
{
    public bool dead;   // confirming death

    public MoveWithPlayer cameraRotation;   
    public LookAt mainCamera;
    private Vector3 relativeDirection;      // These 4 handle the player's movement relative to the camera
    private Vector3 directionNorm;

    public float jumpCharge;
    float maxJump;
    public float jumpRate;          // These 4 are for jumping/double
    private bool doubleJumped;

    private CharacterController controller;
    private Vector3 playerVelocity;     // All to do with the character controller
    private bool groundedPlayer;

    private Vector3 resetPos;
    private Quaternion resetRot;            // For resetting the player on a Reset case

    [SerializeField]
    public float playerSpeed = 2.0f;
    public float sprintSpeed = 4.0f;
    public float currentSpeed;          // These 4 handle walking/sprinting
    public float initialSpeed;

    //public Slider sprintSlider;

    [SerializeField]
    private float pushStrength = 2.0f;  
    //private Controls playerControls;

    //public float stamina;
    //float maxStamina;
    //public float dValue;              // THese were used in managing stamina 
    //public bool rechargingStamina;    // Stamina was removed after the first test
    //public bool sprinting = false;

    [SerializeField]
    private float gravityValue = -9.81f;

    public bool spotted;
    public float stealthHealth;
    float maxStealth;
    public float spottingRate;      // Handles stealth
    public bool beingSpotted;
    public float stealthingRate;
    public float stealthColour;     // Stealth colour is used to linearly change the colour of the guard sight lights

    [SerializeField]
    private float jumpHeight = 3.0f;

    //Double jumping
    private bool _canDoubleJump;
    private bool _jumping;

    private bool leftTrueRightFalse = true;
    public bool LeftTrueRightFalse { get => leftTrueRightFalse; set => leftTrueRightFalse = value; }

    public Slider strengthSlider;       // Slider for kicking strength

    public bool Kicking = false;
    public float kickingStrength;
    public float maxKicking;        // These 4 for kicking
    public float strengthRate;

    public GameController gameController;   // Connects to the game controller for switching between players

    public bool Attract;
    public bool Repel;              // Magnetism

    public Light lightSource;       // This light is for magnetism

    [SerializeField]
    public AudioSource checkpointSound;     // Sounds
    public AudioSource coinPickupSound;
    public AudioSource walkingSound;
    public AudioSource jumpSound;   // For double jumping
    public AudioSource doubleJumpSound;

    private bool playingWalking = false;    // These 2 bools handle the sprinting sound
    private bool soundPlaying = false;

    public int coins;           // For collectables
    public Text coinText;
    private string coinTranslated;

    public bool sharkFood = false;  // For shark guards in the ice level

    void Awake()        //  on awake it sets up starting conditions
    {
        //playerControls = new Controls();
        dead = false;
        spotted = false;
        beingSpotted = false;       

        lightSource = GetComponentInChildren<Light>();

        Attract = false;
        Repel = false;

        coins = 0;

    }

    private void OnEnable()
    {
        // playerControls.Player.Enable();
    }

    private void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();

        resetPos = transform.position;  // FOr resetting
        resetRot = transform.rotation;

        maxJump = jumpCharge;

        //maxStamina = stamina;

        maxStealth = stealthHealth;

        currentSpeed = playerSpeed;

        initialSpeed = playerSpeed;
    }

    void Update()
    {
        if (LeftTrueRightFalse) // Left is free side
        {
            KickingToggle();
            Grounded();
            Jump();
            DoubleJump();
            IncreaseJumpCharge();

            Sprinting();
            Movement();
            ResetPosition();
            InputtingMagnetism();
            MagentismLight();
            Stealth();
            TextTranslation();
            BeingDead();

            if (transform.position.y <= -10)
            {
                dead = true;
            }
        }
        
        else        // The functions here are those active when the player is on the grid side
        {           // So just maintaining appearances and resetting
            ResetPosition();
            MagentismLight();
            Stealth();
            BeingDead();
        }
    }

    

    private void Grounded()     // This works out when the player is on the ground/not in the air
    {
        groundedPlayer = controller.isGrounded;

        if(groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;

            _jumping = false;

            //_canDoubleJump = true;

            doubleJumped = false;

            //Debug.Log("grounded");

          //  playWalking = false;

           
        }

        playerVelocity.y += gravityValue * Time.deltaTime;

        
    }

    private void Movement()         // Horizontal movement
    {
        relativeDirection = mainCamera.transform.position - transform.position;     // The vector from the camera to player

        Vector3 inBetween = new Vector3(relativeDirection.x, 0, relativeDirection.z);

        directionNorm = inBetween.normalized;

       // Debug.Log(relativeDirection);     // Debugging

        if (Input.GetKey(KeyCode.W))        // If press W then move horizontally along the vector from camera to player
        {
            Vector3 move = new Vector3(-directionNorm.x, 0, -directionNorm.z);  // No Y element

            controller.Move(move * Time.deltaTime * currentSpeed);

            transform.rotation = cameraRotation.transform.rotation; // Set player rotation to camera rotation so they match
        }
        
        /*
        if (Input.GetKeyDown(KeyCode.A))
        {}
                                        // These are no longer needed in the player script as the camera rotates instead
        if (Input.GetKeyDown(KeyCode.D))
        {}
        */ 

        if (Input.GetKey(KeyCode.S))        // If press S then move horizontally backwards from the vector from camera to player
        {
            Vector3 move = new Vector3(directionNorm.x, 0, directionNorm.z);

            controller.Move(move * Time.deltaTime * currentSpeed);

            transform.rotation = new Quaternion(0, 1, 0, 0) * cameraRotation.transform.rotation;        // This rotates the player to face the camera when walking backwards
        }                           // This multiplication rotates Y by 180 

        //Debug.Log(controller.velocity.magnitude);     // Walking sound debugging

       controller.Move(playerVelocity * Time.deltaTime);        // For vertical movement
        
    }

    
    private void Jump()     // Vertical movement
    {
        // Changes the height position of the player..
        if(Input.GetButtonDown("Jump") && groundedPlayer && jumpCharge >= maxJump)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
            _jumping = true;
                                // When pressing Space, increases the player's Y velocity
            jumpCharge = 0;
            _canDoubleJump = false;

            jumpSound.Play();   // First jump sound

            //playingWalking = false;
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
    }
    
    private void DoubleJump()   // Jumping whilst airborne
    {
        if (_jumping == true && _canDoubleJump == true)
        {
            // Changes the height position of the player..
            if (Input.GetButtonDown("Jump") && jumpCharge >= maxJump)
            {
                playerVelocity.y += Mathf.Sqrt(jumpHeight * -6.0f * gravityValue);
                                                // The double jump is a stronger jump than the 1st
                jumpCharge = 0;

               // _canDoubleJump = false;

                doubleJumped = true;

                doubleJumpSound.Play();     // Double jump sound
            }
        }
        playerVelocity.y += gravityValue * Time.deltaTime;
    }

    //Jumping 
    private void IncreaseJumpCharge()   // This function can be used to add a delay between when the player can jump
    {                                   // but it felt odd so I just made the charge rate super high so it's like they can 
        if (jumpCharge < maxJump)       // jump instantly, but there's a very slight delay
        {                               // It functions the same way as stealth health charging
            jumpCharge += jumpRate * Time.deltaTime;

            _canDoubleJump = false;     
        }

        else if (jumpCharge >= maxJump && _jumping)
        {
            if (doubleJumped == false)
            {
                _canDoubleJump = true;
            }

            else if (doubleJumped == true)
            {
                _canDoubleJump = false;
            }
        }
    }


    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(hit.collider.CompareTag("PushableObject"))
        {
            var box = hit.rigidbody;
            if (box != null)
            {
                if (Kicking)
                {
                    pushStrength = kickingStrength;

                    Vector3 pushDirection = new Vector3(hit.moveDirection.x, 1f, hit.moveDirection.z);      // Added y component for height
                    box.velocity = pushDirection * pushStrength * 2f;


                    float rotX = hit.moveDirection.x * pushStrength * 3;
                    float rotY = hit.moveDirection.y * pushStrength * 2;
                    float rotZ = hit.moveDirection.z * pushStrength * 3;
                    box.AddTorque(rotZ, rotY, -rotX);                   // This rotates the box in the right direction
                                                                        // when kicking
                }
                else
                {
                    Vector3 pushDirection = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
                    box.velocity = pushDirection * pushStrength * 2f;

                    
                }
            }
        }
    }

   

    private void ResetPosition()        // Reset as normal
    {
        if (Input.GetKey(KeyCode.R))
        {
            Reset();
        }
    }

    void Reset()
    {
        transform.position = resetPos;
        transform.rotation = resetRot;

        //spotted = false;            // Couldn't get the waiting for seconds working so for now it's alarmed till Reset
                                    // but it needs to be here anyways

        stealthHealth = maxStealth;
        sharkFood = false;
    }

    void KickingToggle()
    {
        if (Input.GetKey(KeyCode.F))        // Hold F down to increase kick strength
        {
            WindingUp();
        }

        else
        {
            WindingDown();  // decreases kicking strength when not holding down F
        }

        if (kickingStrength > 0)        // If any kicking strength then kick
        { 
            Kicking = true;
        }

        else if (kickingStrength <= 0)
        {
            Kicking = false;
            strengthSlider.gameObject.SetActive(false);
        }

        if (Kicking)
        {
            Debug.Log("Kicking");
        }
    }

    void WindingUp()                // This increases kicking strength
    {
        if (kickingStrength < maxKicking)
        {
            strengthSlider.gameObject.SetActive(true);

            kickingStrength += strengthRate * Time.deltaTime;

            strengthSlider.value = kickingStrength;
        }
    }

    void WindingDown()              // This decreases kicking strength
    {
        if (kickingStrength >= 0)
        {
           strengthSlider.gameObject.SetActive(true);

            kickingStrength -= (strengthRate / 3f) * Time.deltaTime;    // Decreases slower than it increases

           strengthSlider.value = kickingStrength;
        }
    }

    void InputtingMagnetism()       // Toggles magnetism with button input
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (Attract)
            {
                Attract = false;
                Repel = false;
            }

            else
            {
                Attract = true;
                Repel = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (Repel)
            {
                Attract = false;
                Repel = false;
            }

            else
            {
                Attract = false;
                Repel = true;
            }
        }
    }

    void MagentismLight()       // Handles magnetism light
    {
        if (Attract == true)
        {
            lightSource.color = Color.green;
        }

        else if (Repel == true)
        {
            lightSource.color = Color.blue;
        }

        else
        {
            lightSource.color = Color.clear;
        }
    }

    /*
    //Running 
    private void DecreaseEnergy()
    {
        if (stamina != 0)
        {
            stamina -= dValue * Time.deltaTime;
        }
    }
    //Running 
    private void IncreaseEnergy()
    {
        if (stamina < maxStamina)
        {
            stamina += dValue * Time.deltaTime;
        }
    }

    //Running 
    private void Sprinting()
    {
        if (Input.GetKey(KeyCode.LeftShift) && rechargingStamina == false) 
        {
            DecreaseEnergy();
            currentSpeed = sprintSpeed;
            sprinting = true;

            if (stamina < 0)                // This was added as sprinting didn't stop, even after stamina hit 0
            {
                currentSpeed = playerSpeed;
                sprinting = false;
            }
        }

        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            currentSpeed = playerSpeed;
            sprinting = false;
        }    
    }

    //Running 
    private void RechargingStamina()
    {
        if (stamina < 0)
        {
            rechargingStamina = true;
        }

        else if (stamina >= maxStamina)
        {
            rechargingStamina = false;
        }

        if (stamina < maxStamina && sprinting == false)
        {
            IncreaseEnergy();
        }
    }

    
    // Running
    public void SprintSlider()
    {
        sprintSlider.value = stamina;

        if (stamina >= maxStamina)
        {                                                   // Old limited sprinting system
            sprintSlider.gameObject.SetActive(false);
        }
        else
        {
            sprintSlider.gameObject.SetActive(true);
        }
    }
    */

    private void Sprinting()        // This handles walking faster with Shift and sounds
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed = sprintSpeed;

            if (groundedPlayer)
            {
                playingWalking = true;
            }
                        // If grounded then the walking sound can be played
            else
            {
                playingWalking = false;
            }
        }

        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            currentSpeed = playerSpeed;

            playingWalking = false;
        }


        if (playingWalking)
        {
            if (soundPlaying)
            {}

            else if (!soundPlaying)
            {
                walkingSound.Play();
                soundPlaying = true;
            }
        }

        else if (!playingWalking)
        {
            walkingSound.Stop();
            soundPlaying = false;
        }
    }

    //Steath /////////////////
    //Stealth //
    private void DecreaseStealth()      // This function is used when in enemy vision to reduce stealth health
    {
        if (stealthHealth != 0)  
        {
            stealthHealth -= spottingRate * Time.deltaTime;
        }
    }

    //Stealth //
    private void IncreaseStealth()
    {
        if (stealthHealth < maxStealth)     // If stealth health is less than max, increase at the stealthing rate
        {
            stealthHealth += stealthingRate * Time.deltaTime;
        }
    }

    //Stealth //
    private void Stealth()
    {
        if (beingSpotted == true)
        {
            DecreaseStealth();  // decrease stealth health if in enemy trigger
        }

        RechargingStealth();

        stealthColour = stealthHealth / maxStealth;     // Since the stealthColour will act as 't' in the Color.Lerp eqn
    }                                                   // it needs to be between 0-1, so dividing stealthHealth by max will give that
                                                        // This is then made available to guards
    //Stealth //
    private void RechargingStealth()
    {
        if (stealthHealth < 0)
        {
            spotted = true;
            beingSpotted = false;
        }

        else if (stealthHealth >= maxStealth)
        {
            spotted = false;        // if stealth health is greater than max then no longer spotted
        }

        if (beingSpotted == false)
        {
            IncreaseStealth();      // if not in enemy triggerzone then increase stealth health
        }
    }

    // Collectable UI text
    private void TextTranslation()      // I brute forced this, 5 repeats isn't that bad
    {                                   // For the coin text as part of the HUD
        if (coins == 0)
        {
            coinText.text = "Coins: 0 / 5";
        }

        else if (coins == 1)
        {
            coinText.text = "Coins: 1 / 5";
        }
        
        else if (coins == 2)
        {
            coinText.text = "Coins: 2 / 5";
        }

        else if (coins == 3)
        {
            coinText.text = "Coins: 3 / 5";
        }

        else if (coins == 4)
        {
            coinText.text = "Coins: 4 / 5";
        }

        else if (coins == 5)
        {
            coinText.text = "Coins: 5 / 5";
        }
    }    
    
    private void BeingDead()
    {
        if (dead == true)       // reset if dead
        {
            Reset();
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy"))       // if collide with an enemy then dead
        {
            // Debug.Log("Hit enemy");
            dead = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Collectable"))     // Collectables are coins, upon colliding increment the coin counter
        {                                       //  and play the sound
            coins++;                            // and set the coin object inactive
            coinPickupSound.Play();
            other.gameObject.SetActive(false);
        }
    }

        private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            beingSpotted = true;        // enemy trigger is their cone of vision
        }

        if (other.gameObject.CompareTag("SpawnPoint"))      // Moved to TriggerStay to fix bug of staying 'dead'
        {
            dead = false;

            if (other.transform.position == resetPos)   // if re-entering the same checkpoint, we dont want the sound playing again
            {                                          
                Debug.Log("Same checkpoint");

            }

            else
            {
                checkpointSound.Play(); // if entering a new checkpoint then play the sound

                resetPos = other.transform.position;

            }
        }

        if (other.gameObject.CompareTag("Water"))   // if in the water, player is targetted by sharks and moves half as fast
        {
            sharkFood = true;

            playerSpeed = initialSpeed / 2;

            sprintSpeed = initialSpeed;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))   // to stop being spotted when out of the enemy's sight trigger
        {
            beingSpotted = false;
        }

        if (other.gameObject.CompareTag("Water"))   // on exit the water, stop being hunted by sharks and resume normal speed
        {
            sharkFood = false;

            playerSpeed = initialSpeed;
            sprintSpeed = 2 * initialSpeed;
        }
    }
    /*
        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag("Enemy"))
            {
                Debug.Log("Left sight");

                StartCoroutine(WaitingFive());

                Debug.Log("waited 5");

                spotted = false;
            }
        }

        IEnumerator WaitingFive()
        {
            //yield on a new YieldInstruction that waits for 5 seconds.
            yield return new WaitForSecondsRealtime(5);

        }
        */
}
