using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeDamageDealer : MonoBehaviour
{
    public Player player;
	private Animator animator;
	private float animationTime;
	private float startTime;
	public int damageDealt;
	public string attackType;
	private ResourceBarController resourceBar;
	private SpecialAttackBarController specialBar;
    private MeleeSpawner spawner;
    private AudioSource audio;

	// Use this for initialization
	void Start ()
	{
        player = GameObject.FindObjectOfType<Player>();
        spawner = player.GetComponent<MeleeSpawner>();
        audio = GetComponent<AudioSource>();

        if (attackType == "AltMelee")
        {
            audio.clip = spawner.SmiteSoundEffect();
        }
        else
        {
            audio.clip = spawner.SlashSoundEffect();
        }
        audio.Play();
        
        resourceBar = FindObjectOfType<ResourceBarController>();
        specialBar = FindObjectOfType<SpecialAttackBarController>();
		
		animator = GetComponent<Animator>();
		animationTime = animator.GetCurrentAnimatorStateInfo(0).length;

        startTime = Time.time;
    }
	
	void Update () {
        // If the player changes direction while the spell is active, the spell flips to match
        // must uncomment to use Melee()
        /*
		Vector3 p = gameObject.transform.localPosition;
	    if ((player.lastLookDir < 0 && p.x > 0) || (player.lastLookDir > 0 && p.x < 0))
	    {
		    gameObject.transform.localPosition = new Vector3(-p.x, p.y, p.z);
		    Vector3 s = gameObject.transform.localScale;
		    gameObject.transform.localScale = new Vector3(-s.x, s.y, s.z);
	    }
        */
        
		if (Time.time > startTime + animationTime)
		{
			Destroy(gameObject);
		}
	}

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.GetComponent<SpiderEnemy>()!= null)
        {
            HitSoundEffect(true, coll);

            coll.gameObject.GetComponent<SpiderEnemy>().DealDamage(damageDealt);
	        resourceBar.UpdateBar(attackType);
	        specialBar.AddCharge(5);
        }
        else if (coll.gameObject.GetComponent<SkeletonEnemy>() != null)
        {
            HitSoundEffect(true, coll);

            coll.gameObject.GetComponent<SkeletonEnemy>().DealDamage(damageDealt);
	        resourceBar.UpdateBar(attackType);
	        specialBar.AddCharge(5);
        }
        else if (coll.gameObject.GetComponent<FastEnemy>() != null)
        {
            HitSoundEffect(true, coll);

            coll.gameObject.GetComponent<FastEnemy>().DealDamage(damageDealt);
	        resourceBar.UpdateBar(attackType);
	        specialBar.AddCharge(5);
        }
        else if (coll.gameObject.GetComponent<DragonController>() != null)
        {
            HitSoundEffect(true, coll);

            coll.gameObject.GetComponent<DragonController>().DealDamage(damageDealt);
            resourceBar.UpdateBar(attackType);
            specialBar.AddCharge(5);
        }
        else if (coll.gameObject.GetComponent<ArrowFlight>() != null)
        {
            if (attackType == "Melee")
            {
                spawner.DeflectSoundEffect();
                Vector3 pos = coll.gameObject.transform.position;
                //Vector2 dir = coll.gameObject.GetComponent<Rigidbody2D>().velocity;
                player.GetComponent<ProjectileSpawner>().DeflectFire(pos, Input.mousePosition);
            }
            
            Destroy(coll.gameObject);
        }
        else if (coll.gameObject.GetComponent<Fireball>() != null)
        {
            if (attackType == "AltMelee")
            {
                Destroy(coll.gameObject);
            }
        }
        else if (coll.gameObject.GetComponent<EnemySpawner>() != null)
        {
            HitSoundEffect(false, coll);

            coll.gameObject.GetComponent<EnemySpawner>().DealDamage(damageDealt);
	        resourceBar.UpdateBar(attackType);
	        specialBar.AddCharge(5);
        }
        else if (coll.gameObject.GetComponent<dummyEnemy>() != null)
        {
            HitSoundEffect(true, coll);

            coll.gameObject.GetComponent<dummyEnemy>().DealDamage(attackType);
            resourceBar.UpdateBar(attackType);
            specialBar.AddCharge(5);
        }
    }

    void HitSoundEffect(bool lifeSteal, Collider2D coll)
    {
        AudioSource collAudio = coll.gameObject.GetComponent<AudioSource>();

        if (attackType == "AltMelee")
        {
            collAudio.volume = 1;
            collAudio.clip = spawner.SmiteHitSoundEffect();
            if (lifeSteal)
            {
                FindObjectOfType<Player>().GiveHealth(damageDealt / 10);
            }
        }
        else
        {
            collAudio.volume = 1;
            collAudio.clip = spawner.SlashHitSoundEffect();
        }

        collAudio.Play();
    }
}
