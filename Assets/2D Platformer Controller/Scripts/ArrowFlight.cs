using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowFlight : MonoBehaviour
{
    public float arrowSpeed = 15;
    private int damageAmount = 5;

    // Use this for initialization
    void Start ()
	{
	    
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.GetComponent<Player>() != null)
        {
            coll.gameObject.GetComponent<Player>().TakeDamage(damageAmount);
            Destroy(gameObject);
        }
        if (coll.gameObject.GetComponent<SpiderEnemy>() != null)
        {
            coll.gameObject.GetComponent<SpiderEnemy>().DealDamage(damageAmount);
            Destroy(gameObject);
        }
        else if (coll.gameObject.GetComponent<SkeletonEnemy>() != null)
        {
            return;
        }
        else if (coll.gameObject.GetComponent<DragonController>() != null)
        {
            coll.gameObject.GetComponent<DragonController>().DealDamage(damageAmount);
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

}
