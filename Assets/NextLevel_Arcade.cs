using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextLevel_Arcade : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnCollisionEnter2D(Collision2D coll)
	{

		// If colliding with the player
		if (coll.gameObject.GetComponent<Player>() != null)
		{
			
			Application.LoadLevel("ArcadeLevel");
			
		}

		
	}
}
