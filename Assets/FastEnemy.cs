using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FastEnemy : MonoBehaviour {
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private int health = 1000;
    private int damageDealt = 50;
    private int normalMoveSpeed = 1; //idle move speed, will not be modified during runtime
    private int sprintSpeed = 3; //sprint move speed, will not be modified during runtime
    public int moveSpeed = 1; //current move speed, WILL be modified during runtime
    private float timeCheck; //to help with deciding when to be moving idly, not moving at all, or sprinting
    public bool waiting = false; //flag for whether we're currently waiting or not
    public bool sprinting = false; //flag for whether we're currently sprinting or not
    //if both of the above are false we're assumed to be moving idly or at normal speed
    private float idleCooldown = 5f; //time that the enemy will move at regular speed
    private float waitTime = 5f; //time that the enemy will sit without movement
    private float sprintTime = 3f; //time that the enemy will move quickly

    public Color origColor;

    private bool goingLeft = true;

    public Slider[] sliders;
    public Slider healthslider;
    
    private bool isDead = false;
    private float deathTime;
    private Animator animator;
    private PolygonCollider2D collider;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();

        // Set initial movement
        rb.velocity = Vector2.left * moveSpeed;

        sliders = FindObjectsOfType<Slider>();
        foreach (Slider s in sliders)
        {
            if (s.CompareTag("HealthBar"))
            {
                healthslider = s;
            }
        }
        origColor = gameObject.GetComponentInChildren<SpriteRenderer>().color;

        timeCheck = Time.time;
        
        animator = GetComponent<Animator>();
        collider = GetComponent<PolygonCollider2D>();
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        // If colliding with the player
        if (coll.gameObject.GetComponent<Player>() != null)
        {

            var playerCollidedWith = coll.gameObject.GetComponent<Player>();
            coll.gameObject.GetComponent<Player>().tintPlayer();
            healthslider.value -= damageDealt;
            coll.gameObject.GetComponent<Player>().health -= damageDealt;

        }
		
        // Reverse direction on collision;
        if (coll.gameObject.layer == 9)
        {
            if (goingLeft) {
                rb.velocity = Vector2.right * moveSpeed;
                goingLeft = false;
                sprite.transform.localScale = new Vector3(2f, 2f, 0);
            } else {
                rb.velocity = Vector2.left * moveSpeed;
                goingLeft = true;
                sprite.transform.localScale = new Vector3(-2f, 2f, 0);
            }
        }
    }

    public void DealDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            deathTime = Time.time;
            animator.SetTrigger("isDead");
            collider.enabled = false;
            isDead = true;
            rb.velocity = Vector2.zero;
            
            transform.localScale = new Vector3(5, 5, 0);
        }
        else
        {
            StartCoroutine(coTintEnemy());
        }
    }

    public IEnumerator coTintEnemy()
    {
       
        Color modifiedColor = new Color(origColor.r - .5f, origColor.g - 1.0f, origColor.b - 1.0f, 1f);
        gameObject.GetComponentInChildren<SpriteRenderer>().color = modifiedColor;
        yield return new WaitForSeconds(0.20f);
        gameObject.GetComponentInChildren<SpriteRenderer>().color = origColor;
        yield return null;
    }

    void Update()
    {
        if (isDead) {
            if (Time.time > deathTime + animator.GetCurrentAnimatorStateInfo(0).length)
            {
                Destroy(gameObject);
            }
        }
        
        else if (!waiting && !sprinting)
        {
            //he's moving around like normal, next step is to wait
            if (Time.time > timeCheck + idleCooldown)
            {
                //time to sit still
                waiting = true;
                moveSpeed = 0; // stop moving
                timeCheck = Time.time;
            }

        }
        else if (waiting && !sprinting)
        {
            //he's sitting still, next step is to sprint
            if (Time.time > timeCheck + waitTime)
            {
                //time to sprint
                waiting = false;
                moveSpeed = sprintSpeed; //go fast
                sprinting = true;
                timeCheck = Time.time;
            }
        }
        else if (!waiting && sprinting)
        {
            //he's sprinting, next step is to go back to idle
            if (Time.time > timeCheck + sprintTime)
            {
                //time to go back to idle
                sprinting = false;
                moveSpeed = normalMoveSpeed;
                timeCheck = Time.time;
            }
        }

        else if (rb.velocity == Vector2.zero && !waiting)
        {
            if (goingLeft)
            {
                rb.velocity = Vector2.right * moveSpeed;
                goingLeft = false;
                sprite.transform.localScale = new Vector3(2f, 2f, 0);
            }
            else
            {
                rb.velocity = Vector2.left * moveSpeed;
                goingLeft = true;
                sprite.transform.localScale = new Vector3(-2f, 2f, 0);
            }
        }

    }
}
