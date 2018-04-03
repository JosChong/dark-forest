using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedFire : MonoBehaviour {
	public int fireSpeed = 15;
    public int damageDealt = 10;
	public bool passThroughEnemies = false;
	public string attackType;
	private ResourceBarController resourceBar;
	private SpecialAttackBarController specialBar;
    private ProjectileSpawner spawner;

    private AudioSource audio;
    private float fireTime;
    private float clipLength;

    void Start()
    {
        audio = GetComponent<AudioSource>();
        spawner = FindObjectOfType<Player>().GetComponent<ProjectileSpawner>();

        if (attackType == "AltRanged")
        {
            audio.clip = spawner.OrbSoundEffect();
        }
        else
        {
            audio.clip = spawner.BoltSoundEffect();
        }
        audio.Play();
        fireTime = Time.time;
        clipLength = audio.clip.length;

        resourceBar = FindObjectOfType<ResourceBarController>();
		specialBar = FindObjectOfType<SpecialAttackBarController>();
    }
	
	void OnTriggerEnter2D(Collider2D coll)
	{
        // Deal damage and add special charge on collision
        if (coll.gameObject.GetComponent<SpiderEnemy>() != null)
        {
            HitSoundEffect(coll);

            coll.gameObject.GetComponent<SpiderEnemy>().DealDamage(damageDealt);
            resourceBar.UpdateBar(attackType);
            specialBar.AddCharge(1);
        }
        else if (coll.gameObject.GetComponent<SkeletonEnemy>() != null)
        {
            HitSoundEffect(coll);

            coll.gameObject.GetComponent<SkeletonEnemy>().DealDamage(damageDealt);
            resourceBar.UpdateBar(attackType);
            specialBar.AddCharge(1);
        }
        else if (coll.gameObject.GetComponent<FastEnemy>() != null)
        {
            HitSoundEffect(coll);

            coll.gameObject.GetComponent<FastEnemy>().DealDamage(damageDealt);
            resourceBar.UpdateBar(attackType);
            specialBar.AddCharge(1);
        }
        else if (coll.gameObject.GetComponent<DragonController>() != null)
        {
            HitSoundEffect(coll);

            coll.gameObject.GetComponent<DragonController>().DealDamage(damageDealt);
            resourceBar.UpdateBar(attackType);
            specialBar.AddCharge(1);
        }
        else if (coll.gameObject.GetComponent<EnemySpawner>() != null)
        {
            HitSoundEffect(coll);

            coll.gameObject.GetComponent<EnemySpawner>().DealDamage(damageDealt);
            resourceBar.UpdateBar(attackType);
            specialBar.AddCharge(1);
        }
        else if (coll.gameObject.GetComponent<dummyEnemy>() != null)
        {
            HitSoundEffect(coll);
            coll.gameObject.GetComponent<dummyEnemy>().DealDamage(attackType);
            resourceBar.UpdateBar(attackType);
            specialBar.AddCharge(1);
        }
		else
		{
            StartCoroutine(coDie());
        }
	}

    void HitSoundEffect(Collider2D coll)
    {
        AudioSource collAudio = coll.gameObject.GetComponent<AudioSource>();

        if (attackType == "AltRanged")
        {
            collAudio.volume = 1;
            collAudio.clip = spawner.OrbHitSoundEffect();
        }
        else
        {
            collAudio.volume = 0.2f;
            collAudio.clip = spawner.BoltHitSoundEffect();
            StartCoroutine(coDie());
        }

        collAudio.Play();
    }

    private IEnumerator coDie()
    {
        if (Time.time > fireTime + clipLength)
        {
            Destroy(gameObject);
        }
        else
        {
            GetComponent<PolygonCollider2D>().enabled = false;
            GetComponent<SpriteRenderer>().enabled = false;
            yield return new WaitForSeconds(1);
            StartCoroutine(coDie());
        }
    }
	
	// Destroy self when not on screen
	void OnBecameInvisible() {
        if (this.gameObject.activeSelf)
        {
            StartCoroutine(coDie());
        }
    }
}
