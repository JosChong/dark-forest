using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public int travelSpeed = 5;
    public int damageDealt = 20;

    private AudioSource audio;
    private DragonProjectileSpawner spawner;
    private float fireTime;
    private float clipLength;

    void Start()
    {
        audio = GetComponent<AudioSource>();
        spawner = FindObjectOfType<DragonController>().GetComponent<DragonProjectileSpawner>();
        audio.volume = 0.2f;
        audio.clip = spawner.FireballSoundEffect();
        audio.Play();

        fireTime = Time.time;
        clipLength = audio.clip.length;
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.GetComponent<Player>() != null)
        {
            HitSoundEffect(coll);
            coll.gameObject.GetComponent<Player>().TakeDamage(damageDealt);
        }
        if (coll.gameObject.GetComponent<SpiderEnemy>() != null)
        {
            HitSoundEffect(coll);
            coll.gameObject.GetComponent<SpiderEnemy>().DealDamage(damageDealt);
        }
        else if (coll.gameObject.GetComponent<SkeletonEnemy>() != null)
        {
            HitSoundEffect(coll);
            coll.gameObject.GetComponent<SkeletonEnemy>().DealDamage(damageDealt);
        }
        else if (coll.gameObject.GetComponent<DragonController>() != null)
        {
            return;
        }
        else if (coll.gameObject.GetComponent<ArrowFlight>() != null)
        {
            Destroy(coll.gameObject);
        }
        else if (coll.gameObject.GetComponent<MeleeDamageDealer>() != null)
        {
            return;
        }
        else
        {
            StartCoroutine(coDie());
        }
    }

    void HitSoundEffect(Collider2D coll)
    {
        AudioSource collAudio = coll.gameObject.GetComponent<AudioSource>();
        collAudio.volume = 0.4f;
        collAudio.clip = spawner.FireballHitSoundEffect();
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
    void OnBecameInvisible()
    {
        if (this.gameObject.activeSelf)
        {
            StartCoroutine(coDie());
        }
    }
}
