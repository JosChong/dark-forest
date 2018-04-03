using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObstructionController : MonoBehaviour {
	
	private Rigidbody2D rb;
	
	public Slider[] sliders;
	public Slider healthslider;
	
	// Use this for initialization
	void Start () {
		
		rb = GetComponent<Rigidbody2D>();
		sliders = FindObjectsOfType<Slider>();
		
		foreach (Slider s in sliders)
		{
			if (s.CompareTag("HealthBar"))
			{
				healthslider = s;
			}
		}
		
	}
	
	void OnCollisionEnter2D(Collision2D coll)
	{

		// If colliding with the player
		if (coll.gameObject.GetComponent<Player>() != null)
		{
			coll.gameObject.GetComponent<Player>().TakeDamage(15);
			
		}

		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
