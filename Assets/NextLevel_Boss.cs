using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel_Boss : MonoBehaviour {

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
			
			Application.LoadLevel("BossLevel");
//			Scene hi = SceneManager.GetActiveScene();
////			
//			if (hi.Equals("New Scene"))
//			{
//				Application.LoadLevel("MainLevel 1");
//			}
//			else if(hi.Equals("MainLevel 1"))
//			{
////				SceneManager.LoadScene("BossLevel");
//				Application.LoadLevel("BossLevel");
//			}
			
			
		}

		
	}
}

