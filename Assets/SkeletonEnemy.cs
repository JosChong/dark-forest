using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SkeletonEnemy : MonoBehaviour {
	private Rigidbody2D rb;
	private SpriteRenderer sprite;
	private int health = 200;
	private int damageDealt = 5;
	private float moveSpeed = 1;
	private bool goingLeft = true;

    public Color origColor;
    public Slider[] sliders;
    public Slider healthslider;
    public float distanceToPlayer;
	
	private bool isDead = false;
	private float deathTime;
	private Animator animator;
	private PolygonCollider2D collider;
    private Player thePlayer;
    private bool shooting; // true when the skeleton is shooting.
    private float shootCooldown = 3;
    private float lastShot = 0;
    private GameObject arrowPrefab;

    public bool shouldDropHealthPot = false;

    RaycastHit2D hit;
    public bool grounded = false;

    public float freezeHeight;

    void Start () {
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
		
		animator = GetComponent<Animator>();
		collider = GetComponent<PolygonCollider2D>();

	    thePlayer = FindObjectOfType<Player>();

	    arrowPrefab = Resources.Load("Arrow") as GameObject;

        int potChance = Random.Range(0, 4);

        if (potChance == 1)
        {
            shouldDropHealthPot = true;
        }
	}
	
	void OnCollisionEnter2D(Collision2D coll) {
		// If colliding with the player
		if (coll.gameObject.GetComponent<Player>() != null)
		{
			coll.gameObject.GetComponent<Player>().TakeDamage(damageDealt);
		}
	}

    private IEnumerator ShootPlayer()
    {
        bool wasGoingLeft = goingLeft;
        Vector3 currVelocity = rb.velocity;
        rb.velocity = Vector3.zero;

        animator.SetTrigger("isAttacking");
        yield return new WaitForSeconds(1);

        Vector2 fireDirection = thePlayer.transform.position - gameObject.transform.position;
        yield return new WaitForSeconds(0.5f);
        SpawnArrow(fireDirection);

        if (!isDead)
        {
            goingLeft = wasGoingLeft;
            rb.velocity = currVelocity;
            sprite.transform.localScale = new Vector3(rb.velocity.x / Mathf.Abs(rb.velocity.x), 1f, 0);
        }
        
        shooting = false;
        lastShot = Time.time;
    }

    private void SpawnArrow(Vector2 fireDirection)
    {
        if (isDead)
        {
            return;
        }

        fireDirection.Normalize();

        GameObject arrow = Instantiate(arrowPrefab, gameObject.transform.position + (Vector3)(fireDirection), Quaternion.identity);
        float angle = Mathf.Atan2(fireDirection.y, fireDirection.x) * Mathf.Rad2Deg;
        arrow.transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
        arrow.GetComponent<Rigidbody2D>().velocity = fireDirection * arrow.GetComponent<ArrowFlight>().arrowSpeed;
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

			if (goingLeft)
			{
				transform.position += new Vector3(0.6f, -0.1f, 0);
			}
			else
			{
				transform.position += new Vector3(-0.6f, -0.1f, 0);
			}
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

    private void dropItem(string itemType)
    {
        GameObject pot = Instantiate(Resources.Load(itemType), gameObject.transform.position, Quaternion.identity) as GameObject;
    }

    //private void FixedUpdate()
    //{
    //    hit = Physics2D.Raycast(transform.position + Vector3.down * 0.9f, Vector2.down, 0.1f);

    //    if (hit && hit.collider != null)
    //    {
    //        if (!grounded)
    //        {
    //            Vector2 pos = rb.position;
    //            pos.y = hit.collider.gameObject.transform.position.y + freezeHeight;
    //            rb.position = pos;
    //            rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
    //            grounded = true;
    //        }
    //    }
    //    else
    //    {
    //        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    //        grounded = false;
    //    }
    //}

    void Update ()
    {
        if (isDead) {
		    if (Time.time > deathTime + animator.GetCurrentAnimatorStateInfo(0).length)
		    {
                if (shouldDropHealthPot)
                {
                    dropItem("healthPot");
                }
			    Destroy(gameObject);
		    }
	    }
		else
        {
            if (shooting)
            {
                Vector2 direction = thePlayer.transform.position - gameObject.transform.position;
                if (direction.x / Mathf.Abs(direction.x) != sprite.transform.localScale.x)
                {
                    sprite.transform.localScale = new Vector3(direction.x / Mathf.Abs(direction.x), 1f, 0);
                    goingLeft = !goingLeft;
                }
            }
            //else if (!grounded)
            //{
            //    if (goingLeft)
            //    {
            //        Vector2 v = rb.velocity;
            //        v.x = -moveSpeed;
            //        rb.velocity = v;
            //    }
            //    else
            //    {
            //        Vector2 v = rb.velocity;
            //        v.x = moveSpeed;
            //        rb.velocity = v;
            //    }
            //}
            else if (rb.velocity == Vector2.zero)
            {
                if (goingLeft)
                {
                    Vector2 v = rb.velocity;
                    v.x = moveSpeed;
                    rb.velocity = v;
                    goingLeft = false;
                    sprite.transform.localScale = new Vector3(1f, 1f, 0);
                }
                else
                {
                    Vector2 v = rb.velocity;
                    v.x = -moveSpeed;
                    rb.velocity = v;
                    goingLeft = true;
                    sprite.transform.localScale = new Vector3(-1f, 1f, 0);
                }
            }

            distanceToPlayer = Vector2.Distance(gameObject.transform.position, thePlayer.transform.position);
            if (distanceToPlayer < 10f && !shooting && Time.time > lastShot + shootCooldown)
            {
                shooting = true;
                StartCoroutine(ShootPlayer());
            }
        }
    }
}