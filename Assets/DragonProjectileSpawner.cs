using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonProjectileSpawner : MonoBehaviour
{
    private GameObject fireball;
    private float lastShot;
    private float shotInterval = 5;

    private AudioClip[] fireballSoundEffects;
    private AudioClip[] fireballHitSoundEffects;

    void Start()
    {
        fireball = (GameObject)Resources.Load("Fireball");
        lastShot = Time.time;

        fireballSoundEffects = new AudioClip[6];
        fireballSoundEffects[0] = (AudioClip) Resources.Load("sound_effects/Fireball0");
        fireballSoundEffects[1] = (AudioClip) Resources.Load("sound_effects/Fireball1");
        fireballSoundEffects[2] = (AudioClip) Resources.Load("sound_effects/Fireball2");
        fireballSoundEffects[3] = (AudioClip) Resources.Load("sound_effects/Fireball3");
        fireballSoundEffects[4] = (AudioClip) Resources.Load("sound_effects/Fireball4");
        fireballSoundEffects[5] = (AudioClip) Resources.Load("sound_effects/Fireball5");

        fireballHitSoundEffects = new AudioClip[3];
        fireballHitSoundEffects[0] = (AudioClip) Resources.Load("sound_effects/FireballHit0");
        fireballHitSoundEffects[1] = (AudioClip) Resources.Load("sound_effects/FireballHit1");
        fireballHitSoundEffects[2] = (AudioClip) Resources.Load("sound_effects/FireballHit2");
    }

    private void Update()
    {
        if (Time.time > lastShot + shotInterval)
        {
            lastShot = Time.time;

            int dragonHealth = GetComponent<DragonController>().health;
            if (dragonHealth >= 1500)
            {
                DragonFire();
            }
            else if (dragonHealth >= 500)
            {
                StartCoroutine(coTripleDragonFire());
            }
            else
            {
                StartCoroutine(coDragonWheel());
            }
        }
    }

    private void DragonFire(int offset = 0)
    {
        Vector2 fireDirection = Quaternion.AngleAxis(offset, Vector3.forward) * Vector2.down * 2;
        for (int i = 0; i < 8; i++)
        {
            GameObject f = Instantiate(fireball, transform.position + (Vector3)(fireDirection), Quaternion.identity);
            float angle = Mathf.Atan2(fireDirection.y, fireDirection.x) * Mathf.Rad2Deg;
            f.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            f.GetComponent<Rigidbody2D>().velocity = fireDirection * f.GetComponent<Fireball>().travelSpeed;

            fireDirection = Quaternion.AngleAxis(45, Vector3.forward) * fireDirection;
        }
    }

    private IEnumerator coTripleDragonFire()
    {
        DragonFire();
        yield return new WaitForSeconds(0.25f);
        DragonFire();
        yield return new WaitForSeconds(0.25f);
        DragonFire();
    }

    private IEnumerator coDragonWheel()
    {
        DragonFire();
        yield return new WaitForSeconds(0.25f);
        DragonFire(10);
        yield return new WaitForSeconds(0.25f);
        DragonFire(20);
        yield return new WaitForSeconds(0.25f);
        DragonFire(30);
        yield return new WaitForSeconds(0.25f);
        DragonFire(40);
    }


    public AudioClip FireballSoundEffect()
    {
        int clip = Random.Range(0, fireballSoundEffects.Length);
        return fireballSoundEffects[clip];
    }

    public AudioClip FireballHitSoundEffect()
    {
        int clip = Random.Range(0, fireballHitSoundEffects.Length);
        return fireballHitSoundEffects[clip];
    }

    // Controller Support
    /*
	
	
	public void ControllerAltFire(Vector2 direction) {
		Vector2 fireDirection = direction;
		fireDirection.Normalize();
		
		float angle = Mathf.Atan2(fireDirection.y, fireDirection.x) * Mathf.Rad2Deg;
		float angle2 = (angle + 10) * Mathf.Deg2Rad;
		float angle3 = (angle - 10) * Mathf.Deg2Rad;
		
		Vector2 fireDirection2 = new Vector2(Mathf.Cos(angle2), Mathf.Sin(angle2));
		Vector2 fireDirection3 = new Vector2(Mathf.Cos(angle3), Mathf.Sin(angle3));
		
		GameObject b = Instantiate (bullet, transform.position + (Vector3)(fireDirection), Quaternion.identity);
		GameObject b2 = Instantiate (bullet, transform.position + (Vector3)(fireDirection2), Quaternion.identity);
		GameObject b3 = Instantiate (bullet, transform.position + (Vector3)(fireDirection3), Quaternion.identity);

		b.transform.rotation = Quaternion.AngleAxis(angle + 90, Vector3.forward);
		b2.transform.rotation = Quaternion.AngleAxis(angle + 100, Vector3.forward);
		b3.transform.rotation = Quaternion.AngleAxis(angle + 80, Vector3.forward);

		b.GetComponent<Rigidbody2D>().velocity = fireDirection * b.GetComponent<Bullet>().fireSpeed;
		b2.GetComponent<Rigidbody2D>().velocity = fireDirection2 * b2.GetComponent<Bullet>().fireSpeed;
		b3.GetComponent<Rigidbody2D>().velocity = fireDirection3 * b3.GetComponent<Bullet>().fireSpeed;
	}
	*/
}
