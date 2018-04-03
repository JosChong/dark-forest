using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Controller2D))]
public class Player : MonoBehaviour
{
    public float maxJumpHeight = 3f;
    public float minJumpHeight = 1f;
    public float timeToJumpApex = .2f;
    private float accelerationTimeAirborne = .2f;
    private float accelerationTimeGrounded = .1f;
    private float moveSpeed = 6f;

    public Vector2 wallJumpClimb;
    public Vector2 wallJumpOff;
    public Vector2 wallLeap;

    public bool canDoubleJump;
    public bool isDoubleJumping = false;

    public float wallSlideSpeedMax = 3f;
    public float wallStickTime = .25f;
    private float timeToWallUnstick;
    public Slider healthslider;

    private float gravity;
    private float maxJumpVelocity;
    private float minJumpVelocity;
    private Vector3 velocity;
    private float velocityXSmoothing;

    private Controller2D controller;

    private Vector2 directionalInput;
    private bool wallSliding;
    private int wallDirX;

    public float lastLookDir;
    private SpriteRenderer sprite;

    private Animator animator;

    public bool isAlive = true;
    public int health = 100;
    
    public bool ifInstantiated;

    private Color origColor;

    private BoxCollider2D collider;

    public bool tutorialOn;

    private float initialY;

    public bool playerActive = true;
    
    private void Start()
    {
        controller = GetComponent<Controller2D>();
        gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);
        sprite = GetComponentInChildren<SpriteRenderer>();
        lastLookDir = 1;

        collider = GetComponent<BoxCollider2D>();
        origColor = gameObject.GetComponentInChildren<SpriteRenderer>().color;
        animator = GetComponentInChildren<Animator>();
        
        ifInstantiated = false;

        initialY = gameObject.transform.position.y;
        // for start menu and tutorial
        if (SceneManager.GetActiveScene().name == "Tutorial")
        {
            tutorialOn = true;
        }
        else
        {
            tutorialOn = false;
        }
        
        if (tutorialOn)
        {
            Time.timeScale = 0;
            GameObject menu = Instantiate(Resources.Load("menus/initialMenu"), GameObject.FindWithTag("MainCanvas").transform) as GameObject;
        }
        
        Slider[] sliders = FindObjectsOfType<Slider>();
        foreach (Slider s in sliders)
        {
            if (s.CompareTag("HealthBar"))
            {
                healthslider = s;
            }
        }
    }

    private void Update()
    {
        if (gameObject.transform.position.y < initialY - 50f && ifInstantiated==false)
        {
            isAlive = false;
            Instantiate(Resources.Load("RestartButton"), GameObject.FindWithTag("MainCanvas").transform);
            ifInstantiated = true;
        }
        // could maybe update slider health bar in player.cs file
        controller.enabled = false;
        if (health <= 0)
        {
            isAlive = false;
        }
        if (isAlive)
        {
            if (playerActive)
            {
                Time.timeScale = 1;
            }
            CalculateVelocity();
            HandleWallSliding();

            controller.Move(velocity * Time.deltaTime, directionalInput);

            if (controller.collisions.above || controller.collisions.below)
            {
                velocity.y = 0f;
                animator.SetBool("isJumping", false);
            }
        }
        else
        {
            Time.timeScale = 0;
            
            //animator.speed = 0;
            controller.enabled = false;
            //velocity.x = 0;
            //velocity.y = 0;
            //            animator.SetTrigger ("GameOver"); // would need to impliment

            if (!ifInstantiated)
            {
                Instantiate(Resources.Load("RestartButton"), GameObject.FindWithTag("MainCanvas").transform);
                ifInstantiated = true;
            }
            
        }
    }

    public void tintPlayer()
    {
        StartCoroutine(coTintPlayer());
    }

    public IEnumerator coTintPlayer()
    {
        Color modifiedColor = new Color(origColor.r-.5f, origColor.g - 1.0f, origColor.b-1.0f, 1f);
        gameObject.GetComponentInChildren<SpriteRenderer>().color = modifiedColor;
        yield return new WaitForSeconds(0.20f);
        gameObject.GetComponentInChildren<SpriteRenderer>().color = origColor;
        yield return null;
    }
    
    public void SetDirectionalInput(Vector2 input)
    {
        directionalInput = input;
        
        // Flip sprite to face move direction
        float direction;
        if (input.x != 0)
        {
            direction = input.x / Mathf.Abs(input.x);
            lastLookDir = Mathf.Sign(input.x);
        }
        else
        {
            direction = lastLookDir;
        }
        sprite.transform.localScale = new Vector3(direction * 0.6f, 0.6f, 0);
    }

    public void OnJumpInputDown()
    {
        if (wallSliding)
        {
            if (wallDirX == directionalInput.x)
            {
                velocity.x = -wallDirX * wallJumpClimb.x;
                velocity.y = wallJumpClimb.y;
            }
            else if (directionalInput.x == 0)
            {
                velocity.x = -wallDirX * wallJumpOff.x;
                velocity.y = wallJumpOff.y;
            }
            else
            {
                velocity.x = -wallDirX * wallLeap.x;
                velocity.y = wallLeap.y;
            }
            isDoubleJumping = false;
        }
        if (controller.collisions.below)
        {
            velocity.y = maxJumpVelocity;
            isDoubleJumping = false;
        }
        if (canDoubleJump && !controller.collisions.below && !isDoubleJumping && !wallSliding)
        {
            velocity.y = maxJumpVelocity;
            isDoubleJumping = true;
        }
    }

    public void OnJumpInputUp()
    {
        if (velocity.y > minJumpVelocity)
        {
            velocity.y = minJumpVelocity;
        }
    }

    public void TakeDamage(int amount)
    {
        tintPlayer();
        health -= amount;
        healthslider.value -= amount;
    }

    public bool GiveHealth(int amount)
    {
        bool gaveHealth = false;
        if (health < 100)
        {
            if (health + amount > 100)
            {
                health = 100;
                healthslider.value = 100;
            }
            else
            {
                health += amount;
                healthslider.value += amount;
            }
            gaveHealth = true;
        }

        return gaveHealth;
    }

    private void HandleWallSliding()
    {
        wallDirX = (controller.collisions.left) ? -1 : 1;
        wallSliding = false;
        if ((controller.collisions.left || controller.collisions.right) && !controller.collisions.below && velocity.y < 0)
        {
            wallSliding = true;

            if (velocity.y < -wallSlideSpeedMax)
            {
                velocity.y = -wallSlideSpeedMax;
            }

            if (timeToWallUnstick > 0f)
            {
                velocityXSmoothing = 0f;
                velocity.x = 0f;
                if (directionalInput.x != wallDirX && directionalInput.x != 0f)
                {
                    timeToWallUnstick -= Time.deltaTime;
                }
                else
                {
                    timeToWallUnstick = wallStickTime;
                }
            }
            else
            {
                timeToWallUnstick = wallStickTime;
            }
        }
    }

    private void CalculateVelocity()
    {
        float targetVelocityX = directionalInput.x * moveSpeed;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below ? accelerationTimeGrounded : accelerationTimeAirborne));
        velocity.y += gravity * Time.deltaTime;
    }

//    private void PlayerDead()
//    {
//        velocity.x = 0;
//        velocity.y = 0;
//    }
}
