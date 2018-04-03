using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class SpiderEnemy : MonoBehaviour {
	private Rigidbody2D rb;
	private SpriteRenderer sprite;
	private int health = 150;
	private int damageDealt = 5;
	private float moveSpeed = 1.2f;
	private bool goingLeft = true;

    private Color origColor;

    public Slider[] sliders;
	public Slider healthslider;

	private bool isDead = false;
	private float deathTime;
	private Animator animator;
	private PolygonCollider2D collider;

    public bool shouldDropHealthPot = false;

    public float freezeHeight;

    RaycastHit2D hit;
    private bool grounded = false;

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

        int potChance = Random.Range(0, 8);
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
    //    hit = Physics2D.Raycast(transform.position + Vector3.down * 0.4f, Vector2.down, 0.1f);

    //    if (hit && hit.collider != null && hit.collider.gameObject.layer == 9)
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

    void Update () {
        if (isDead)
        {
            if (Time.time > deathTime + animator.GetCurrentAnimatorStateInfo(0).length)
            {
                if (shouldDropHealthPot)
                {
                    dropItem("healthPot");
                }
                Destroy(gameObject);
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
                sprite.transform.localScale = new Vector3(0.6f, 0.6f, 0);
            }
            else
            {
                Vector2 v = rb.velocity;
                v.x = -moveSpeed;
                rb.velocity = v;
                goingLeft = true;
                sprite.transform.localScale = new Vector3(-0.6f, 0.6f, 0);
            }
        }
    }
}