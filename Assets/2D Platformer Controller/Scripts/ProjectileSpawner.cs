using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpawner : MonoBehaviour {
	private GameObject rangedFire;
	private GameObject rangedAltFire;
    private GameObject deflectedArrow;
    
    private AudioClip[] boltSoundEffects;
    private AudioClip[] boltHitSoundEffects;
    private AudioClip[] orbSoundEffects;
    private AudioClip[] orbHitSoundEffects;

    void Start() {
		rangedFire = (GameObject) Resources.Load("RangedFire");
		rangedAltFire = (GameObject) Resources.Load("RangedAltFire");
        deflectedArrow = (GameObject) Resources.Load("DeflectedArrow");

        boltSoundEffects = new AudioClip[3];
        boltSoundEffects[0] = (AudioClip )Resources.Load("sound_effects/Bolt0");
        boltSoundEffects[1] = (AudioClip) Resources.Load("sound_effects/Bolt1");
        boltSoundEffects[2] = (AudioClip) Resources.Load("sound_effects/Bolt2");

        boltHitSoundEffects = new AudioClip[3];
        boltHitSoundEffects[0] = (AudioClip) Resources.Load("sound_effects/BoltHit0");
        boltHitSoundEffects[1] = (AudioClip) Resources.Load("sound_effects/BoltHit1");
        boltHitSoundEffects[2] = (AudioClip) Resources.Load("sound_effects/BoltHit2");

        orbSoundEffects = new AudioClip[3];
        orbSoundEffects[0] = (AudioClip) Resources.Load("sound_effects/Orb0");
        orbSoundEffects[1] = (AudioClip) Resources.Load("sound_effects/Orb1");
        orbSoundEffects[2] = (AudioClip) Resources.Load("sound_effects/Orb2");

        orbHitSoundEffects = new AudioClip[3];
        orbHitSoundEffects[0] = (AudioClip) Resources.Load("sound_effects/OrbHit0");
        orbHitSoundEffects[1] = (AudioClip) Resources.Load("sound_effects/OrbHit1");
        orbHitSoundEffects[2] = (AudioClip) Resources.Load("sound_effects/OrbHit2");
    }

	public void Fire(Vector3 position) {
		// Create normalized vector from player position to mouse click position
		Vector3 mousePosition = Camera.main.ScreenToWorldPoint(position);
		Vector2 fireDirection = mousePosition - transform.position;
		fireDirection.Normalize();
		
		// Instantiate bullet at player position
		GameObject rf = Instantiate (rangedFire, transform.position + (Vector3)(fireDirection), Quaternion.identity);
		// Change orientation of bullet to match firing direction
		float angle = Mathf.Atan2(fireDirection.y, fireDirection.x) * Mathf.Rad2Deg;
		rf.transform.rotation = Quaternion.AngleAxis(angle + 90, Vector3.forward);
		// Add velocity to bullet
		rf.GetComponent<Rigidbody2D>().velocity = fireDirection * rf.GetComponent<RangedFire>().fireSpeed;
	}
	
	public void AltFire(Vector3 position) {
		Vector3 mousePosition = Camera.main.ScreenToWorldPoint(position);
		Vector2 fireDirection = mousePosition - transform.position;
		fireDirection.Normalize();
		
		float angle = Mathf.Atan2(fireDirection.y, fireDirection.x) * Mathf.Rad2Deg;
		GameObject raf = Instantiate (rangedAltFire, transform.position + (Vector3)(fireDirection), Quaternion.identity);
		raf.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
		raf.GetComponent<Rigidbody2D>().velocity = fireDirection * raf.GetComponent<RangedFire>().fireSpeed;
	}

    public void DeflectFire(Vector3 origin, Vector3 position/*, Vector2 direction*/)
    {
        // Give original arrow velocity as argument and uncomment to deflect arrows in the opposite direction
        //Vector2 fireDirection = direction * -1;
        //fireDirection.Normalize();

        // Otherwise using mouse input as arrow trajectory
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(position);
        Vector2 fireDirection = mousePosition - transform.position;
        fireDirection.Normalize();

        float angle = Mathf.Atan2(fireDirection.y, fireDirection.x) * Mathf.Rad2Deg;
        GameObject da = Instantiate(deflectedArrow, origin, Quaternion.identity);
        da.transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
        da.GetComponent<Rigidbody2D>().velocity = fireDirection * da.GetComponent<RangedFire>().fireSpeed;
    }

    public AudioClip BoltSoundEffect()
    {
        int clip = Random.Range(0, boltSoundEffects.Length);
        return boltSoundEffects[clip];
    }

    public AudioClip BoltHitSoundEffect()
    {
        int clip = Random.Range(0, boltHitSoundEffects.Length);
        return boltHitSoundEffects[clip];
    }

    public AudioClip OrbSoundEffect()
    {
        int clip = Random.Range(0, orbSoundEffects.Length);
        return orbSoundEffects[clip];
    }

    public AudioClip OrbHitSoundEffect()
    {
        int clip = Random.Range(0, orbHitSoundEffects.Length);
        return orbHitSoundEffects[clip];
    }

    // Controller Support
    /*
	public void ControllerFire(Vector2 direction) {
		// Create normalized vector from player position to mouse click position
		Vector2 fireDirection = direction;
		fireDirection.Normalize();
		
		// Instantiate bullet at player position
		GameObject b = Instantiate (bullet, transform.position + (Vector3)(fireDirection), Quaternion.identity);
		// Change orientation of bullet to match firing direction
		float angle = Mathf.Atan2(fireDirection.y, fireDirection.x) * Mathf.Rad2Deg;
		b.transform.rotation = Quaternion.AngleAxis(angle + 90, Vector3.forward);
		// Add velocity to bullet
		b.GetComponent<Rigidbody2D>().velocity = fireDirection * b.GetComponent<Bullet>().fireSpeed;
	}
	
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
