using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class resumeGame : MonoBehaviour
{

    private Button resume;
	// Use this for initialization
	void Start ()
	{
	    resume = gameObject.GetComponent<Button>();
        resume.onClick.AddListener(ResumeGameFunc);
        FindObjectOfType<Player>().playerActive = false;
        Time.timeScale = 0;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void ResumeGameFunc()
    {
        FindObjectOfType<Player>().playerActive = true;
        Destroy(transform.parent.gameObject);
    }
}
