// This script controls the player's movement and physics within the game



using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public bool drawDebugRaycasts = true;   //Should the environment checks be visualized

    [Header("Movement Properties")]
    public float speed = 8f;                //Player speed
    public float coyoteDuration = .05f;     //How long the player can jump after falling
    public float maxFallSpeed = -25f;       //Max speed player can fall

    [Header("Jump Properties")]
    public float jumpForce = 6.3f;          //Initial force of jump

    [Header("Stuned Properties")]
    public float stunedTime = 1.0f;

    [Header("Dash Properties")]
    public float dashForce = 4.3f;          //Initial force of dash
    public float dashCooldown = 2.1f;       //How long the dash can't be performed


    public float dashTime = 1.1f;            //How long the dash last

    [Header("Environment Check Properties")]
    public float footOffset = .4f;			//X Offset of feet raycast
    public float handsOffset = .4f;          //Y Offset of feet raycast
    public float groundDistance = .2f;		//Distance player is considered to be on the ground
    public float wallDistance = .2f;      //Distance player is considered to be on the ground
    public LayerMask groundLayer;           //Layer of the ground

    [Header("Status Flags")]
    public bool isOnGround;                 //Is the player on the ground?
    public bool isOnWall;                 //Is the player on the wall?
    public bool isDash;                     //Is the player isDashing?
    public bool isDead;                     //Is the player isDashing?
    public bool isStuned;                     //Is the player isDashing?


    [Header("Sounds ")]
    public AudioClip dashSound;
    public AudioClip jumpSound;
    public AudioClip killSound;


    PlayerInput playerInput;                        //The current inputs for the player
    BoxCollider2D bodyCollider;				//The collider component
    Rigidbody2D rigidBody;                  //The rigidbody component
    Animator animator;                  //The rigidbody component
    TrailRenderer trail;                  //The rigidbody component
     AudioSource audioSource;

    float dashTimeStamp = 0f;                   //Variable to hold dash duration
    float stunTimeStamp = 0f;                   //Variable to hold dash duration
    float coyoteTime;                       //Variable to hold coyote duration
    float playerHeight = 0f;				//Height of the player

    float originalXScale;                   //Original scale on X axis
    float originalGravity;                  //Original scale on X axis
    float originalTrailTime;                  //Original scale on X axis
    int direction = 1;                      //Direction player is facing

    Vector2 colliderStandSize;              //Size of the standing collider
    Vector2 colliderStandOffset;            //Offset of the standing collider
    Vector2 colliderCrouchSize;             //Size of the crouching collider
    Vector2 colliderCrouchOffset;           //Offset of the crouching collider

    const float smallAmount = .05f;         //A small amount used for hanging position

    public ParticleSystem jumpParticle;
    public ParticleSystem deathParticle;
    public ParticleSystem deathParticle2;

    public int Direction { get => direction; }

    void Start()
    {
        isDash = false;
        dashTimeStamp = Time.time - dashCooldown;
        //Get a reference to the required components
        playerInput = GetComponent<PlayerInput>();
        rigidBody = GetComponent<Rigidbody2D>();
        bodyCollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        trail = GetComponent<TrailRenderer>();
        audioSource = GetComponent<AudioSource>();

        if (trail)
        {
            originalTrailTime = trail.time;
            trail.time = 0;
        }

        //Record the original x scale of the player
        originalXScale = transform.localScale.x;


        originalGravity = rigidBody.gravityScale;
        //Record the player's height from the collider
        playerHeight = bodyCollider.size.y;

        //Record initial collider size and offset
        colliderStandSize = bodyCollider.size;
        colliderStandOffset = bodyCollider.offset;

        //Calculate crouching collider size and offset
        colliderCrouchSize = new Vector2(bodyCollider.size.x, bodyCollider.size.y / 2f);
        colliderCrouchOffset = new Vector2(bodyCollider.offset.x, bodyCollider.offset.y / 2f);
    }

    void FixedUpdate()
    {
        if (isDead)
            return;
        if(isStuned)
        {
            if (stunTimeStamp + stunedTime < Time.time)
                isStuned = false;
            else 
            return;
        }

        //Check the environment to determine status
        PhysicsCheck();

        //Process ground and air movements
        GroundMovement();
        MidAirMovement();
        Dash();
        UpdateAnimator();
    }

    private void UpdateAnimator()
    {
        if (animator == null)
            return;

        if (isDash)
        {
            animator.SetTrigger("dash");
        }
        else if (isOnGround)
        {
            if (rigidBody.velocity.x != 0)
                animator.SetTrigger("walk");
            else
                animator.SetTrigger("idle");
        }
        else
        {
            if (rigidBody.velocity.y > 0)
            { 
            animator.SetTrigger("jump");
            
            }
            else
                animator.SetTrigger("fall");
        }
    }
    public void Push(float force, Vector2 direction)
    {
        force *= 5;
        Debug.Log("push " + force + " " +  direction);
        isStuned = true;
        stunTimeStamp = Time.time;
        animator.SetTrigger("jump");
        this.rigidBody.AddForce(direction.normalized * force, ForceMode2D.Impulse);
    }

    public void Kill()
    {
        if (isDead)
            return;
        deathParticle.Play(true);
        deathParticle2.Play(true);
        animator.SetTrigger("die");

        animator.ResetTrigger("jump");
        animator.ResetTrigger("dash");
        animator.ResetTrigger("idle");
        animator.ResetTrigger("fall");
        animator.ResetTrigger("walk");

        isDead = true;

        if (audioSource != null && killSound != null)
            audioSource.PlayOneShot(killSound);

    }

    void Dash()
    {
        if (playerInput.dashPressed && !isDash && dashTimeStamp + dashCooldown < Time.time)
        {
        
            //...record the time the player will be able to dash again
            dashTimeStamp = Time.time;
            isDash = true;
            //rigidBody.AddForce(new Vector2(dashForce * direction, 0f ), ForceMode2D.Impulse);
            rigidBody.velocity = new Vector2(dashForce * direction, 0f);
            rigidBody.gravityScale = 0;

            if (audioSource != null && dashSound != null)
                audioSource.PlayOneShot(dashSound);

            if (trail)
            {
                //trail.enabled = true;
                trail.time = originalTrailTime; 
                trail.Clear();
            }
        }

        if (isDash && dashTimeStamp + dashTime < Time.time)
        {
            isDash = false;
            rigidBody.gravityScale = originalGravity;

            if (trail)
            {
                //trail.enabled = false;
                trail.time = 0;
            }
        }

    }


    void PhysicsCheck()
    {
        //Start by assuming the player isn't on the ground and the head isn't blocked
        isOnGround = false;


        //Cast rays for the left and right foot
        RaycastHit2D leftCheck = Raycast(new Vector2(-footOffset, 0f), Vector2.down, groundDistance);
        RaycastHit2D rightCheck = Raycast(new Vector2(footOffset, 0f), Vector2.down, groundDistance);

        //If either ray hit the ground, the player is on the ground
        if (leftCheck || rightCheck)
        {
            isOnGround = true;
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, 0f);
        }

        //Cast rays for the left and right foot
        RaycastHit2D topCheck = Raycast(new Vector2(0f, -handsOffset), Vector2.right * direction, wallDistance);
        RaycastHit2D middleCheck = Raycast(new Vector2(0f, 0f), Vector2.right * direction, wallDistance);
        RaycastHit2D bottomCheck = Raycast(new Vector2(0f, handsOffset), Vector2.right * direction, wallDistance);

        isOnWall = topCheck || bottomCheck || middleCheck;


    }

    void GroundMovement()
    {
        //Calculate the desired velocity based on inputs
        float xVelocity = speed * playerInput.horizontal;

        //If the sign of the velocity and direction don't match, flip the character
        if (xVelocity * direction < 0f)
            FlipCharacterDirection();

        //Apply the desired velocity 
        if (!isDash && !isOnWall)
            rigidBody.velocity = new Vector2(xVelocity, rigidBody.velocity.y);

        //If the player is on the ground, extend the coyote time window
        if (isOnGround)
            coyoteTime = Time.time + coyoteDuration;
        
    }

    void MidAirMovement()
    {
        //If the jump key is pressed AND the player isn't already jumping AND EITHER
        //the player is on the ground or within the coyote time window...
        if (playerInput.jumpPressed && (isOnGround || coyoteTime > Time.time))
        {
            //...The player is no longer on the groud and is jumping...
            isOnGround = false;
            jumpParticle.Play(true);

            if (audioSource != null && jumpSound != null)
                audioSource.PlayOneShot(jumpSound);


            //...add the jump force to the rigidbody...
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, 0);

            rigidBody.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);

            //...and tell the Audio Manager to play the jump audio
            //	AudioManager.PlayJumpAudio();
        }


        ////If player is falling to fast, reduce the Y velocity to the max
        //if (rigidBody.velocity.y < maxFallSpeed)
        //rigidBody.velocity = new Vector2(rigidBody.velocity.x, maxFallSpeed);
    }

    void FlipCharacterDirection()
    {
        //Turn the character by flipping the direction
        direction *= -1;

        //Record the current scale
        Vector3 scale = transform.localScale;

        //Set the X scale to be the original times the direction
        scale.x = originalXScale * direction;

        //Apply the new scale
        transform.localScale = scale;
    }


    //These two Raycast methods wrap the Physics2D.Raycast() and provide some extra
    //functionality
    RaycastHit2D Raycast(Vector2 offset, Vector2 rayDirection, float length)
    {
        //Call the overloaded Raycast() method using the ground layermask and return 
        //the results
        return Raycast(offset, rayDirection, length, groundLayer);
    }

    RaycastHit2D Raycast(Vector2 offset, Vector2 rayDirection, float length, LayerMask mask)
    {
        //Record the player's position
        Vector2 pos = transform.position;

        //Send out the desired raycasr and record the result
        RaycastHit2D hit = Physics2D.Raycast(pos + offset, rayDirection, length, mask);

        //If we want to show debug raycasts in the scene...
        if (drawDebugRaycasts)
        {
            //...determine the color based on if the raycast hit...
            Color color = hit ? Color.red : Color.green;
            //...and draw the ray in the scene view
            Debug.DrawRay(pos + offset, rayDirection * length, color);
        }

        //Return the results of the raycast
        return hit;
    }
}
