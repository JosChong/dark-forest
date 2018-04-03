using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class DragonController : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    public int health = 2500;
    private int damageDealt = 50;
    public float moveSpeed = 0.5f;

    private Color origColor;

    public Slider[] sliders;
    public Slider healthslider;
    public Slider dragonHealthSlider;

    private bool isDead = false;
    private float deathTime;
    private Animator animator;
    private PolygonCollider2D collider;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();

        sliders = FindObjectsOfType<Slider>();
        foreach (Slider s in sliders)
        {
            if (s.CompareTag("HealthBar"))
            {
                healthslider = s;
            }
            if (s.CompareTag("BossHealthBar"))
            {
                dragonHealthSlider = s;
            }
        }

        rb.velocity = Vector2.left * moveSpeed;

        origColor = gameObject.GetComponentInChildren<SpriteRenderer>().color;

        animator = GetComponent<Animator>();
        collider = GetComponent<PolygonCollider2D>();
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        // If colliding with the player
        if (coll.gameObject.GetComponent<Player>() != null)
        {
            coll.gameObject.GetComponent<Player>().tintPlayer();
            healthslider.value -= damageDealt;
            coll.gameObject.GetComponent<Player>().health -= damageDealt;
        }
    }

    public void DealDamage(int damage)
    {
        health -= damage;
        dragonHealthSlider.value -= damage;
        if (health <= 0)
        {
            GetComponent<DragonProjectileSpawner>().enabled = false;
            deathTime = Time.time;
            animator.SetTrigger("isDead");
            collider.enabled = false;
            isDead = true;
            rb.velocity = Vector2.zero;
            rb.constraints = RigidbodyConstraints2D.None;
            rb.gravityScale = 2;

            EnemySpawner[] spawners = FindObjectsOfType<EnemySpawner>();
            foreach (EnemySpawner spawner in spawners)
            {
                spawner.enabled = false;
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

    void Update()
    {
        if (isDead)
        {
            if (Time.time > deathTime + animator.GetCurrentAnimatorStateInfo(0).length)
            {
                Destroy(gameObject);
                Victory();
            }

            return;
        }
        else if (transform.position.x < 35)
        {
            rb.velocity = Vector2.right * moveSpeed;
            sprite.transform.localScale = new Vector3(1f, 1f, 0);
        }
        else if (transform.position.x > 50)
        {
            rb.velocity = Vector2.left * moveSpeed;
            sprite.transform.localScale = new Vector3(-1f, 1f, 0);
        }

        if (health >= 1200)
        {
            moveSpeed = 0.5f;
        }
        else if (health >= 400)
        {
            moveSpeed = 1.5f;
            if (sprite.transform.localScale.x > 0)
            {
                rb.velocity = Vector2.right * moveSpeed;
            }
            else
            {
                rb.velocity = Vector2.left * moveSpeed;
            }
        }
        else
        {
            moveSpeed = 2.5f;
            if (sprite.transform.localScale.x > 0)
            {
                rb.velocity = Vector2.right * moveSpeed;
            }
            else
            {
                rb.velocity = Vector2.left * moveSpeed;
            }
        }
    }

    void Victory()
    {
        Instantiate(Resources.Load("menus/victoryMessage"), GameObject.FindWithTag("MainCanvas").transform);
        AudioSource bgm = GameObject.FindGameObjectWithTag("BackgroundMusic").GetComponent<AudioSource>();
        bgm.clip = (AudioClip) Resources.Load("music/VictoryMusic");
        bgm.volume = 0.4f;
        bgm.Play();
        FindObjectOfType<Player>().playerActive = false;
        Time.timeScale = 0;
    }
}