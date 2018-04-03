using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dummyEnemy : MonoBehaviour
{
    public Color origColor;

    public bool hasBeenHitMelee;

    public bool hasBeenHitRanged;

    public bool hasBeenHitSpecial;

    private tutorialController tut;
    // Use this for initialization
    void Start ()
	{
	    origColor = gameObject.GetComponentInChildren<SpriteRenderer>().color;
	    tut = FindObjectOfType<tutorialController>();
	    hasBeenHitMelee = false;
	    hasBeenHitRanged = false;
	    hasBeenHitSpecial = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void DealDamage(string attackType)
    {
        StartCoroutine(coTintEnemy());

        if (attackType == "Melee" && hasBeenHitMelee == false && tut.inCombatSection)
        {
            hasBeenHitMelee = true;
        }
        else if(attackType == "Ranged" && hasBeenHitRanged == false && tut.inCombatSection)
        {
            hasBeenHitRanged = true;
        }
        else if ((attackType == "AltRanged" || attackType == "AltMelee") && hasBeenHitSpecial == false &&
                 tut.inSpecialSection)
        {
            hasBeenHitSpecial = true;
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
}
