using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeginTutButton : MonoBehaviour
{

    public Button thisButton;

    // Use this for initialization
    void Start()
    {
        thisButton = gameObject.GetComponent<Button>();

        thisButton.onClick.AddListener(startTut);

        FindObjectOfType<Player>().playerActive = false;
        Time.timeScale = 0;

        EnemySpawner[] spawners = FindObjectsOfType<EnemySpawner>();
        foreach (EnemySpawner s in spawners)
        {
            s.spawnerActive = false;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void startTut()
    {
        GameObject tutMenu = Instantiate(Resources.Load("menus/controlsMenu"), GameObject.FindWithTag("MainCanvas").transform) as GameObject;
        Destroy(transform.parent.gameObject);

    }
}
