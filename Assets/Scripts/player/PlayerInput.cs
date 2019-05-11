// This script handles inputs for the player. It serves two main purposes: 1) wrap up
// inputs so swapping between mobile and standalone is simpler and 2) keeping inputs
// from Update() in sync with FixedUpdate()

using Rewired;
using UnityEngine;

//We first ensure this script runs before all other player scripts to prevent laggy
//inputs
[DefaultExecutionOrder(-100)]
public class PlayerInput : MonoBehaviour
{
    public InputID inputID= InputID.Player_One;
    public bool testTouchControlsInEditor = false;	//Should touch controls be tested?
	public float verticalDPadThreshold = .5f;		//Threshold touch pad inputs
	//public Thumbstick thumbstick;					//Reference to Thumbstick
//	public TouchButton jumpButton;					//Reference to jump TouchButton

	[HideInInspector] public float horizontal;		//Float that stores horizontal input
	[HideInInspector] public bool jumpHeld;			//Bool that stores jump pressed
	[HideInInspector] public bool jumpPressed;		//Bool that stores jump held
	[HideInInspector] public bool crouchHeld;		//Bool that stores crouch pressed
	[HideInInspector] public bool crouchPressed;	//Bool that stores crouch held
    [HideInInspector] public bool dashHeld;      
    [HideInInspector] public bool dashPressed;   
    [HideInInspector] public bool fireHeld;      
    [HideInInspector] public bool firePressed;   


    bool readyToClear;                              //Bool used to keep input in sync
    private Player player; // The Rewired Player
    private bool initialized= false;
    void Update()
	{

        if (!ReInput.isReady) return; // Exit if Rewired isn't ready. This would only happen during a script recompile in the editor.
        if (!initialized) Initialize(); // Reinitialize after a recompile in the editor

        //Clear out existing input values
        ClearInput();

		//If the Game Manager says the game is over, exit
		//if (GameManager.IsGameOver())
			//return;

		//Process keyboard, mouse, gamepad (etc) inputs
		ProcessInputs();
		//Process mobile (touch) inputs
	//	ProcessTouchInputs();

		//Clamp the horizontal input to be between -1 and 1
		horizontal = Mathf.Clamp(horizontal, -1f, 1f);
	}
    private void Initialize()
    {
        // Get the Rewired Player object for this player.
        player = ReInput.players.GetPlayer((int)inputID);

        initialized = true;
    }

   
     void FixedUpdate()
	{
            //In FixedUpdate() we set a flag that lets inputs to be cleared out during the 
            //next Update(). This ensures that all code gets to use the current inputs
            readyToClear = true;
	}

	void ClearInput()
	{
		//If we're not ready to clear input, exit
		if (!readyToClear)
			return;

		//Reset all inputs
		horizontal		= 0f;
		jumpPressed		= false;
		jumpHeld		= false;
		crouchPressed	= false;
		crouchHeld		= false;
        dashPressed = false;
        dashHeld = false;
        firePressed = false;
        fireHeld = false;

        readyToClear	= false;
	}

	void ProcessInputs()
	{
        if(player!=null)
        {

            horizontal += player.GetAxis("Move Horizontal"); // get input by name or action id

            jumpPressed       = jumpPressed || player.GetButtonDown("Jump");
            jumpHeld      = jumpHeld || player.GetButton("Jump");

            dashPressed = dashPressed || player.GetButtonDown("Dash");
            dashHeld = dashHeld || player.GetButton("Dash");

            firePressed = firePressed || player.GetButtonDown("Fire");
            fireHeld = fireHeld || player.GetButton("Fire");


        }

    }

	
}
