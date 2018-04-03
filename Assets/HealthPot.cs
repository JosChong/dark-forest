using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPot : MonoBehaviour {


    private int amountToGive = 10;
    private float movementSpeed = 0.25f;
    private Rigidbody2D rb;
    private float originalY;
    private float yThresh = 0.2f;
	// Use this for initialization
	void Start () {
        rb = gameObject.GetComponent<Rigidbody2D>();
        originalY = gameObject.transform.position.y;
	}
	
	// Update is called once per frame
	void Update () {
        if (rb.velocity == Vector2.zero || (gameObject.transform.position.y <= originalY))
        {
            rb.velocity = Vector2.up * movementSpeed;
        }
        else if (gameObject.transform.position.y > originalY + yThresh)
        {
            rb.velocity = Vector2.down * movementSpeed;
        }
		
	}

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.GetComponent<Player>() != null)
        {
            if (coll.gameObject.GetComponent<Player>().GiveHealth(amountToGive))
            {
                Destroy(gameObject);
            }
            
        }

    }

    //void OnTriggerEnter2D(Collider2D coll)
    //{

    //}
}
