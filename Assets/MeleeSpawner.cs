using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeSpawner : MonoBehaviour {
	private GameObject meleeSpell;
	private GameObject meleeAltSpell;
    
    private AudioSource audio;
    private AudioClip[] slashSoundEffects;
    private AudioClip[] slashHitSoundEffects;
    private AudioClip[] deflectSoundEffects;
    private AudioClip[] smiteSoundEffects;
    private AudioClip[] smiteHitSoundEffects;

    void Start() {
		meleeSpell = (GameObject) Resources.Load("Slash");
		meleeAltSpell = (GameObject) Resources.Load("LightSpell");

        audio = GetComponent<AudioSource>();

        slashSoundEffects = new AudioClip[1];
        slashSoundEffects[0] = (AudioClip) Resources.Load("sound_effects/Slash0");

        slashHitSoundEffects = new AudioClip[2];
        slashHitSoundEffects[0] = (AudioClip) Resources.Load("sound_effects/SlashHit0");
        slashHitSoundEffects[1] = (AudioClip) Resources.Load("sound_effects/SlashHit1");

        deflectSoundEffects = new AudioClip[2];
        deflectSoundEffects[0] = (AudioClip) Resources.Load("sound_effects/Deflect0");
        deflectSoundEffects[1] = (AudioClip) Resources.Load("sound_effects/Deflect1");

        smiteSoundEffects = new AudioClip[3];
        smiteSoundEffects[0] = (AudioClip) Resources.Load("sound_effects/Smite0");
        smiteSoundEffects[1] = (AudioClip) Resources.Load("sound_effects/Smite1");
        smiteSoundEffects[2] = (AudioClip) Resources.Load("sound_effects/Smite2");

        smiteHitSoundEffects = new AudioClip[6];
        smiteHitSoundEffects[0] = (AudioClip) Resources.Load("sound_effects/SmiteHit0");
        smiteHitSoundEffects[1] = (AudioClip) Resources.Load("sound_effects/SmiteHit1");
        smiteHitSoundEffects[2] = (AudioClip) Resources.Load("sound_effects/SmiteHit2");
        smiteHitSoundEffects[3] = (AudioClip) Resources.Load("sound_effects/SmiteHit3");
        smiteHitSoundEffects[4] = (AudioClip) Resources.Load("sound_effects/SmiteHit4");
        smiteHitSoundEffects[5] = (AudioClip) Resources.Load("sound_effects/SmiteHit5");
    }

    public void Melee()
    {
        GameObject attack = Instantiate(meleeSpell, transform);
        attack.transform.localPosition = new Vector3(1, 0, 0);
    }

	public void DirectionalMelee(Vector3 position) {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(position);
        Vector2 fireDirection = mousePosition - transform.position;
        fireDirection.Normalize();
        
        GameObject f = Instantiate(meleeSpell, transform.position + (Vector3)(fireDirection), Quaternion.identity, transform);
        float angle = Mathf.Atan2(fireDirection.y, fireDirection.x) * Mathf.Rad2Deg;
        if (fireDirection.x < 0)
        {
            f.transform.localScale = new Vector3(1.5f, -1.5f, 0);
            f.transform.rotation = Quaternion.AngleAxis(angle - 20, Vector3.forward);
        } else
        {
            f.transform.rotation = Quaternion.AngleAxis(angle + 20, Vector3.forward);
        }
	}
	
	public void AltMelee() {
		Instantiate(meleeAltSpell, transform);
	}

    public AudioClip SlashSoundEffect()
    {
        int clip = Random.Range(0, slashSoundEffects.Length);
        return slashSoundEffects[clip];
    }

    public AudioClip SlashHitSoundEffect()
    {
        int clip = Random.Range(0, slashHitSoundEffects.Length);
        return slashHitSoundEffects[clip];
    }

    public void DeflectSoundEffect()
    {
        audio.volume = 0.2f;
        int clip = Random.Range(0, deflectSoundEffects.Length);
        audio.clip = deflectSoundEffects[clip];
        audio.Play();
    }

    public AudioClip SmiteSoundEffect()
    {
        int clip = Random.Range(0, smiteSoundEffects.Length);
        return smiteSoundEffects[clip];
    }

    public AudioClip SmiteHitSoundEffect()
    {
        int clip = Random.Range(0, smiteHitSoundEffects.Length);
        return smiteHitSoundEffects[clip];
    }
}
