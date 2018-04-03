using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class StartGameButton : MonoBehaviour
{

    public Button ThisButton;

	// Use this for initialization
	void Start () {
	    ThisButton = gameObject.GetComponent<Button>();

	    if (ThisButton.transform.GetChild(0).GetComponent<Text>().text == "I've Never Played")
	    {
	        ThisButton.onClick.AddListener(startGameWithTut);
	    }
	    else
	    {
	        ThisButton.onClick.AddListener(startGameWithoutTut);
	    }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void startGameWithTut()
    {
        Time.timeScale = 1;
        Instantiate(Resources.Load("tutController"));
        Destroy(transform.parent.gameObject);
    }

    void startGameWithoutTut()
    {
        Time.timeScale = 1;
        //GameObject tut = GameObject.FindObjectOfType<tutorialController>().gameObject;
        //Destroy(tut);
        EnemySpawner[] spawners = FindObjectsOfType<EnemySpawner>();
        foreach (EnemySpawner s in spawners)
        {
            s.spawnerActive = true;
        }
        Destroy(transform.parent.gameObject);
    }
}
