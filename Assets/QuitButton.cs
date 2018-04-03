using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuitButton : MonoBehaviour
{

    public Button thisButton;

    // Use this for initialization
    void Start()
    {
        thisButton = gameObject.GetComponent<Button>();

        thisButton.onClick.AddListener(quitGame);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void quitGame()
    {
        System.Diagnostics.Process.GetCurrentProcess().Kill();
    }
}
