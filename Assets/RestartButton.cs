using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Button))]
public class RestartButton : MonoBehaviour
{
	private Scene currentScene;
	private GameObject p;
	private GameObject b;

	private Button restartButton;
	// private GameObject b = GameObject.FindGameObjectWithTag("RestartButton").GetComponent<Button>();

	// Use this for initialization
	void Start()
	{
		p = GameObject.FindGameObjectWithTag("Player");
		b = GameObject.FindGameObjectWithTag("RestartButton");
		restartButton = b.GetComponent<Button>();
		restartButton.onClick.AddListener(TaskOnClick);
		//b.SetActive(false);
		//b.GetComponent<Image>().color.a = 0;
	}

	void Update()
	{

	}

	void TaskOnClick()
	{
	    currentScene = SceneManager.GetActiveScene();
        // Application.LoadLevel(Application.loadedLevel); // loads scene to initial but doesn't restart everything 
        if (currentScene.name == "Tutorial")
		{
			SceneManager.LoadScene("Tutorial"); // Same issue	
		} else if (currentScene.name == "BossLevel")
		{
			SceneManager.LoadScene("BossLevel"); // Same issue
		} else if (currentScene.name == "ArcadeLevel")
		{
			SceneManager.LoadScene("ArcadeLevel"); // Same issue
		}
		
		Time.timeScale = 1f;
		p.GetComponent<Player>().isAlive = true;
		p.GetComponent<PlayerHealth>().currentHealth = 100;
		p.GetComponent<PlayerInput>().Restart();
		// TODO make a reset alive function
	}

}
	
